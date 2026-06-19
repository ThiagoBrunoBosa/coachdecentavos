import { setRequestLocale } from "next-intl/server";
import { SignInForm } from "@/components/SignInForm";

export default async function SignInPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);

  return (
    <div className="px-4 py-12">
      <SignInForm />
    </div>
  );
}
