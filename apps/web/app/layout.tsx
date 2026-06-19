import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Coach de Centavos",
  description: "Financial education with Carolyne",
};

export default function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return children;
}
