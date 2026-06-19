"use client";

import { useSession } from "next-auth/react";
import { useRouter } from "@/i18n/navigation";
import { useTranslations } from "next-intl";
import { useEffect, useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import { API_PUBLIC_URL } from "@/lib/api-config";
import type { AvailabilitySlot, ConsultingPackage } from "@/lib/types/api";

export function BookingForm() {
  const t = useTranslations("consultations");
  const { data: session, status } = useSession();
  const router = useRouter();
  const [packages, setPackages] = useState<ConsultingPackage[]>([]);
  const [slots, setSlots] = useState<AvailabilitySlot[]>([]);
  const [packageId, setPackageId] = useState("");
  const [slotId, setSlotId] = useState("");
  const [notes, setNotes] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (status === "unauthenticated") {
      router.push("/sign-in");
    }
  }, [status, router]);

  useEffect(() => {
    Promise.all([
      fetch(`${API_PUBLIC_URL}/consulting/packages`).then((r) => r.json()),
      fetch(`${API_PUBLIC_URL}/consulting/slots`).then((r) => r.json()),
    ])
      .then(([pkgData, slotData]) => {
        setPackages(pkgData as ConsultingPackage[]);
        setSlots(slotData as AvailabilitySlot[]);
      })
      .catch(() => setError(t("loadError")));
  }, [t]);

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    const token = session?.accessToken;
    if (!token || !packageId || !slotId) return;

    setSubmitting(true);
    setError(null);
    try {
      await apiFetchBrowser("/me/bookings", {
        method: "POST",
        accessToken: token,
        body: JSON.stringify({
          packageId,
          slotId,
          notes: notes.trim() || null,
        }),
      });
      router.push("/account/consultations");
      router.refresh();
    } catch {
      setError(t("submitError"));
    } finally {
      setSubmitting(false);
    }
  }

  if (status === "loading") {
    return <p className="text-sm text-foreground/60">{t("loading")}</p>;
  }

  return (
    <form onSubmit={onSubmit} className="mt-6 space-y-4 rounded-lg border bg-white p-6">
      <div>
        <label htmlFor="package" className="block text-sm font-medium">
          {t("packageLabel")}
        </label>
        <select
          id="package"
          required
          value={packageId}
          onChange={(e) => setPackageId(e.target.value)}
          className="mt-1 w-full rounded border px-3 py-2"
        >
          <option value="">{t("selectPackage")}</option>
          {packages.map((pkg) => (
            <option key={pkg.id} value={pkg.id}>
              {pkg.name} — {pkg.durationMinutes} min — {pkg.currency} {pkg.price}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label htmlFor="slot" className="block text-sm font-medium">
          {t("slotLabel")}
        </label>
        <select
          id="slot"
          required
          value={slotId}
          onChange={(e) => setSlotId(e.target.value)}
          className="mt-1 w-full rounded border px-3 py-2"
        >
          <option value="">{t("selectSlot")}</option>
          {slots.map((slot) => (
            <option key={slot.id} value={slot.id}>
              {new Date(slot.startsAtUtc).toLocaleString()} —{" "}
              {new Date(slot.endsAtUtc).toLocaleTimeString()}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label htmlFor="notes" className="block text-sm font-medium">
          {t("notesLabel")}
        </label>
        <textarea
          id="notes"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          rows={3}
          className="mt-1 w-full rounded border px-3 py-2"
        />
      </div>

      {error && <p className="text-sm text-red-600">{error}</p>}

      <button
        type="submit"
        disabled={submitting || !packageId || !slotId}
        className="rounded bg-primary px-4 py-2 text-primary-foreground disabled:opacity-50"
      >
        {submitting ? t("submitting") : t("submit")}
      </button>
    </form>
  );
}
