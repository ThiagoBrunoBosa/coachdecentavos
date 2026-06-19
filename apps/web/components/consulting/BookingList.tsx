"use client";

import { useSession } from "next-auth/react";
import { useTranslations } from "next-intl";
import { useRouter } from "@/i18n/navigation";
import { useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import type { Booking } from "@/lib/types/api";

const STATUS_KEYS = {
  Pending: "statuses.Pending",
  Confirmed: "statuses.Confirmed",
  Cancelled: "statuses.Cancelled",
  Completed: "statuses.Completed",
} as const;

function statusLabel(status: string, t: (key: string) => string) {
  const key = STATUS_KEYS[status as keyof typeof STATUS_KEYS];
  return key ? t(key) : status;
}

export function BookingList({ bookings }: { bookings: Booking[] }) {
  const t = useTranslations("consultations");
  const { data: session } = useSession();
  const router = useRouter();
  const [items, setItems] = useState(bookings);
  const [cancellingId, setCancellingId] = useState<string | null>(null);

  async function cancelBooking(id: string) {
    const token = session?.accessToken;
    if (!token) return;
    setCancellingId(id);
    try {
      await apiFetchBrowser<void>(`/me/bookings/${id}/cancel`, {
        method: "POST",
        accessToken: token,
      });
      setItems((prev) =>
        prev.map((b) => (b.id === id ? { ...b, status: "Cancelled" } : b)),
      );
      router.refresh();
    } finally {
      setCancellingId(null);
    }
  }

  if (items.length === 0) {
    return <p className="mt-8 text-foreground/70">{t("empty")}</p>;
  }

  return (
    <ul className="mt-8 space-y-4">
      {items.map((booking) => (
        <li key={booking.id} className="rounded-lg border bg-white p-4">
          <div className="font-medium">{booking.packageName}</div>
          <div className="mt-1 text-sm text-foreground/70">
            {new Date(booking.startsAtUtc).toLocaleString()} —{" "}
            {new Date(booking.endsAtUtc).toLocaleTimeString()}
          </div>
          <div className="mt-2 text-sm">
            {t("status")}:{" "}
            <span className="font-medium">{statusLabel(booking.status, t)}</span>
          </div>
          {booking.meetingUrl && (
            <a
              href={booking.meetingUrl}
              target="_blank"
              rel="noreferrer"
              className="mt-2 inline-block text-sm text-primary underline"
            >
              {t("joinMeeting")}
            </a>
          )}
          {(booking.status === "Pending" || booking.status === "Confirmed") && (
            <button
              type="button"
              disabled={cancellingId === booking.id}
              onClick={() => cancelBooking(booking.id)}
              className="mt-3 rounded border border-red-200 px-3 py-1 text-xs text-red-700 disabled:opacity-50"
            >
              {cancellingId === booking.id ? t("cancelling") : t("cancel")}
            </button>
          )}
        </li>
      ))}
    </ul>
  );
}
