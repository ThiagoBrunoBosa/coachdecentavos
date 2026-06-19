import { auth } from "@/auth";
import { Link } from "@/i18n/navigation";
import { FaqAccordion } from "@/components/home/FaqAccordion";
import { listConsultingPackages } from "@/lib/services/consulting";
import { getTranslations, setRequestLocale } from "next-intl/server";

export default async function ConsultingPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const session = await auth();
  const t = await getTranslations("consulting");
  const th = await getTranslations("home");
  const packages = await listConsultingPackages();

  const benefits = t.raw("benefits") as string[];
  const howSteps = t.raw("howSteps") as string[];
  const faqItems = th.raw("faq.items") as { question: string; answer: string }[];

  return (
    <div>
      <section className="bg-primary px-4 py-14 text-primary-foreground">
        <div className="mx-auto max-w-6xl">
          <h1 className="font-serif text-4xl md:text-5xl">{t("title")}</h1>
          <p className="mt-4 max-w-3xl text-lg opacity-90">{t("body")}</p>
          <Link
            href="/consulting/interest"
            className="mt-8 inline-block rounded bg-accent px-6 py-3 font-semibold text-accent-foreground"
          >
            {t("interestCta")}
          </Link>
        </div>
      </section>

      <div className="mx-auto max-w-6xl px-4 py-12">
        <section className="max-w-3xl">
          <h2 className="font-serif text-2xl text-primary">{t("introTitle")}</h2>
          <p className="mt-3 text-foreground/75">{t("introBody")}</p>
        </section>

        <div className="mt-12 grid gap-10 lg:grid-cols-2">
          <section>
            <h2 className="font-serif text-2xl text-primary">{t("benefitsTitle")}</h2>
            <ul className="mt-4 space-y-3">
              {benefits.map((item) => (
                <li key={item} className="flex gap-3 text-sm text-foreground/80">
                  <span className="font-bold text-accent" aria-hidden>
                    ✓
                  </span>
                  {item}
                </li>
              ))}
            </ul>
          </section>
          <section>
            <h2 className="font-serif text-2xl text-primary">{t("howTitle")}</h2>
            <ol className="mt-4 space-y-4">
              {howSteps.map((step, index) => (
                <li key={step} className="flex gap-4 text-sm">
                  <span className="flex h-7 w-7 shrink-0 items-center justify-center rounded-full bg-primary font-serif text-xs text-primary-foreground">
                    {index + 1}
                  </span>
                  <span className="pt-1 text-foreground/80">{step}</span>
                </li>
              ))}
            </ol>
          </section>
        </div>

        {packages.length > 0 && (
          <section className="mt-14">
            <h2 className="font-serif text-2xl text-primary">{t("packagesTitle")}</h2>
            <ul className="mt-6 grid gap-4 sm:grid-cols-2">
              {packages.map((pkg) => (
                <li
                  key={pkg.id}
                  className="rounded-xl border border-primary/10 bg-white p-6 shadow-sm"
                >
                  <h3 className="text-lg font-semibold text-primary">{pkg.name}</h3>
                  {pkg.description && (
                    <p className="mt-2 text-sm text-foreground/70">{pkg.description}</p>
                  )}
                  <p className="mt-4 text-sm font-medium">
                    {pkg.durationMinutes} min · {pkg.currency} {pkg.price}
                  </p>
                </li>
              ))}
            </ul>
            <div className="mt-8">
              {session?.user ? (
                <Link
                  href="/account/consultations/book"
                  className="inline-block rounded bg-primary px-6 py-3 text-primary-foreground"
                >
                  {t("bookCta")}
                </Link>
              ) : (
                <Link
                  href="/sign-in"
                  className="inline-block rounded bg-primary px-6 py-3 text-primary-foreground"
                >
                  {t("signInToBook")}
                </Link>
              )}
            </div>
          </section>
        )}

        <section className="mt-16 max-w-3xl">
          <h2 className="font-serif text-2xl text-primary">{th("faq.title")}</h2>
          <div className="mt-6">
            <FaqAccordion items={faqItems.slice(0, 4)} />
          </div>
        </section>

        <p className="mt-8 text-sm text-foreground/60">
          <Link href="/consulting/interest" className="text-primary underline">
            {t("interestLink")}
          </Link>
        </p>
      </div>
    </div>
  );
}
