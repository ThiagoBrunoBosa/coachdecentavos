/** Browser-facing API (host machine). */
export const API_PUBLIC_URL =
  process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5299/api/v1";

/** Server-side API (Docker internal network or local). */
export const API_BASE_URL =
  process.env.API_INTERNAL_URL ?? API_PUBLIC_URL;
