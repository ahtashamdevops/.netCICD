FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SmallTodoApi/SmallTodoApi.csproj", "SmallTodoApi/"]
RUN dotnet restore "SmallTodoApi/SmallTodoApi.csproj"
COPY . .
WORKDIR "/src/SmallTodoApi"
RUN dotnet build "SmallTodoApi.csproj" -c Release -o /app/build
RUN dotnet publish "SmallTodoApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmallTodoApi.dll"]