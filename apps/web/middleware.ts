import createMiddleware from "next-intl/middleware";
import { NextResponse } from "next/server";
import { auth } from "@/auth";
import { routing } from "@/i18n/routing";

const intlMiddleware = createMiddleware(routing);

export default auth((req) => {
  const { pathname } = req.nextUrl;

  if (pathname.startsWith("/admin")) {
    const isLogin = pathname === "/admin/login";
    const role = req.auth?.role?.toLowerCase();
    const isAdmin = role === "admin" || role === "administrator";

    if (!isLogin && !isAdmin) {
      const url = req.nextUrl.clone();
      url.pathname = "/admin/login";
      return NextResponse.redirect(url);
    }
    return NextResponse.next();
  }

  const accountMatch = pathname.match(/^\/(pt|en)\/account(\/|$)/);
  if (accountMatch && !req.auth) {
    const locale = accountMatch[1];
    return NextResponse.redirect(new URL(`/${locale}/sign-in`, req.url));
  }

  return intlMiddleware(req);
});

export const config = {
  matcher: ["/((?!api|_next|_vercel|.*\\..*).*)"],
};
