import { auth } from "@/auth";
import { FinancialAssistantPanel } from "@/components/assistant/FinancialAssistantPanel";
import { getTranslations, setRequestLocale } from "next-intl/server";
import { redirect } from "@/i18n/navigation";

export default async function AssistantPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const session = await auth();
  if (!session?.user) redirect({ href: "/sign-in", locale });
  const t = await getTranslations("assistant");

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
      <p className="mt-2 text-sm text-foreground/70">{t("subtitle")}</p>
      <div className="mt-8">
        <FinancialAssistantPanel />
      </div>
    </div>
  );
}
