"use client";

import { useState } from "react";
import { useTranslations } from "next-intl";
import { apiFetchBrowser } from "@/lib/api-client-browser";

export function LeadInterestForm() {
  const t = useTranslations("lead");
  const [status, setStatus] = useState<"idle" | "success" | "error">("idle");
  const [loading, setLoading] = useState(false);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setLoading(true);
    setStatus("idle");
    const form = new FormData(e.currentTarget);
    try {
      await apiFetchBrowser("/leads", {
        method: "POST",
        body: JSON.stringify({
          name: form.get("name"),
          email: form.get("email"),
          phone: form.get("phone"),
          message: form.get("message"),
          source: "consulting-interest",
        }),
      });
      setStatus("success");
      e.currentTarget.reset();
    } catch {
      setStatus("error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <form onSubmit={onSubmit} className="mx-auto max-w-lg space-y-4 rounded-xl bg-white p-6 shadow-sm">
      <h2 className="font-serif text-2xl text-primary">{t("title")}</h2>
      <label className="block text-sm">
        {t("name")}
        <input name="name" required className="mt-1 w-full rounded border border-primary/20 px-3 py-2" />
      </label>
      <label className="block text-sm">
        {t("email")}
        <input name="email" type="email" required className="mt-1 w-full rounded border border-primary/20 px-3 py-2" />
      </label>
      <label className="block text-sm">
        {t("phone")}
        <input name="phone" className="mt-1 w-full rounded border border-primary/20 px-3 py-2" />
      </label>
      <label className="block text-sm">
        {t("message")}
        <textarea name="message" rows={4} className="mt-1 w-full rounded border border-primary/20 px-3 py-2" />
      </label>
      <button
        type="submit"
        disabled={loading}
        className="w-full rounded bg-accent px-4 py-2 font-semibold text-primary-foreground disabled:opacity-60"
      >
        {loading ? t("submitting") : t("submit")}
      </button>
      {status === "success" && <p className="text-sm text-primary">{t("success")}</p>}
      {status === "error" && <p className="text-sm text-red-600">{t("error")}</p>}
    </form>
  );
}
