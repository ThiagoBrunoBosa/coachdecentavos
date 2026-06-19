import { setRequestLocale } from "next-intl/server";
import { getTranslations } from "next-intl/server";
import { Link } from "@/i18n/navigation";
import { listProducts } from "@/lib/services/catalog";
import { AdSlot } from "@/components/AdSlot";

export default async function ProductsPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("products");
  const th = await getTranslations("home");

  let products: Awaited<ReturnType<typeof listProducts>> = [];
  try {
    products = await listProducts();
  } catch {
    products = [];
  }

  return (
    <div>
      <section className="bg-primary px-4 py-14 text-primary-foreground">
        <div className="mx-auto max-w-6xl">
          <h1 className="font-serif text-4xl md:text-5xl">{t("title")}</h1>
          <p className="mt-4 max-w-2xl text-lg opacity-90">{t("body")}</p>
        </div>
      </section>

      <div className="mx-auto max-w-6xl px-4 py-12">
        <div className="grid gap-8 lg:grid-cols-[1fr_240px]">
          <div>
            <ul className="grid gap-4 sm:grid-cols-2">
              {products.map((p) => (
                <li key={p.id}>
                  <Link
                    href={`/products/${p.slug}`}
                    className="flex h-full flex-col rounded-xl border border-primary/15 bg-white p-6 transition hover:border-accent"
                  >
                    <h2 className="font-semibold text-primary">{p.name}</h2>
                    <p className="mt-2 text-sm text-foreground/70">
                      {p.currency} {p.price.toFixed(2)}
                    </p>
                    <span className="mt-auto pt-4 text-sm font-medium text-accent">
                      {t("buyHotmart")} →
                    </span>
                  </Link>
                </li>
              ))}
            </ul>
            {products.length === 0 && (
              <p className="text-sm text-foreground/60">{t("empty")}</p>
            )}
          </div>
          <AdSlot slot="sidebar" />
        </div>

        <div className="mt-12 rounded-xl border border-primary/10 bg-white p-8 text-center">
          <p className="font-serif text-xl text-primary">{th("finalCta.title")}</p>
          <Link
            href="/consulting"
            className="mt-4 inline-block rounded bg-primary px-5 py-2.5 text-sm text-primary-foreground"
          >
            {th("ctaSecondary")}
          </Link>
        </div>
      </div>
    </div>
  );
}
