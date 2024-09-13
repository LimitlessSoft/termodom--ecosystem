FROM node:latest

WORKDIR /app

COPY ./src/cron/node-shared/db-connection ./node-shared/db-connection
WORKDIR /app/node-shared/db-connection
RUN npm install

WORKDIR /app

COPY ./src/cron/node-shared/jobs-common ./node-shared/jobs-common
WORKDIR /app/node-shared/jobs-common
RUN npm install

WORKDIR /app

COPY ./src/cron/service-1 ./service-1
WORKDIR /app/service-1
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