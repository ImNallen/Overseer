# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Overseer.Web/Overseer.Web.csproj", "Overseer.Web/"]
RUN dotnet restore "Overseer.Web/Overseer.Web.csproj"
COPY . .
WORKDIR "/src/Overseer.Web"
RUN dotnet build "Overseer.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Overseer.Web.csproj" -c Release -o /app/publish

# Final stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY Overseer.Web/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80