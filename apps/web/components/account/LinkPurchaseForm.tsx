"use client";

import { useState } from "react";
import { useSession } from "next-auth/react";
import { useTranslations } from "next-intl";
import { API_PUBLIC_URL } from "@/lib/api-config";

export function LinkPurchaseForm() {
  const { data: session } = useSession();
  const t = useTranslations("linkPurchase");
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    const token = session?.accessToken;
    if (!token) return;
    setLoading(true);
    setError("");
    setMessage("");
    try {
      const res = await fetch(`${API_PUBLIC_URL}/me/entitlements/link-purchase`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ buyerEmail: email.trim() }),
      });
      const data = await res.json();
      if (!res.ok) {
        setError(data.error ?? t("error"));
        return;
      }
      setMessage(t("success", { count: data.linkedCount ?? 0 }));
    } catch {
      setError(t("error"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={submit} className="max-w-md space-y-4">
      <p className="text-sm text-foreground/80">{t("body")}</p>
      <div>
        <label htmlFor="buyer-email" className="block text-sm font-medium">
          {t("emailLabel")}
        </label>
        <input
          id="buyer-email"
          type="email"
          required
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="mt-1 w-full rounded-md border border-primary/20 px-3 py-2 text-sm"
        />
      </div>
      <button
        type="submit"
        disabled={loading}
        className="rounded-md bg-primary px-4 py-2 text-sm font-semibold text-white disabled:opacity-50"
      >
        {loading ? t("sending") : t("submit")}
      </button>
      {error && <p className="text-sm text-red-600">{error}</p>}
      {message && <p className="text-sm text-green-700">{message}</p>}
    </form>
  );
}
