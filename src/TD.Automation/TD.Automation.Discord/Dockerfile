FROM node:latest

WORKDIR /app

COPY ./src/TD.Automation/TD.Automation.Discord/package.json ./
COPY ./src/TD.Automation/TD.Automation.Discord/index.js ./
COPY ./src/TD.Automation/TD.Automation.Discord/domain ./domain

RUN npm install

CMD [ "node", "index.js" ]
