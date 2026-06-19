import { getTranslations } from "next-intl/server";
import { Link } from "@/i18n/navigation";
import { DeveloperCredit } from "@/components/DeveloperCredit";

export async function Footer() {
  const t = await getTranslations("common");
  const tf = await getTranslations("footer");
  const year = new Date().getFullYear();

  const navLinks = [
    { href: "/about" as const, label: t("about") },
    { href: "/consulting" as const, label: t("consulting") },
    { href: "/products" as const, label: t("products") },
    { href: "/shorts" as const, label: t("shorts") },
  ];

  return (
    <footer className="mt-auto border-t border-primary/10 bg-primary text-primary-foreground">
      <div className="mx-auto grid max-w-6xl gap-8 px-4 py-12 sm:grid-cols-2 lg:grid-cols-4">
        <div className="sm:col-span-2 lg:col-span-1">
          <p className="font-serif text-xl">{t("siteName")}</p>
          <p className="mt-2 text-sm opacity-80">{tf("tagline")}</p>
        </div>
        <div>
          <p className="text-xs font-medium uppercase tracking-widest opacity-70">{tf("navTitle")}</p>
          <nav className="mt-3 flex flex-col gap-2 text-sm opacity-90">
            {navLinks.map((link) => (
              <Link key={link.href} href={link.href} className="hover:underline">
                {link.label}
              </Link>
            ))}
          </nav>
        </div>
        <div>
          <p className="text-xs font-medium uppercase tracking-widest opacity-70">{tf("contactTitle")}</p>
          <p className="mt-3 text-sm opacity-80">{tf("contactBody")}</p>
          <Link
            href="/consulting/interest"
            className="mt-3 inline-block text-sm font-medium text-accent hover:underline"
          >
            {tf("interestLink")} →
          </Link>
        </div>
        <div>
          <p className="text-xs font-medium uppercase tracking-widest opacity-70">{t("privacy")}</p>
          <nav className="mt-3 flex flex-col gap-2 text-sm opacity-90">
            <Link href="/privacy" className="hover:underline">
              {t("privacy")}
            </Link>
            <Link href="/terms" className="hover:underline">
              {t("terms")}
            </Link>
          </nav>
        </div>
      </div>
      <div className="border-t border-primary-foreground/10 px-4 py-4">
        <div className="mx-auto flex max-w-6xl flex-col items-center justify-between gap-2 text-xs opacity-70 sm:flex-row">
          <p>
            © {year} {t("siteName")}. {tf("rights")}
          </p>
          <DeveloperCredit className="text-center sm:text-right" />
        </div>
      </div>
    </footer>
  );
}
