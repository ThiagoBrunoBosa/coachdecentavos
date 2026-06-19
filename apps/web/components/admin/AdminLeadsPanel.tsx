"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { apiFetchBrowser } from "@/lib/api-client-browser";
import type { AdminLead } from "@/lib/types/api";

export function AdminLeadsPanel() {
  const { data: session } = useSession();
  const [leads, setLeads] = useState<AdminLead[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = session?.accessToken;
    if (!token) return;

    apiFetchBrowser<AdminLead[]>("/admin/leads", { accessToken: token })
      .then(setLeads)
      .catch(() => setError("Could not load leads."))
      .finally(() => setLoading(false));
  }, [session?.accessToken]);

  if (loading) return <p className="text-sm text-foreground/60">Loading leads…</p>;
  if (error) return <p className="text-sm text-red-600">{error}</p>;
  if (leads.length === 0) return <p className="text-sm text-foreground/60">No leads yet.</p>;

  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left text-sm">
        <thead>
          <tr className="border-b text-foreground/60">
            <th className="py-2 pr-4">Name</th>
            <th className="py-2 pr-4">Email</th>
            <th className="py-2 pr-4">Phone</th>
            <th className="py-2 pr-4">Message</th>
            <th className="py-2">Created</th>
          </tr>
        </thead>
        <tbody>
          {leads.map((lead) => (
            <tr key={lead.id} className="border-b border-primary/10 align-top">
              <td className="py-2 pr-4">{lead.name ?? "—"}</td>
              <td className="py-2 pr-4">{lead.email}</td>
              <td className="py-2 pr-4">{lead.phone ?? "—"}</td>
              <td className="max-w-xs py-2 pr-4 text-xs text-foreground/70">{lead.message ?? "—"}</td>
              <td className="py-2">{new Date(lead.createdAtUtc).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
