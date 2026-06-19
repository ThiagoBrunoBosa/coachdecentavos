import { apiFetch } from "@/lib/api-client";
import type {
  AvailabilitySlot,
  Booking,
  ConsultingPackage,
} from "@/lib/types/api";

export async function listConsultingPackages(): Promise<ConsultingPackage[]> {
  return apiFetch<ConsultingPackage[]>("/consulting/packages");
}

export async function listAvailabilitySlots(): Promise<AvailabilitySlot[]> {
  return apiFetch<AvailabilitySlot[]>("/consulting/slots");
}

export async function listMyBookings(accessToken: string): Promise<Booking[]> {
  return apiFetch<Booking[]>("/me/bookings", { accessToken });
}
