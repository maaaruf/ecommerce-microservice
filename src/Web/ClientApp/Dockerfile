# Build stage
FROM node:18-alpine AS build

WORKDIR /app

# Copy package files
COPY src/Web/ClientApp/package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy source code
COPY src/Web/ClientApp/ .

# Build the app
RUN npm run build

# Production stage
FROM nginx:alpine

# Copy built app to nginx
COPY --from=build /app/build /usr/share/nginx/html

# Copy nginx configuration
COPY src/Web/ClientApp/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"] 