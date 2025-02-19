FROM node:22.4-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

COPY \
  src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/package.json \
  src/TD.Office/TD.Office.Public/TD.Office.Public.Fe/package-lock.json* ./
RUN npm i

FROM node:22.4-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY src/TD.Office/TD.Office.Public/TD.Office.Public.Fe .

ENV NEXT_TELEMETRY_DISABLED=1
ARG DEPLOY_ENV
ARG OVERRIDE_DEPLOY_ENV
ENV DEPLOYMENT_ENVIRONMENT=${DEPLOY_ENV}
ENV NEXT_PUBLIC_API_BASE_MAIN_URL=${OVERRIDE_DEPLOY_ENV:-https://api-${DEPLOY_ENV}.termodom.rs}

RUN npm run build

FROM node:22.4-alpine AS runner
WORKDIR /app

ENV NODE_ENV=production
ENV NEXT_TELEMETRY_DISABLED=1

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder --chown=nextjs:nodejs /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

USER nextjs

EXPOSE 3000

ENV PORT 3000

CMD ["npm", "start"]