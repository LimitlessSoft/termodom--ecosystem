FROM node:latest

WORKDIR /app

## Copy shared packages
COPY ./package.json ./
COPY ./src/TD.Cron/TD.Cron.Common/TD.Cron.Common.Domain.Node ./src/TD.Cron/TD.Cron.Common/TD.Cron.Common.Domain.Node
COPY ./src/TD.Web/TD.Web.Common/TD.Web.Common.Repository.Node ./src/TD.Web/TD.Web.Common/TD.Web.Common.Repository.Node
COPY ./src/TD.Web/TD.Web.Common/TD.Web.Common.Contracts.Node ./src/TD.Web/TD.Web.Common/TD.Web.Common.Contracts.Node

## Copy job package
COPY ./src/TD.Cron/TD.Cron.OrderStatusCheckCronJob ./src/TD.Cron/TD.Cron.OrderStatusCheckCronJob
WORKDIR /app/src/TD.Cron/TD.Cron.OrderStatusCheckCronJob
RUN npm install

ENV DB_USER=postgres
ENV DB_HOST=139.177.181.216
ENV DB_NAME=develop_web
ENV DB_PASS=FFnF2JegHu0pt6RmBr5ib2mxRIuaCwNe
ENV DB_PORT=5432
ENV BASE_COMERCIAL_API_URL=https://2024-komercijalno.termodom.rs/dokumenti
ENV WAITING_COLLECTION_ORDER_STATUS=3
ENV TAKEN_ORDER_STATUS=4
ENV NODE_TLS_REJECT_UNAUTHORIZED=0

CMD [ "npm", "run", "start" ]