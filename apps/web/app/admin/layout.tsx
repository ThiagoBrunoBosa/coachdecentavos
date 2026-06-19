import { Fraunces, Plus_Jakarta_Sans } from "next/font/google";
import { AdminFooter } from "@/components/admin/AdminFooter";
import "../globals.css";

const fraunces = Fraunces({ subsets: ["latin"], variable: "--font-fraunces" });
const plusJakarta = Plus_Jakarta_Sans({ subsets: ["latin"], variable: "--font-plus-jakarta" });

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={`${fraunces.variable} ${plusJakarta.variable}`}>
      <body className="flex min-h-screen flex-col bg-background antialiased">
        {children}
        <AdminFooter />
      </body>
    </html>
  );
}
