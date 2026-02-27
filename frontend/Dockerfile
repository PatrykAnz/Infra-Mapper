FROM node:25-alpine
WORKDIR /frontend
COPY package.json package-lock.json ./
RUN npm install
COPY . .
ENV PORT=5173
ENV HOST=0.0.0.0
EXPOSE 5173
CMD ["npm", "run", "dev", "--", "--host"]
