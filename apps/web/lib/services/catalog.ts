import { apiFetch } from "@/lib/api-client";
import type { ProductDetail, ProductSummary, YouTubeShortItem } from "@/lib/types/api";

export async function listProducts(): Promise<ProductSummary[]> {
  return apiFetch<ProductSummary[]>("/products");
}

export async function getProduct(slug: string): Promise<ProductDetail | null> {
  try {
    return await apiFetch<ProductDetail>(`/products/${slug}`);
  } catch {
    return null;
  }
}

export async function listShorts(): Promise<YouTubeShortItem[]> {
  return apiFetch<YouTubeShortItem[]>("/shorts");
}

export async function listLatestShorts(limit = 3): Promise<YouTubeShortItem[]> {
  return apiFetch<YouTubeShortItem[]>(`/shorts?sort=latest&limit=${limit}`);
}
