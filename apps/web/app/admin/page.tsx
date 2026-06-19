import { auth } from "@/auth";
import { AdminBookingsPanel } from "@/components/admin/AdminBookingsPanel";
import { AdminLeadsPanel } from "@/components/admin/AdminLeadsPanel";
import { AdminSlotsPanel } from "@/components/admin/AdminSlotsPanel";
import { AuthSessionProvider } from "@/providers/session-provider";
import Link from "next/link";
import { redirect } from "next/navigation";

export default async function AdminDashboardPage() {
  const session = await auth();
  const role = session?.role?.toLowerCase();
  const isAdmin = role === "admin" || role === "administrator";
  if (!isAdmin) {
    redirect("/admin/login");
  }

  return (
    <AuthSessionProvider>
      <div className="mx-auto max-w-5xl px-4 py-12">
        <header className="mb-8 flex items-center justify-between">
          <h1 className="font-serif text-3xl text-primary">Admin dashboard</h1>
          <Link href="/pt" className="text-sm text-primary underline">
            View site
          </Link>
        </header>
        <p className="text-foreground/80">Signed in as {session?.user?.email}</p>
        <div className="mt-8 grid gap-6">
          <section className="rounded-lg border bg-white p-6">
            <h2 className="mb-4 font-serif text-xl text-primary">Leads</h2>
            <AdminLeadsPanel />
          </section>
          <section className="rounded-lg border bg-white p-6">
            <h2 className="mb-4 font-serif text-xl text-primary">Availability slots</h2>
            <AdminSlotsPanel />
          </section>
          <section className="rounded-lg border bg-white p-6">
            <h2 className="mb-4 font-serif text-xl text-primary">Bookings</h2>
            <AdminBookingsPanel />
          </section>
        </div>
      </div>
    </AuthSessionProvider>
  );
}
