FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
COPY . ./

# Adicionar a fonte privada do GitHub Packages
RUN dotnet nuget add source "https://nuget.pkg.github.com/caiofabiogomes/index.json" \
    --name github \
    --username caiofabiogomes \
    --password "${SECRET_NUGET_PACKAGES}" \
    --store-password-in-clear-text

RUN dotnet restore

RUN dotnet publish TCFiapMicrosserviceConsumerCreateContact.Worker/TCFiapMicrosserviceConsumerCreateContact.Worker.csproj -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /App/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "TCFiapMicrosserviceConsumerCreateContact.Worker.dll"]