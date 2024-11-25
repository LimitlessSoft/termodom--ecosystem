# Use the official Node.js image as a base image
FROM node:18-alpine

RUN apk add --no-cache postgresql-client

WORKDIR /app

COPY . .

RUN npm install

CMD ["node", "index.js"]