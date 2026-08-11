FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file(s) from root
COPY ["SmallTodoApi.csproj", "./"]
RUN dotnet restore "SmallTodoApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "SmallTodoApi.csproj" -c Release -o /app/build
RUN dotnet publish "SmallTodoApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmallTodoApi.dll"]