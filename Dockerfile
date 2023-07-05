#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["back.csproj", "."]
RUN dotnet restore "./back.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "back.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "back.dll"]

#RUN ../import_sql.sh

# ENTRYPOINT [ "dotnet", "HerokuApp.dll" ]
# Use the following instead for Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HerokuApp.dll