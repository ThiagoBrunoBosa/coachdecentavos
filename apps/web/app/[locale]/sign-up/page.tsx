import { setRequestLocale } from "next-intl/server";
import { SignUpForm } from "@/components/SignUpForm";

export default async function SignUpPage({
  params,
}: {
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  setRequestLocale(locale);

  return (
    <div className="px-4 py-12">
      <SignUpForm />
    </div>
  );
}
