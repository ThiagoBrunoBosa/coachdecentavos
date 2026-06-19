import { AuthSessionProvider } from "@/providers/session-provider";
import { AdminLoginForm } from "@/components/AdminLoginForm";

export default function AdminLoginPage() {
  return (
    <AuthSessionProvider>
      <AdminLoginForm />
    </AuthSessionProvider>
  );
}
