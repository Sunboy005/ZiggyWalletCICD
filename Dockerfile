#FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
#WORKDIR /app
#EXPOSE 80
#
#ENV ASPNETCORE_URLS=http://+:80
#
#FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
#WORKDIR /src
#COPY [". ./*csproj"]
#RUN dotnet restore "ZiggyZiggyWallet/ZiggyZiggyWallet.csproj"
#COPY . .
#WORKDIR "/src/ZiggyZiggyWallet"
#RUN dotnet build "ZiggyZiggyWallet.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "ZiggyZiggyWallet.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ZiggyZiggyWallet.dll"]
#

# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app
#EXPOSE 80



# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore



# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out



# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .



#ENV ASPNETCORE_URL=http://*:$PORT
#ENTRYPOINT ["dotnet","ZiggyZiggyWallet.dll"]

CMD ASPNETCORE_URLS=http://*:PORT dotnet ziggywallet.Core.dll