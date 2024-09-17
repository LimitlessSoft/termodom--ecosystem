FROM node:22.4-alpine

WORKDIR /app

COPY src/TD.Office/TD.Office.Public/TD.Office.Public.Api.SpecifikacijaNovcaMicroservice .
RUN npm install

EXPOSE 3000

ENV PORT 3000

CMD [ "npm", "run", "start"]