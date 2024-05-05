FROM node:latest

ENV NODE_ENV=development

WORKDIR /app

COPY package.json package.json
COPY package-lock.json package-lock.json

RUN npm i

VOLUME /app

CMD [ "npm", "run", "dev", "--", "--port", "5173" ]
EXPOSE 5173