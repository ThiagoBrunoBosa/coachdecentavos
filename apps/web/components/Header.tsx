"use client";

import { useLocale, useTranslations } from "next-intl";
import { Link, usePathname, useRouter } from "@/i18n/navigation";
import { signOut, useSession } from "next-auth/react";

const locales = ["pt", "en"] as const;

const navLinks = [
  { href: "/" as const, labelKey: "home" as const },
  { href: "/consulting" as const, labelKey: "consulting" as const },
  { href: "/products" as const, labelKey: "products" as const },
  { href: "/shorts" as const, labelKey: "shorts" as const },
  { href: "/about" as const, labelKey: "about" as const },
];

export function Header() {
  const t = useTranslations("common");
  const locale = useLocale();
  const pathname = usePathname();
  const router = useRouter();
  const { data: session } = useSession();

  const switchLocale = (next: string) => {
    router.replace(pathname as "/", { locale: next });
  };

  return (
    <header className="border-b border-primary/10 bg-background/95 backdrop-blur">
      <div className="mx-auto flex max-w-6xl items-center gap-6 px-4 py-4">
        <Link href="/" className="shrink-0 font-serif text-xl font-semibold text-primary">
          {t("siteName")}
        </Link>

        <nav className="hidden flex-1 items-center justify-center gap-5 text-sm font-medium text-foreground/80 sm:flex">
          {navLinks.map(({ href, labelKey }) => (
            <Link key={href} href={href}>
              {t(labelKey)}
            </Link>
          ))}
        </nav>

        <div className="ml-auto flex items-center gap-3">
          <div className="hidden items-center gap-1 text-sm sm:flex">
            {locales.map((l) => (
              <button
                key={l}
                type="button"
                onClick={() => switchLocale(l)}
                className={`rounded px-2 py-1 uppercase ${locale === l ? "bg-primary text-white" : "text-primary hover:bg-primary/10"}`}
                aria-label={`${t("language")} ${l}`}
              >
                {l}
              </button>
            ))}
          </div>

          {session?.user ? (
            <>
              <Link
                href="/account"
                className="text-sm font-medium text-foreground/80 hover:text-primary"
              >
                {t("account")}
              </Link>
              <button
                type="button"
                onClick={() => signOut()}
                className="rounded border border-primary/20 px-3 py-1.5 text-sm text-primary"
              >
                {t("signOut")}
              </button>
            </>
          ) : (
            <Link
              href="/sign-in"
              className="rounded bg-primary px-4 py-1.5 text-sm font-medium text-primary-foreground"
            >
              {t("signIn")}
            </Link>
          )}
        </div>
      </div>

      <nav className="flex flex-wrap items-center justify-center gap-4 border-t border-primary/5 px-4 py-2 text-sm font-medium text-foreground/80 sm:hidden">
        {navLinks.map(({ href, labelKey }) => (
          <Link key={href} href={href}>
            {t(labelKey)}
          </Link>
        ))}
      </nav>
    </header>
  );
}
