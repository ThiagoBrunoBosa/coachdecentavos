import { Link } from "@/i18n/navigation";
import { listLatestShorts } from "@/lib/services/catalog";
import { getTranslations } from "next-intl/server";

function shortThumbnailUrl(videoId: string, thumbnailUrl?: string | null) {
  return thumbnailUrl ?? `https://img.youtube.com/vi/${videoId}/mqdefault.jpg`;
}

export async function HomeLatestShorts() {
  const t = await getTranslations("home.shortsSection");

  let shorts: Awaited<ReturnType<typeof listLatestShorts>> = [];
  try {
    shorts = await listLatestShorts(3);
  } catch {
    shorts = [];
  }

  if (shorts.length === 0) return null;

  const [featured, ...more] = shorts;

  return (
    <section className="border-y border-primary/10 bg-gradient-to-b from-background to-white px-4 py-16">
      <div className="mx-auto max-w-6xl">
        <div className="grid gap-10 lg:grid-cols-12 lg:items-center">
          <div className="lg:col-span-5">
            <p className="text-sm uppercase tracking-widest text-accent">{t("eyebrow")}</p>
            <h2 className="mt-2 font-serif text-3xl text-primary md:text-4xl">{t("title")}</h2>
            <p className="mt-4 leading-relaxed text-foreground/70">{t("subtitle")}</p>
            <Link
              href="/shorts"
              className="mt-6 inline-flex items-center gap-2 rounded bg-primary px-5 py-2.5 text-sm font-semibold text-primary-foreground transition hover:bg-primary/90"
            >
              {t("viewAll")}
              <span aria-hidden>→</span>
            </Link>
          </div>

          <div className="lg:col-span-7">
            <div className="flex flex-col items-center gap-8 sm:flex-row sm:items-start sm:justify-end">
              <article className="w-full max-w-[280px] shrink-0 overflow-hidden rounded-2xl bg-white shadow-lg ring-1 ring-primary/10">
                <div className="relative aspect-[9/16] bg-black/5">
                  <span className="absolute left-3 top-3 z-10 rounded-full bg-accent px-2.5 py-1 text-xs font-semibold text-accent-foreground">
                    {t("latestBadge")}
                  </span>
                  <iframe
                    title={featured.title}
                    src={`https://www.youtube.com/embed/${featured.videoId}`}
                    className="h-full w-full"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen
                  />
                </div>
                <p className="border-t border-primary/5 p-4 text-sm font-medium leading-snug text-primary">
                  {featured.title}
                </p>
              </article>

              {more.length > 0 && (
                <ul className="flex w-full flex-row gap-3 overflow-x-auto pb-1 sm:w-auto sm:flex-col sm:overflow-visible sm:pb-0">
                  {more.map((short) => (
                    <li key={short.videoId} className="min-w-[160px] shrink-0 sm:min-w-0">
                      <Link
                        href="/shorts"
                        className="group flex gap-3 rounded-xl border border-primary/10 bg-white p-2 transition hover:border-accent hover:shadow-md sm:max-w-[220px]"
                      >
                        <div className="relative aspect-[9/16] w-16 shrink-0 overflow-hidden rounded-lg bg-black/5">
                          {/* eslint-disable-next-line @next/next/no-img-element */}
                          <img
                            src={shortThumbnailUrl(short.videoId, short.thumbnailUrl)}
                            alt=""
                            className="h-full w-full object-cover transition group-hover:scale-105"
                          />
                        </div>
                        <div className="flex min-w-0 flex-1 flex-col justify-center py-1">
                          <p className="line-clamp-3 text-xs font-medium leading-snug text-primary">
                            {short.title}
                          </p>
                          <span className="mt-2 text-xs font-medium text-accent">{t("watchMore")} →</span>
                        </div>
                      </Link>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
