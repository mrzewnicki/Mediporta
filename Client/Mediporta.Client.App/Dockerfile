# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Client/Mediporta.Client.App/Mediporta.Client.App.csproj", "Client/Mediporta.Client.App/"]
COPY ["Mediporta.Shared/Mediporta.Shared.csproj", "Mediporta.Shared/"]
RUN dotnet restore "Client/Mediporta.Client.App/Mediporta.Client.App.csproj"

# Copy rest of the source
COPY . .
WORKDIR /src/Client/Mediporta.Client.App
RUN dotnet publish -c Release -o /app/publish

# Runtime stage - Nginx
FROM nginx:stable-alpine AS runtime
WORKDIR /usr/share/nginx/html

# custom nginx config
COPY Client/Mediporta.Client.App/nginx.conf /etc/nginx/conf.d/default.conf

# published files
COPY --from=build /app/publish/wwwroot .