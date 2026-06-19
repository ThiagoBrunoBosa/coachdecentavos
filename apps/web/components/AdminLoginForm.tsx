"use client";

import { signIn } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useState } from "react";

export function AdminLoginForm() {
  const router = useRouter();
  const [error, setError] = useState<string | null>(null);

  async function onSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError(null);
    const form = new FormData(e.currentTarget);
    const res = await signIn("credentials", {
      email: form.get("email"),
      password: form.get("password"),
      redirect: false,
    });
    if (res?.error) {
      setError("Invalid credentials");
      return;
    }
    router.push("/admin");
    router.refresh();
  }

  return (
    <form onSubmit={onSubmit} className="mx-auto mt-24 max-w-md space-y-4 rounded-xl bg-white p-8 shadow">
      <h1 className="font-serif text-2xl text-primary">Admin login</h1>
      <input name="email" type="email" placeholder="Email" required className="w-full rounded border px-3 py-2" />
      <input name="password" type="password" placeholder="Password" required className="w-full rounded border px-3 py-2" />
      <button type="submit" className="w-full rounded bg-primary px-4 py-2 text-primary-foreground">
        Sign in
      </button>
      {error && <p className="text-sm text-red-600">{error}</p>}
    </form>
  );
}
