"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import type { AdminAvailabilitySlot } from "@/lib/types/api";

export function AdminSlotsPanel() {
  const { data: session } = useSession();
  const [slots, setSlots] = useState<AdminAvailabilitySlot[]>([]);
  const [startsAt, setStartsAt] = useState("");
  const [endsAt, setEndsAt] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  async function loadSlots() {
    const token = session?.accessToken;
    if (!token) return;
    setLoading(true);
    try {
      const items = await apiFetchBrowser<AdminAvailabilitySlot[]>("/admin/slots", {
        accessToken: token,
      });
      setSlots(items);
      setError(null);
    } catch {
      setError("Could not load slots.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    void loadSlots();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [session?.accessToken]);

  async function createSlot(e: React.FormEvent) {
    e.preventDefault();
    const token = session?.accessToken;
    if (!token || !startsAt || !endsAt) return;
    setSaving(true);
    setError(null);
    try {
      await apiFetchBrowser("/admin/slots", {
        method: "POST",
        accessToken: token,
        body: JSON.stringify({
          startsAtUtc: new Date(startsAt).toISOString(),
          endsAtUtc: new Date(endsAt).toISOString(),
        }),
      });
      setStartsAt("");
      setEndsAt("");
      await loadSlots();
    } catch {
      setError("Could not create slot.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="space-y-6">
      <form onSubmit={createSlot} className="grid gap-3 rounded border border-primary/10 p-4 sm:grid-cols-3">
        <label className="text-sm">
          Start (local)
          <input
            type="datetime-local"
            required
            value={startsAt}
            onChange={(e) => setStartsAt(e.target.value)}
            className="mt-1 w-full rounded border px-2 py-1"
          />
        </label>
        <label className="text-sm">
          End (local)
          <input
            type="datetime-local"
            required
            value={endsAt}
            onChange={(e) => setEndsAt(e.target.value)}
            className="mt-1 w-full rounded border px-2 py-1"
          />
        </label>
        <div className="flex items-end">
          <button
            type="submit"
            disabled={saving}
            className="w-full rounded bg-primary px-3 py-2 text-sm text-primary-foreground disabled:opacity-50"
          >
            {saving ? "Saving…" : "Add slot"}
          </button>
        </div>
      </form>

      {error && <p className="text-sm text-red-600">{error}</p>}
      {loading ? (
        <p className="text-sm text-foreground/60">Loading slots…</p>
      ) : slots.length === 0 ? (
        <p className="text-sm text-foreground/60">No upcoming slots.</p>
      ) : (
        <ul className="space-y-2 text-sm">
          {slots.map((slot) => (
            <li key={slot.id} className="flex flex-wrap items-center justify-between gap-2 rounded border px-3 py-2">
              <span>
                {new Date(slot.startsAtUtc).toLocaleString()} —{" "}
                {new Date(slot.endsAtUtc).toLocaleTimeString()}
              </span>
              <span className="text-xs text-foreground/60">
                {slot.isBlocked ? "Blocked" : slot.isBooked ? "Booked" : "Open"}
              </span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
