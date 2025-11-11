# Docker & Render deployment for KpiService

This document explains how to build the project into a Docker image, run it locally, and deploy to Render.com as a Background Worker.

Prerequisites
- Docker installed (for local image build/run)
- A Render.com account and a connected Git repository (optional: you can push a Docker image to a registry instead)

Environment variables required (set in Render Dashboard or locally):
- `MONGO_MAIN_URI` — connection string for the main Harbor database
- `MONGO_KPI_URI` — connection string for the KPIs database

Build the Docker image locally

```bash
# from repo root (where the Dockerfile and .csproj are)
docker build -t kpiservice:local .
```

Run the container locally (pass env vars)

```bash
docker run --rm \
  -e MONGO_MAIN_URI="$MONGO_MAIN_URI" \
  -e MONGO_KPI_URI="$MONGO_KPI_URI" \
  kpiservice:local
```

If you prefer to use an `.env` file in development, load it into your shell before running (bash):

```bash
set -o allexport; source .env; set +o allexport
docker run --rm -e MONGO_MAIN_URI="$MONGO_MAIN_URI" -e MONGO_KPI_URI="$MONGO_KPI_URI" kpiservice:local
```

Deploying to Render.com (recommended: Background Worker)

1. Commit and push this repository to a Git host (GitHub/GitLab/Bitbucket) connected to Render.
2. On Render, create a new service and choose "Background Worker" (not "Web Service").
3. For the deployment method, select "Dockerfile" (Render will build using your Dockerfile).
4. Fill in the environment variables `MONGO_MAIN_URI` and `MONGO_KPI_URI` in the Render dashboard (Settings → Environment).
5. Select the branch and create the service. Render will build the image and run the container. The worker will keep running once the process (the background loop) is started.

Notes and tips
- Render will build the Dockerfile in your repository. The `ENTRYPOINT` in the Dockerfile runs the published `KpiService.dll`.
- Because this is a background worker that does not expose an HTTP port, choose the Background Worker type on Render so the platform expects a long-running process without health checks on a port.
- For production, avoid embedding credentials in `appsettings.json`. Use Render's environment variables or secrets.
- If you prefer to use a container registry (Docker Hub / GitHub Container Registry) instead of letting Render build from source, build & push the image, then create a Render service and point it to your image.

Troubleshooting
- If the container exits immediately:
  - Check container logs in Render or run locally with `docker logs`.
  - Ensure the worker is not throwing an unhandled exception during startup (missing env variables cause exceptions when constructing MongoClient).
- If you see authentication/connection errors, confirm network access from Render to your MongoDB cluster (some managed DBs restrict network egress; configure IP allowlist or use a private connection).

Want me to add a `render.yaml` example or automatic health-check wiring? I can add a sample manifest, or change the repository to upsert latest results rather than inserting every run.
