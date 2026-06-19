import { setRequestLocale } from "next-intl/server";
import { getTranslations } from "next-intl/server";
import { listShorts } from "@/lib/services/catalog";

export default async function ShortsPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("shorts");

  let shorts: Awaited<ReturnType<typeof listShorts>> = [];
  try {
    shorts = await listShorts();
  } catch {
    shorts = [];
  }

  return (
    <div className="mx-auto max-w-6xl px-4 py-12">
      <h1 className="font-serif text-4xl text-primary">{t("title")}</h1>
      <p className="mt-4 text-foreground/80">{t("body")}</p>
      <div className="mt-8 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {shorts.map((s) => (
          <article key={s.videoId} className="overflow-hidden rounded-lg border border-primary/10 bg-white">
            <div className="aspect-[9/16] bg-black/5">
              <iframe
                title={s.title}
                src={`https://www.youtube.com/embed/${s.videoId}`}
                className="h-full w-full"
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowFullScreen
              />
            </div>
            <p className="p-3 text-sm font-medium text-primary">{s.title}</p>
          </article>
        ))}
      </div>
      {shorts.length === 0 && (
        <p className="mt-8 text-sm text-foreground/60">{t("empty")}</p>
      )}
    </div>
  );
}
