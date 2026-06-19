import { setRequestLocale } from "next-intl/server";
import { LeadInterestForm } from "@/components/LeadInterestForm";

export default async function ConsultingInterestPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);

  return (
    <div className="px-4 py-12">
      <LeadInterestForm />
    </div>
  );
}
