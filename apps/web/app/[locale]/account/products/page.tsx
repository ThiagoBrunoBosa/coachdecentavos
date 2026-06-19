import { auth } from "@/auth";
import { listEntitlements } from "@/lib/services/entitlements";
import { getTranslations, setRequestLocale } from "next-intl/server";
import { redirect } from "@/i18n/navigation";

export default async function AccountProductsPage({
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

  const t = await getTranslations("productsAccount");
  const entitlements = await listEntitlements(accessToken);

  return (
    <div className="mx-auto max-w-3xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
      <p className="mt-2 text-sm text-foreground/70">{t("subtitle")}</p>

      {entitlements.length === 0 ? (
        <p className="mt-8 text-foreground/70">{t("empty")}</p>
      ) : (
        <ul className="mt-8 space-y-4">
          {entitlements.map((item) => (
            <li key={item.id} className="rounded-lg border bg-white p-4">
              <div className="font-medium">{item.productName}</div>
              <div className="mt-1 text-sm text-foreground/70">
                {t("status")}: {item.status}
              </div>
              {item.activatedAtUtc && (
                <div className="mt-1 text-xs text-foreground/60">
                  {t("activated")}: {new Date(item.activatedAtUtc).toLocaleDateString()}
                </div>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
