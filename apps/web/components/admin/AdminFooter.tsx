import { DeveloperCredit } from "@/components/DeveloperCredit";

export async function AdminFooter() {
  return (
    <footer className="mt-auto border-t border-primary/10 bg-background">
      <DeveloperCredit
        className="py-4 text-center text-xs text-foreground/60"
        linkClassName="font-medium text-primary hover:underline"
      />
    </footer>
  );
}
