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
