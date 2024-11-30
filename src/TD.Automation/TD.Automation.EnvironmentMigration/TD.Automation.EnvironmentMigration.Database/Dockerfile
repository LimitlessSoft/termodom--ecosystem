# Use the official Node.js image as a base image
FROM node:22

RUN apt update
RUN apt install -y postgresql-client-15

WORKDIR /app

COPY . .

RUN npm install

CMD ["node", "index.js"]