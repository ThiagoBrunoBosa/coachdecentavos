"use client";

import { API_PUBLIC_URL } from "./api-config";

export type ApiRequestOptions = RequestInit & {
  accessToken?: string;
};

export async function apiFetchBrowser<T>(
  path: string,
  options: ApiRequestOptions = {},
): Promise<T> {
  const { accessToken, headers, ...rest } = options;
  const res = await fetch(`${API_PUBLIC_URL}${path}`, {
    ...rest,
    headers: {
      "Content-Type": "application/json",
      ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {}),
      ...(headers ?? {}),
    },
  });

  if (!res.ok) {
    throw new Error(`API error ${res.status}: ${path}`);
  }

  if (res.status === 204) {
    return undefined as T;
  }

  return (await res.json()) as T;
}
