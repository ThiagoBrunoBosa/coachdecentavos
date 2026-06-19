"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import type { AdminBooking } from "@/lib/types/api";

export function AdminBookingsPanel() {
  const { data: session } = useSession();
  const [bookings, setBookings] = useState<AdminBooking[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [busyId, setBusyId] = useState<string | null>(null);
  const [confirmingId, setConfirmingId] = useState<string | null>(null);
  const [meetingUrl, setMeetingUrl] = useState("");

  async function loadBookings() {
    const token = session?.accessToken;
    if (!token) return;
    setLoading(true);
    try {
      const items = await apiFetchBrowser<AdminBooking[]>("/admin/bookings", {
        accessToken: token,
      });
      setBookings(items);
      setError(null);
    } catch {
      setError("Could not load bookings.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadBookings();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [session?.accessToken]);

  async function confirmBooking(id: string) {
    const token = session?.accessToken;
    if (!token) return;
    setBusyId(id);
    try {
      await apiFetchBrowser<void>(`/admin/bookings/${id}/confirm`, {
        method: "POST",
        accessToken: token,
        body: JSON.stringify({ meetingUrl: meetingUrl.trim() || null }),
      });
      setConfirmingId(null);
      setMeetingUrl("");
      await loadBookings();
    } catch {
      setError("Could not confirm booking.");
    } finally {
      setBusyId(null);
    }
  }

  async function cancelBooking(id: string) {
    const token = session?.accessToken;
    if (!token) return;
    setBusyId(id);
    try {
      await apiFetchBrowser<void>(`/admin/bookings/${id}/cancel`, {
        method: "POST",
        accessToken: token,
      });
      await loadBookings();
    } catch {
      setError("Could not cancel booking.");
    } finally {
      setBusyId(null);
    }
  }

  async function completeBooking(id: string) {
    const token = session?.accessToken;
    if (!token) return;
    setBusyId(id);
    try {
      await apiFetchBrowser<void>(`/admin/bookings/${id}/complete`, {
        method: "POST",
        accessToken: token,
      });
      await loadBookings();
    } catch {
      setError("Could not complete booking.");
    } finally {
      setBusyId(null);
    }
  }

  if (loading) return <p className="text-sm text-foreground/60">Loading bookings…</p>;
  if (error) return <p className="text-sm text-red-600">{error}</p>;
  if (bookings.length === 0) return <p className="text-sm text-foreground/60">No bookings yet.</p>;

  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left text-sm">
        <thead>
          <tr className="border-b text-foreground/60">
            <th className="py-2 pr-4">Client</th>
            <th className="py-2 pr-4">Package</th>
            <th className="py-2 pr-4">When</th>
            <th className="py-2 pr-4">Status</th>
            <th className="py-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {bookings.map((booking) => (
            <tr key={booking.id} className="border-b border-primary/10 align-top">
              <td className="py-2 pr-4">
                <div>{booking.userName}</div>
                <div className="text-xs text-foreground/60">{booking.userEmail}</div>
              </td>
              <td className="py-2 pr-4">{booking.packageName}</td>
              <td className="py-2 pr-4">{new Date(booking.startsAtUtc).toLocaleString()}</td>
              <td className="py-2 pr-4">{booking.status}</td>
              <td className="py-2">
                <div className="flex flex-col gap-2">
                  {booking.status === "Pending" && confirmingId !== booking.id && (
                    <button
                      type="button"
                      disabled={busyId === booking.id}
                      onClick={() => setConfirmingId(booking.id)}
                      className="rounded bg-primary px-2 py-1 text-xs text-primary-foreground disabled:opacity-50"
                    >
                      Confirm
                    </button>
                  )}
                  {confirmingId === booking.id && (
                    <div className="space-y-1">
                      <input
                        type="url"
                        value={meetingUrl}
                        onChange={(e) => setMeetingUrl(e.target.value)}
                        placeholder="Meeting URL (optional)"
                        className="w-full min-w-[180px] rounded border px-2 py-1 text-xs"
                      />
                      <div className="flex gap-1">
                        <button
                          type="button"
                          disabled={busyId === booking.id}
                          onClick={() => confirmBooking(booking.id)}
                          className="rounded bg-primary px-2 py-1 text-xs text-primary-foreground"
                        >
                          Save
                        </button>
                        <button
                          type="button"
                          onClick={() => {
                            setConfirmingId(null);
                            setMeetingUrl("");
                          }}
                          className="rounded border px-2 py-1 text-xs"
                        >
                          Cancel
                        </button>
                      </div>
                    </div>
                  )}
                  {booking.status === "Confirmed" && (
                    <button
                      type="button"
                      disabled={busyId === booking.id}
                      onClick={() => completeBooking(booking.id)}
                      className="rounded border border-primary/30 px-2 py-1 text-xs text-primary"
                    >
                      Complete
                    </button>
                  )}
                  {(booking.status === "Pending" || booking.status === "Confirmed") && (
                    <button
                      type="button"
                      disabled={busyId === booking.id}
                      onClick={() => cancelBooking(booking.id)}
                      className="rounded border border-red-200 px-2 py-1 text-xs text-red-700"
                    >
                      Cancel booking
                    </button>
                  )}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
