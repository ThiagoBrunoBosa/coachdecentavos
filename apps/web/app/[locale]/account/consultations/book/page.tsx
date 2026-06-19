import { BookingForm } from "@/components/consulting/BookingForm";
import { getTranslations, setRequestLocale } from "next-intl/server";

export default async function BookConsultationPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("consultations");

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("bookTitle")}</h1>
      <p className="mt-2 text-sm text-foreground/70">{t("bookSubtitle")}</p>
      <BookingForm />
    </div>
  );
}
