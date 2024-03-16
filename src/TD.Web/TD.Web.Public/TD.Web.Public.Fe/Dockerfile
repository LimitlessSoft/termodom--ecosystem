FROM node:18-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

COPY \
  src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package.json \
  src/TD.Web/TD.Web.Public/TD.Web.Public.Fe/package-lock.json* ./
RUN npm ci

FROM node:18-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY src/TD.Web/TD.Web.Public/TD.Web.Public.Fe .

ENV NEXT_TELEMETRY_DISABLED 1
ARG DEPLOY_ENV
ENV DEPLOYMENT_ENVIRONMENT ${DEPLOY_ENV}

RUN npm run build

FROM node:18-alpine AS runner
WORKDIR /app

ENV NODE_ENV production
ENV DEPLOYMENT_ENVIRONMENT ${DEPLOY_ENV}
ENV NEXT_PUBLIC_API_BASE_MAIN_URL "https://api-${DEPLOY_ENV}.termodom.rs"

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder --chown=nextjs:nodejs /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

USER nextjs

EXPOSE 3000

ENV PORT 3000


CMD ["npm", "start"]