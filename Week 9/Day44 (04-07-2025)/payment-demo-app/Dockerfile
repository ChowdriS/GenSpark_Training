FROM node:22

WORKDIR /app

COPY package.json .
RUN npm install

COPY . .

EXPOSE 4200

CMD ["npm","start"]



FROM node:22-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install -g @angular/cli && npm install

COPY . .

RUN ng build --configuration production

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /app/dist/payment-demo-app /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
