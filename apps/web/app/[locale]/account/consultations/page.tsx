import { auth } from "@/auth";
import { BookingList } from "@/components/consulting/BookingList";
import { Link } from "@/i18n/navigation";
import { listMyBookings } from "@/lib/services/consulting";
import { getTranslations, setRequestLocale } from "next-intl/server";
import { redirect } from "@/i18n/navigation";

export default async function ConsultationsPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const session = await auth();
  const sessionUser = session?.user;
  const accessToken = session?.accessToken;
  if (!sessionUser || !accessToken) {
    redirect({ href: "/sign-in", locale });
    throw new Error("Unauthorized");
  }

  const t = await getTranslations("consultations");
  const bookings = await listMyBookings(accessToken);

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
        <Link
          href="/account/consultations/book"
          className="rounded bg-primary px-4 py-2 text-sm text-primary-foreground"
        >
          {t("bookNew")}
        </Link>
      </div>
      <BookingList bookings={bookings} />
    </div>
  );
}
