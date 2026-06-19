"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useTranslations } from "next-intl";
import { API_PUBLIC_URL } from "@/lib/api-config";

export function FinancialAssistantPanel() {
  const { data: session } = useSession();
  const t = useTranslations("assistant");
  const [question, setQuestion] = useState("");
  const [answer, setAnswer] = useState<string | null>(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [consent, setConsent] = useState<boolean | null>(null);

  const token = session?.accessToken;

  useEffect(() => {
    if (!token) return;
    void (async () => {
      const res = await fetch(`${API_PUBLIC_URL}/me/ai/consent`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) {
        const data = (await res.json()) as { hasConsent: boolean };
        setConsent(data.hasConsent);
      }
    })();
  }, [token]);

  const acceptConsent = async () => {
    if (!token) return;
    await fetch(`${API_PUBLIC_URL}/me/ai/consent`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ disclaimerVersion: "2026-06-1" }),
    });
    setConsent(true);
  };

  const ask = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!token || !question.trim()) return;
    setLoading(true);
    setError("");
    setAnswer(null);
    try {
      const res = await fetch(`${API_PUBLIC_URL}/me/ai/ask`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ question: question.trim() }),
      });
      const data = await res.json();
      if (!res.ok) {
        setError(data.error ?? t("error"));
        return;
      }
      setAnswer(data.answer);
    } catch {
      setError(t("error"));
    } finally {
      setLoading(false);
    }
  };

  if (consent === null) {
    return <p className="text-sm text-foreground/60">{t("loading")}</p>;
  }

  if (!consent) {
    return (
      <div className="rounded-lg border border-accent/30 bg-accent/5 p-6">
        <p className="text-sm text-foreground/80">{t("disclaimer")}</p>
        <button
          type="button"
          onClick={() => void acceptConsent()}
          className="mt-4 rounded-md bg-primary px-4 py-2 text-sm font-semibold text-white"
        >
          {t("accept")}
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <form onSubmit={ask} className="space-y-3">
        <label htmlFor="ai-question" className="block text-sm font-medium">
          {t("questionLabel")}
        </label>
        <textarea
          id="ai-question"
          value={question}
          onChange={(e) => setQuestion(e.target.value)}
          rows={3}
          className="w-full rounded-md border border-primary/20 px-3 py-2 text-sm"
          placeholder={t("placeholder")}
        />
        <button
          type="submit"
          disabled={loading}
          className="rounded-md bg-primary px-4 py-2 text-sm font-semibold text-white disabled:opacity-50"
        >
          {loading ? t("sending") : t("submit")}
        </button>
      </form>
      {error && <p className="text-sm text-red-600">{error}</p>}
      {answer && (
        <div className="rounded-lg bg-white p-4 text-sm text-foreground/90">{answer}</div>
      )}
    </div>
  );
}
