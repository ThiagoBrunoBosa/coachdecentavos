import { getTranslations } from "next-intl/server";

const LEXTECH_URL = "https://www.lextechsolutions.com.br/";

type DeveloperCreditProps = {
  className?: string;
  linkClassName?: string;
};

export async function DeveloperCredit({
  className = "text-xs opacity-70",
  linkClassName = "font-medium text-accent hover:underline",
}: DeveloperCreditProps) {
  const tf = await getTranslations("footer");

  return (
    <p className={className}>
      {tf("developedBy")}{" "}
      <a
        href={LEXTECH_URL}
        target="_blank"
        rel="noopener noreferrer"
        className={linkClassName}
      >
        LexTech Solutions
      </a>
    </p>
  );
}
