import { Link } from "@/i18n/navigation";
import { listProducts } from "@/lib/services/catalog";
import { getTranslations } from "next-intl/server";

export async function HomeProductsTeaser() {
  const t = await getTranslations("home");
  const tp = await getTranslations("products");

  let products: Awaited<ReturnType<typeof listProducts>> = [];
  try {
    products = await listProducts();
  } catch {
    products = [];
  }

  if (products.length === 0) return null;

  const preview = products.slice(0, 3);

  return (
    <section className="mx-auto max-w-6xl px-4 py-16">
      <div className="flex flex-wrap items-end justify-between gap-4">
        <div>
          <h2 className="font-serif text-3xl text-primary">{t("productsTitle")}</h2>
          <p className="mt-2 max-w-xl text-foreground/70">{t("productsSubtitle")}</p>
        </div>
        <Link href="/products" className="text-sm font-medium text-primary underline">
          {t("viewAll")}
        </Link>
      </div>
      <ul className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {preview.map((p) => (
          <li key={p.id}>
            <Link
              href={`/products/${p.slug}`}
              className="block h-full rounded-xl border border-primary/10 bg-white p-6 transition hover:border-accent"
            >
              <h3 className="font-semibold text-primary">{p.name}</h3>
              <p className="mt-2 text-sm text-foreground/70">
                {p.currency} {p.price.toFixed(2)}
              </p>
              <span className="mt-4 inline-block text-sm font-medium text-accent">{tp("buyHotmart")} →</span>
            </Link>
          </li>
        ))}
      </ul>
    </section>
  );
}
