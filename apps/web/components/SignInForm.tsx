"use client";

import { signIn } from "next-auth/react";
import { useLocale, useTranslations } from "next-intl";
import { useState } from "react";
import { useRouter } from "@/i18n/navigation";
import { googleAuthEnabled } from "@/lib/auth-config";

export function SignInForm() {
  const t = useTranslations("auth");
  const locale = useLocale();
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);

  async function onCredentials(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    const form = new FormData(e.currentTarget);
    const res = await signIn("credentials", {
      email: form.get("email"),
      password: form.get("password"),
      redirect: false,
    });
    if (res?.error) {
      setError(t("invalidCredentials"));
      return;
    }
    router.push("/account");
  }

  return (
    <div className="mx-auto max-w-md space-y-6 rounded-xl bg-white p-8 shadow-sm">
      <h1 className="font-serif text-3xl text-primary">{t("signInTitle")}</h1>
      {googleAuthEnabled && (
        <button
          type="button"
          onClick={() => signIn("google", { callbackUrl: `/${locale}/account` })}
          className="w-full rounded border border-primary/20 px-4 py-2 text-primary"
        >
          {t("google")}
        </button>
      )}
      <form onSubmit={onCredentials} className="space-y-4">
        <label className="block text-sm">
          {t("email")}
          <input name="email" type="email" required className="mt-1 w-full rounded border px-3 py-2" />
        </label>
        <label className="block text-sm">
          {t("password")}
          <input name="password" type="password" required className="mt-1 w-full rounded border px-3 py-2" />
        </label>
        <button type="submit" className="w-full rounded bg-primary px-4 py-2 text-primary-foreground">
          {t("submitSignIn")}
        </button>
      </form>
      {error && <p className="text-sm text-red-600">{error}</p>}
    </div>
  );
}
