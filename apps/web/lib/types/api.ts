export type ProductSummary = {
  id: string;
  slug: string;
  name: string;
  type: string;
  price: number;
  currency: string;
  hotmartCheckoutUrl?: string | null;
};

export type ProductDetail = ProductSummary & {
  description?: string | null;
};

export type YouTubeShortItem = {
  videoId: string;
  title: string;
  thumbnailUrl?: string | null;
};

export type Entitlement = {
  id: string;
  productId: string;
  productSlug: string;
  productName: string;
  status: string;
  activatedAtUtc?: string | null;
};

export type ConsultingPackage = {
  id: string;
  slug: string;
  name: string;
  description?: string | null;
  durationMinutes: number;
  price: number;
  currency: string;
};

export type AvailabilitySlot = {
  id: string;
  startsAtUtc: string;
  endsAtUtc: string;
};

export type Booking = {
  id: string;
  packageName: string;
  startsAtUtc: string;
  endsAtUtc: string;
  status: string;
  meetingUrl?: string | null;
};

export type AdminLead = {
  id: string;
  email: string;
  name?: string | null;
  phone?: string | null;
  source?: string | null;
  message?: string | null;
  status: number | string;
  createdAtUtc: string;
};

export type AdminAvailabilitySlot = {
  id: string;
  startsAtUtc: string;
  endsAtUtc: string;
  isBlocked: boolean;
  isBooked: boolean;
};

export type AdminBooking = {
  id: string;
  userName: string;
  userEmail: string;
  packageName: string;
  startsAtUtc: string;
  endsAtUtc: string;
  status: string;
};
