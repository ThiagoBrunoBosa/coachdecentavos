# Coach de Centavos

Financial coaching platform for **Carolyne Moraes** (CFP®). Monorepo: Next.js web (Vercel) + .NET API (Render) + PostgreSQL (Neon).

## Structure

```
apps/web/                          Next.js 16 (PT/EN)
src/CoachDecentavos.Api/           ASP.NET Core entry
src/CoachDecentavos.Application/   Use cases
src/CoachDecentavos.Domain/        Entities
src/CoachDecentavos.Infrastructure EF Core, external services
tests/CoachDecentavos.Tests/       xUnit + FluentAssertions (ready, no tests yet)
```

## Prerequisites

- .NET 10 SDK
- Node.js 20+
- Docker Desktop

## Local development (Docker Desktop — recommended)

Full stack with Postgres + API + Web:

```bash
# Copy env and start everything
cp .env.docker.example .env.docker
# Optional: set GOOGLE_CLIENT_ID/SECRET, LLM_API_KEY in .env.docker

docker compose --env-file .env.docker up --build
```

| Service | URL |
|---------|-----|
| Web | http://localhost:3000/pt |
| API | http://localhost:5299/api/v1/health |
| Postgres | localhost:5432 (user/pass/db: postgres/postgres/coachdecentavos) |

Admin: http://localhost:3000/admin/login — `admin@local.dev` / `ChangeMe123!`

User (Docker/Development seed): `thiago@local.dev` / `Thiago123456!` — Thiago Bosa

Demo client (Docker/Development seed): `demo@local.dev` / `Demo123456!` — has one active product and one pending entitlement to test link-purchase.

No Google, Hotmart, Groq, or YouTube keys required for local testing. Credentials login and email/password registration work out of the box.

Migrations and demo seed run automatically when the API starts (`ASPNETCORE_ENVIRONMENT=Docker`).

Stop: `docker compose down` · Reset DB: `docker compose down -v`

## Local development (without Docker)

```bash
# 1. Database
npm run db:up

# 2. API (http://localhost:5299)
npm run api:dev
# Migrations run automatically in Development.
# Admin seed: admin@local.dev / ChangeMe123! (launchSettings.json)
# User seed: thiago@local.dev / Thiago123456!

# 3. Web (http://localhost:3000)
cd apps/web
cp .env.local.example .env.local
# Set NEXTAUTH_SECRET, GOOGLE_CLIENT_ID/SECRET (optional for credentials login)
npm install
npm run dev
```

Root shortcuts: `npm run db:migrate`, `npm run build`, `npm run neon:create-db`

## Environments

| Env | API | Web | Config |
|-----|-----|-----|--------|
| **Development** | `appsettings.Development.json` + launchSettings | `.env.local` | Auto-migrate + demo seed |
| **Staging** | Render env vars, `ASPNETCORE_ENVIRONMENT=Staging` | Vercel preview | `appsettings.Staging.json` CORS |
| **Production** | Render env vars, `ASPNETCORE_ENVIRONMENT=Production` | Vercel prod | Secrets via platform only |

Copy [`.env.example`](.env.example) for local API overrides. Never commit secrets.

## API (`/api/v1`)

| Area | Routes |
|------|--------|
| Health | `GET /health` |
| Auth | `POST /auth/register`, `/login`, `/sso`, `/refresh`, `/logout` |
| Leads | `POST /leads`, `GET /admin/leads` |
| Admin | `GET /admin/bookings`, `POST /admin/bookings/{id}/confirm` |
| Catalog | `GET /products`, `/products/{slug}`, `/shorts` |
| Consulting | `GET /consulting/packages`, `/consulting/slots` |
| Client | `GET /me/entitlements`, `POST /me/entitlements/link-purchase`, `/me/bookings`, `/me/ai/*` |
| Webhooks | `POST /webhooks/hotmart` |
| Internal | `POST /internal/youtube/sync` (Render Cron + `X-Cron-Secret`) |

OpenAPI (dev): `http://localhost:5299/openapi/v1.json`

## Deploy (free tier)

- **Vercel:** root `apps/web`
- **Render:** [`render.yaml`](render.yaml) — API Docker + YouTube cron
- **Neon:** `npm run neon:create-db` then `npm run db:migrate` with direct connection URL

## Conventions

All source code in **English**. UI strings in `apps/web/messages/{pt,en}.json`. See [CONVENTIONS.md](CONVENTIONS.md).
