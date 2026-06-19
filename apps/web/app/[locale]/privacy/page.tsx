import { getTranslations, setRequestLocale } from "next-intl/server";

export default async function PrivacyPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const tc = await getTranslations("common");
  const t = await getTranslations("legal");

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{tc("privacy")}</h1>
      <div className="mt-6 space-y-4 text-foreground/80">
        <p>{t("privacyIntro")}</p>
        <p>{t("privacyData")}</p>
        <p>{t("privacyUse")}</p>
        <p>{t("privacyContact")}</p>
      </div>
    </div>
  );
}
