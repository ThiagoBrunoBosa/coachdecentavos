"use client";

import { useLocale, useTranslations } from "next-intl";
import { useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import { signIn } from "next-auth/react";
import { useRouter } from "@/i18n/navigation";

export function SignUpForm() {
  const t = useTranslations("auth");
  const locale = useLocale();
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    const form = new FormData(e.currentTarget);
    try {
      await apiFetchBrowser("/auth/register", {
        method: "POST",
        body: JSON.stringify({
          email: form.get("email"),
          password: form.get("password"),
          name: form.get("name"),
          preferredLocale: locale === "en" ? "EnUs" : "PtBr",
        }),
      });
      const res = await signIn("credentials", {
        email: form.get("email"),
        password: form.get("password"),
        redirect: false,
      });
      if (res?.error) {
        setError(t("signInAfterRegisterFailed"));
        return;
      }
      router.push("/account");
    } catch {
      setError(t("registrationFailed"));
    }
  }

  return (
    <div className="mx-auto max-w-md space-y-6 rounded-xl bg-white p-8 shadow-sm">
      <h1 className="font-serif text-3xl text-primary">{t("signUpTitle")}</h1>
      <form onSubmit={onSubmit} className="space-y-4">
        <label className="block text-sm">
          {t("name")}
          <input name="name" required className="mt-1 w-full rounded border px-3 py-2" />
        </label>
        <label className="block text-sm">
          {t("email")}
          <input name="email" type="email" required className="mt-1 w-full rounded border px-3 py-2" />
        </label>
        <label className="block text-sm">
          {t("password")}
          <input name="password" type="password" required minLength={8} className="mt-1 w-full rounded border px-3 py-2" />
        </label>
        <button type="submit" className="w-full rounded bg-accent px-4 py-2 font-semibold text-accent-foreground">
          {t("submitSignUp")}
        </button>
      </form>
      {error && <p className="text-sm text-red-600">{error}</p>}
    </div>
  );
}
