FROM node:22.4-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

COPY \
  src/TD.Quizz/TD.Quizz.Fe/package.json \
  src/TD.Quizz/TD.Quizz.Fe/package-lock.json* ./
RUN npm i

FROM node:22.4-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY src/TD.Quizz/TD.Quizz.Fe .

ARG NEXTAUTH_SECRET
ARG SUPERBASE_KEY
ARG SUPERBASE_URL
ARG SUPERBASE_SCHEMA
ENV NEXT_TELEMETRY_DISABLED 1
ENV NEXTAUTH_SECRET ${NEXTAUTH_SECRET}
ENV SUPERBASE_KEY ${SUPERBASE_KEY}
ENV SUPERBASE_URL ${SUPERBASE_URL}
ENV SUPERBASE_SCHEMA ${SUPERBASE_SCHEMA}

RUN npm run build

FROM node:22.4-alpine AS runner
WORKDIR /app

ENV NODE_ENV production

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

ENV NEXT_TELEMETRY_DISABLED 1
ENV NEXTAUTH_SECRET ${NEXTAUTH_SECRET}
ENV SUPERBASE_KEY ${SUPERBASE_KEY}
ENV SUPERBASE_URL ${SUPERBASE_URL}
ENV SUPERBASE_SCHEMA ${SUPERBASE_SCHEMA}

COPY --from=builder --chown=nextjs:nodejs /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/public ./public

USER nextjs

EXPOSE 3000

ENV PORT 3000

CMD ["npm", "start"]