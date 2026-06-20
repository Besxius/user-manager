# --- Giai đoạn 1: Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# --- Giai đoạn 2: Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# BƯỚC QUAN TRỌNG: Copy TẤT CẢ các file .csproj của các dự án con vào đúng thư mục tương ứng
COPY ["UserManager.Api/UserManager.Api.csproj", "UserManager.Api/"]
COPY ["UserManager.Application/UserManager.Application.csproj", "UserManager.Application/"]
COPY ["UserManager.Infrastructure/UserManager.Infrastructure.csproj", "UserManager.Infrastructure/"]
COPY ["UserManager.Domain/UserManager.Domain.csproj", "UserManager.Domain/"]

# Chạy restore trên dự án Startup (Nó sẽ tự động restore các dự án phụ thuộc đã copy ở trên)
RUN dotnet restore "UserManager.Api/UserManager.Api.csproj"

# Sau khi restore xong mới copy toàn bộ mã nguồn vào để build
COPY . .

# Di chuyển vào thư mục của dự án Startup để chạy lệnh Build
WORKDIR "/src/UserManager.Api"
RUN dotnet build "UserManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- Giai đoạn 3: Publish ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- Giai đoạn 4: Final chạy ứng dụng ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManager.Api.dll"]