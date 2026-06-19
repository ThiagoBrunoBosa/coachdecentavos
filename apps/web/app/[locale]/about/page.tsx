import { getTranslations, setRequestLocale } from "next-intl/server";
import { Link } from "@/i18n/navigation";
import { AdSlot } from "@/components/AdSlot";

export default async function AboutPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("about");
  const th = await getTranslations("home");

  const trustItems = th.raw("trustItems") as string[];

  return (
    <div>
      <section className="bg-primary px-4 py-14 text-primary-foreground">
        <div className="mx-auto max-w-6xl">
          <p className="text-sm uppercase tracking-widest text-accent">{th("heroEyebrow")}</p>
          <h1 className="mt-2 font-serif text-4xl md:text-5xl">{t("title")}</h1>
          <p className="mt-4 max-w-3xl text-lg opacity-90">{t("body")}</p>
        </div>
      </section>

      <div className="mx-auto max-w-6xl px-4 py-12">
        <div className="grid gap-8 lg:grid-cols-[1fr_240px]">
          <div className="space-y-10">
            <div className="flex flex-wrap gap-4 text-sm font-medium text-foreground/70">
              {trustItems.map((item) => (
                <span key={item} className="rounded-full border border-primary/15 px-3 py-1">
                  {item}
                </span>
              ))}
            </div>

            <div className="grid gap-6 md:grid-cols-3">
              <article className="rounded-xl border border-primary/10 bg-white p-6">
                <h2 className="font-serif text-xl text-primary">{t("missionTitle")}</h2>
                <p className="mt-3 text-sm leading-relaxed text-foreground/75">{t("missionBody")}</p>
              </article>
              <article className="rounded-xl border border-primary/10 bg-white p-6">
                <h2 className="font-serif text-xl text-primary">{t("experienceTitle")}</h2>
                <p className="mt-3 text-sm leading-relaxed text-foreground/75">{t("experienceBody")}</p>
              </article>
              <article className="rounded-xl border border-primary/10 bg-white p-6">
                <h2 className="font-serif text-xl text-primary">{t("approachTitle")}</h2>
                <p className="mt-3 text-sm leading-relaxed text-foreground/75">{t("approachBody")}</p>
              </article>
            </div>

            <div className="flex flex-wrap gap-3">
              <Link
                href="/consulting"
                className="rounded bg-primary px-5 py-2.5 text-sm font-medium text-primary-foreground"
              >
                {t("ctaConsulting")}
              </Link>
              <Link
                href="/products"
                className="rounded border border-primary/20 px-5 py-2.5 text-sm font-medium text-primary"
              >
                {t("ctaProducts")}
              </Link>
            </div>
          </div>
          <AdSlot slot="sidebar" />
        </div>
      </div>
    </div>
  );
}
