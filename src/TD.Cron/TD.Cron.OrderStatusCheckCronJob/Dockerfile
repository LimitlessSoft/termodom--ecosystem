FROM node:latest

WORKDIR /app

## Copy shared packages
COPY ./package.json ./
COPY ./src/TD.Cron/TD.Cron.Common/TD.Cron.Common.Domain.Node ./src/TD.Cron/TD.Cron.Common/TD.Cron.Common.Domain.Node
COPY ./src/TD.Web/TD.Web.Common/TD.Web.Common.Repository.Node ./src/TD.Web/TD.Web.Common/TD.Web.Common.Repository.Node
COPY ./src/TD.Web/TD.Web.Common/TD.Web.Common.Domain.Node ./src/TD.Web/TD.Web.Common/TD.Web.Common.Domain.Node
COPY ./src/TD.Web/TD.Web.Common/TD.Web.Common.Contracts.Node ./src/TD.Web/TD.Web.Common/TD.Web.Common.Contracts.Node

## Copy job package
COPY ./src/TD.Cron/TD.Cron.OrderStatusCheckCronJob ./src/TD.Cron/TD.Cron.OrderStatusCheckCronJob
WORKDIR /app/src/TD.Cron/TD.Cron.OrderStatusCheckCronJob
RUN npm install

ARG DB_USER
ARG DB_HOST
ARG DB_NAME
ARG DB_PASS
ARG DB_PORT
ARG BASE_KOMERCIJALNO_API_URL

ENV DB_USER=${DB_USER}
ENV DB_HOST=${DB_HOST}
ENV DB_NAME=${DB_NAME}
ENV DB_PASS=${DB_PASS}
ENV DB_PORT=${DB_PORT}
ENV BASE_KOMERCIJALNO_API_URL=${BASE_KOMERCIJALNO_API_URL}
ENV NODE_TLS_REJECT_UNAUTHORIZED=0

CMD [ "npm", "run", "start" ]