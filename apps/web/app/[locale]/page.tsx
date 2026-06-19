import { getTranslations, setRequestLocale } from "next-intl/server";
import { Link } from "@/i18n/navigation";
import { AdSlot } from "@/components/AdSlot";
import { FaqAccordion } from "@/components/home/FaqAccordion";
import { HomeLatestShorts } from "@/components/home/HomeLatestShorts";
import { HomeProductsTeaser } from "@/components/home/HomeProductsTeaser";

type BenefitItem = { title: string; body: string };
type StepItem = { title: string; body: string };
type TestimonialItem = { quote: string; author: string };
type FaqItem = { question: string; answer: string };

export default async function HomePage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);
  const t = await getTranslations("home");
  const tc = await getTranslations("common");

  const heroBullets = t.raw("heroBullets") as string[];
  const trustItems = t.raw("trustItems") as string[];
  const benefits = t.raw("benefits.items") as BenefitItem[];
  const offerings = t.raw("offerings.items") as string[];
  const steps = t.raw("process.steps") as StepItem[];
  const audience = t.raw("audience.items") as string[];
  const testimonials = t.raw("testimonials.items") as TestimonialItem[];
  const faqItems = t.raw("faq.items") as FaqItem[];

  return (
    <div>
      {/* Hero */}
      <section className="bg-primary px-4 py-16 text-primary-foreground md:py-24">
        <div className="mx-auto grid max-w-6xl gap-10 md:grid-cols-2 md:items-center">
          <div>
            <p className="text-sm uppercase tracking-widest text-accent">{t("heroEyebrow")}</p>
            <h1 className="mt-2 font-serif text-4xl leading-tight md:text-5xl">{t("heroTitle")}</h1>
            <p className="mt-4 text-lg opacity-90">{t("heroSubtitle")}</p>
            <ul className="mt-6 space-y-2 text-sm opacity-90">
              {heroBullets.map((item) => (
                <li key={item} className="flex gap-2">
                  <span className="text-accent" aria-hidden>
                    ✓
                  </span>
                  {item}
                </li>
              ))}
            </ul>
            <div className="mt-8 flex flex-wrap gap-3">
              <Link
                href="/consulting/interest"
                className="rounded bg-accent px-6 py-3 font-semibold text-accent-foreground"
              >
                {t("ctaPrimary")}
              </Link>
              <Link
                href="/consulting"
                className="rounded border border-primary-foreground/30 px-6 py-3 font-semibold hover:bg-primary-foreground/10"
              >
                {t("ctaSecondary")}
              </Link>
            </div>
          </div>
          <div className="relative aspect-[4/5] max-h-[420px] overflow-hidden rounded-2xl border border-primary-foreground/15 bg-primary-foreground/5 p-8 md:max-h-none">
            <div className="absolute inset-0 bg-gradient-to-br from-accent/20 via-transparent to-primary-foreground/10" />
            <div className="relative flex h-full flex-col justify-end">
              <div className="mb-auto flex h-20 w-20 items-center justify-center rounded-full bg-accent font-serif text-3xl text-accent-foreground">
                CM
              </div>
              <p className="font-serif text-2xl leading-snug">{t("heroCardQuote")}</p>
              <p className="mt-3 text-sm opacity-80">{t("heroCardAuthor")}</p>
            </div>
          </div>
        </div>
      </section>

      {/* Trust bar */}
      <section className="border-b border-primary/10 bg-white">
        <div className="mx-auto max-w-6xl px-4 py-8">
          <p className="text-center text-xs font-medium uppercase tracking-widest text-primary/70">
            {t("credentials")}
          </p>
          <div className="mt-4 flex flex-wrap justify-center gap-x-8 gap-y-2 text-sm font-medium text-foreground/80">
            {trustItems.map((item) => (
              <span key={item}>{item}</span>
            ))}
          </div>
        </div>
      </section>

      {/* Benefits */}
      <section className="mx-auto max-w-6xl px-4 py-16">
        <div className="max-w-2xl">
          <h2 className="font-serif text-3xl text-primary">{t("benefits.title")}</h2>
          <p className="mt-3 text-foreground/70">{t("benefits.subtitle")}</p>
        </div>
        <ul className="mt-10 grid gap-6 md:grid-cols-3">
          {benefits.map((item) => (
            <li
              key={item.title}
              className="rounded-xl border border-primary/10 bg-white p-6 shadow-sm"
            >
              <div className="mb-4 h-1 w-10 rounded bg-accent" />
              <h3 className="font-semibold text-primary">{item.title}</h3>
              <p className="mt-2 text-sm leading-relaxed text-foreground/70">{item.body}</p>
            </li>
          ))}
        </ul>
      </section>

      {/* Offerings */}
      <section className="bg-white px-4 py-16">
        <div className="mx-auto grid max-w-6xl gap-10 lg:grid-cols-2 lg:items-center">
          <div>
            <h2 className="font-serif text-3xl text-primary">{t("offerings.title")}</h2>
            <p className="mt-3 text-foreground/70">{t("offerings.subtitle")}</p>
            <Link
              href="/about"
              className="mt-6 inline-block text-sm font-medium text-primary underline"
            >
              {tc("learnMore")}
            </Link>
          </div>
          <ul className="space-y-3">
            {offerings.map((item) => (
              <li
                key={item}
                className="flex gap-3 rounded-lg border border-primary/10 bg-background px-4 py-3 text-sm"
              >
                <span className="mt-0.5 shrink-0 font-bold text-accent" aria-hidden>
                  ✓
                </span>
                {item}
              </li>
            ))}
          </ul>
        </div>
      </section>

      {/* Process */}
      <section className="mx-auto max-w-6xl px-4 py-16">
        <h2 className="text-center font-serif text-3xl text-primary">{t("process.title")}</h2>
        <p className="mx-auto mt-3 max-w-2xl text-center text-foreground/70">{t("process.subtitle")}</p>
        <ol className="mt-10 grid gap-6 md:grid-cols-3">
          {steps.map((step, index) => (
            <li
              key={step.title}
              className="relative rounded-xl border border-primary/10 bg-white p-6 pt-10"
            >
              <span className="absolute left-6 top-0 flex h-8 w-8 -translate-y-1/2 items-center justify-center rounded-full bg-primary font-serif text-sm text-primary-foreground">
                {index + 1}
              </span>
              <h3 className="font-semibold text-primary">{step.title}</h3>
              <p className="mt-2 text-sm leading-relaxed text-foreground/70">{step.body}</p>
            </li>
          ))}
        </ol>
      </section>

      <HomeLatestShorts />

      {/* Audience */}
      <section className="bg-primary px-4 py-16 text-primary-foreground">
        <div className="mx-auto max-w-6xl">
          <h2 className="font-serif text-3xl">{t("audience.title")}</h2>
          <p className="mt-3 max-w-2xl opacity-90">{t("audience.subtitle")}</p>
          <ul className="mt-8 grid gap-3 sm:grid-cols-2">
            {audience.map((item) => (
              <li key={item} className="flex gap-2 rounded-lg bg-primary-foreground/10 px-4 py-3 text-sm">
                <span className="text-accent" aria-hidden>
                  •
                </span>
                {item}
              </li>
            ))}
          </ul>
        </div>
      </section>

      <AdSlot slot="inline" className="mx-auto max-w-6xl px-4" />

      {/* Testimonials */}
      <section className="mx-auto max-w-6xl px-4 py-16">
        <h2 className="text-center font-serif text-3xl text-primary">{t("testimonials.title")}</h2>
        <p className="mx-auto mt-3 max-w-2xl text-center text-sm text-foreground/60">
          {t("testimonials.disclaimer")}
        </p>
        <ul className="mt-10 grid gap-6 md:grid-cols-3">
          {testimonials.map((item) => (
            <li
              key={item.author}
              className="flex flex-col rounded-xl border border-primary/10 bg-white p-6"
            >
              <p className="flex-1 text-sm leading-relaxed text-foreground/80">&ldquo;{item.quote}&rdquo;</p>
              <p className="mt-4 text-sm font-medium text-primary">— {item.author}</p>
            </li>
          ))}
        </ul>
      </section>

      <HomeProductsTeaser />

      {/* FAQ */}
      <section className="mx-auto max-w-3xl px-4 py-16">
        <h2 className="text-center font-serif text-3xl text-primary">{t("faq.title")}</h2>
        <p className="mx-auto mt-3 max-w-xl text-center text-foreground/70">{t("faq.subtitle")}</p>
        <div className="mt-8">
          <FaqAccordion items={faqItems} />
        </div>
      </section>

      {/* Final CTA */}
      <section className="mx-auto max-w-6xl px-4 pb-20">
        <div className="rounded-2xl bg-gradient-to-br from-primary to-primary/90 px-6 py-12 text-center text-primary-foreground md:px-12">
          <h2 className="font-serif text-3xl">{t("finalCta.title")}</h2>
          <p className="mx-auto mt-3 max-w-xl opacity-90">{t("finalCta.subtitle")}</p>
          <div className="mt-8 flex flex-wrap justify-center gap-3">
            <Link
              href="/consulting/interest"
              className="rounded bg-accent px-6 py-3 font-semibold text-accent-foreground"
            >
              {t("finalCta.primary")}
            </Link>
            <Link
              href="/sign-up"
              className="rounded border border-primary-foreground/30 px-6 py-3 font-semibold hover:bg-primary-foreground/10"
            >
              {tc("signUp")}
            </Link>
          </div>
        </div>
      </section>
    </div>
  );
}
