import { getTranslations, setRequestLocale } from "next-intl/server";
import { notFound } from "next/navigation";
import { AdSlot } from "@/components/AdSlot";
import { getProduct } from "@/lib/services/catalog";

export default async function ProductDetailPage({
  params,
}: {
  params: Promise<{ locale: string; slug: string }>;
}) {
  const { locale, slug } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("products");
  const product = await getProduct(slug);
  if (!product) notFound();

  return (
    <div className="mx-auto max-w-5xl px-4 py-12">
      <div className="grid gap-8 lg:grid-cols-[1fr_280px]">
        <div>
          <h1 className="font-serif text-4xl text-primary">{product.name}</h1>
          {product.description && (
            <p className="mt-4 text-foreground/80">{product.description}</p>
          )}
          <p className="mt-6 text-lg font-semibold text-primary">
            {product.currency} {product.price.toFixed(2)}
          </p>
          {product.hotmartCheckoutUrl && (
            <a
              href={product.hotmartCheckoutUrl}
              target="_blank"
              rel="noopener noreferrer"
              className="mt-6 inline-block rounded-md bg-primary px-6 py-3 text-sm font-semibold text-white hover:bg-primary/90"
            >
              {t("buyHotmart")}
            </a>
          )}
        </div>
        <AdSlot slot="sidebar" />
      </div>
    </div>
  );
}
