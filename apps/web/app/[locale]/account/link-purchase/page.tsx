import { auth } from "@/auth";
import { LinkPurchaseForm } from "@/components/account/LinkPurchaseForm";
import { getTranslations, setRequestLocale } from "next-intl/server";
import { redirect } from "@/i18n/navigation";

export default async function LinkPurchasePage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const session = await auth();
  if (!session?.user) redirect({ href: "/sign-in", locale });
  const t = await getTranslations("linkPurchase");

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
      <div className="mt-8">
        <LinkPurchaseForm />
      </div>
    </div>
  );
}
