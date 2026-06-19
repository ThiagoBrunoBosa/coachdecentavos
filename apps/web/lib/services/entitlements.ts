import { apiFetch } from "@/lib/api-client";
import type { Entitlement } from "@/lib/types/api";

export async function listEntitlements(accessToken: string): Promise<Entitlement[]> {
  return apiFetch<Entitlement[]>("/me/entitlements", { accessToken });
}
