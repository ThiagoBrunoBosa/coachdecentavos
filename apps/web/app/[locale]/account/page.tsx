import { auth } from "@/auth";
import { Link } from "@/i18n/navigation";
import { getTranslations, setRequestLocale } from "next-intl/server";
import { redirect } from "@/i18n/navigation";

export default async function AccountPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const session = await auth();
  if (!session?.user) {
    redirect({ href: "/sign-in", locale });
  }

  const t = await getTranslations("account");

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
      <p className="mt-4">{t("welcome")}, {session?.user?.email}</p>
      <nav className="mt-8 grid gap-3 sm:grid-cols-2">
        <Link
          href="/account/assistant"
          className="rounded-lg border border-primary/15 bg-white p-4 text-sm font-medium hover:border-accent"
        >
          {t("assistantLink")}
        </Link>
        <Link
          href="/account/link-purchase"
          className="rounded-lg border border-primary/15 bg-white p-4 text-sm font-medium hover:border-accent"
        >
          {t("linkPurchase")}
        </Link>
        <Link
          href="/account/consultations"
          className="rounded-lg border border-primary/15 bg-white p-4 text-sm font-medium hover:border-accent"
        >
          {t("consultationsLink")}
        </Link>
        <Link
          href="/account/products"
          className="rounded-lg border border-primary/15 bg-white p-4 text-sm font-medium hover:border-accent"
        >
          {t("productsLink")}
        </Link>
      </nav>
      <dl className="mt-8 space-y-2 rounded-lg bg-white p-6 text-sm">
        <div className="flex justify-between gap-4">
          <dt className="text-foreground/60">Role</dt>
          <dd>{session?.role ?? "—"}</dd>
        </div>
      </dl>
    </div>
  );
}
