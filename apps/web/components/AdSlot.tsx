type AdSlotProps = {
  slot: "sidebar" | "inline";
  className?: string;
};

export function AdSlot({ slot, className = "" }: AdSlotProps) {
  return (
    <aside
      aria-label={`Advertisement ${slot}`}
      className={`flex min-h-[90px] items-center justify-center rounded-lg border border-dashed border-accent/40 bg-white/60 text-xs text-foreground/50 ${className}`}
    >
      Ad slot: {slot}
    </aside>
  );
}
