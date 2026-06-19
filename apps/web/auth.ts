import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";
import Google from "next-auth/providers/google";
import { API_BASE_URL } from "@/lib/api-config";

type AuthTokens = {
  accessToken?: string;
  refreshToken?: string;
  role?: string;
  userId?: string;
  name?: string;
  email?: string;
};

async function exchangeGoogleIdToken(idToken: string): Promise<AuthTokens | null> {
  try {
    const res = await fetch(`${API_BASE_URL}/auth/sso`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ idToken }),
    });
    if (!res.ok) return null;
    return (await res.json()) as AuthTokens;
  } catch {
    return null;
  }
}

export const { handlers, auth, signIn, signOut } = NextAuth({
  providers: [
    Google({
      clientId: process.env.GOOGLE_CLIENT_ID,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET,
    }),
    Credentials({
      name: "credentials",
      credentials: {
        email: { label: "Email", type: "email" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials) {
        if (!credentials?.email || !credentials?.password) return null;

        try {
          const res = await fetch(`${API_BASE_URL}/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
              email: credentials.email,
              password: credentials.password,
            }),
          });

          if (!res.ok) return null;

          const data = (await res.json()) as AuthTokens;
          if (!data.accessToken) return null;

          return {
            id: data.userId ?? "user",
            userId: data.userId,
            email: credentials.email as string,
            name: data.name ?? "User",
            accessToken: data.accessToken,
            refreshToken: data.refreshToken,
            role: data.role ?? "",
          };
        } catch {
          return null;
        }
      },
    }),
  ],
  callbacks: {
    async jwt({ token, user, account }) {
      if (account?.provider === "google" && account.id_token) {
        const data = await exchangeGoogleIdToken(account.id_token);
        if (data?.accessToken) {
          token.accessToken = data.accessToken;
          token.refreshToken = data.refreshToken;
          token.role = data.role ?? "";
          token.userId = data.userId;
          token.sub = data.userId ?? token.sub;
        }
      }

      if (user) {
        const u = user as AuthTokens & { id?: string };
        token.accessToken = u.accessToken ?? token.accessToken;
        token.refreshToken = u.refreshToken ?? token.refreshToken;
        token.role = u.role ?? token.role ?? "";
        token.userId = u.userId ?? u.id ?? token.userId;
      }

      return token;
    },
    async session({ session, token }) {
      if (token.accessToken) session.accessToken = token.accessToken;
      if (token.refreshToken) session.refreshToken = token.refreshToken;
      if (token.role) session.role = token.role;
      if (token.userId) session.userId = token.userId;
      if (session.user && (token.userId || token.sub)) {
        session.user.id = token.userId ?? token.sub ?? "";
      }
      return session;
    },
  },
  session: {
    strategy: "jwt",
  },
  secret: process.env.NEXTAUTH_SECRET,
  trustHost: true,
});
