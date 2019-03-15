FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev 
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS publish
WORKDIR /build
COPY . .
RUN mkdir /app
RUN dotnet publish src/Easynvest.Infohub.Parse.Api/Easynvest.Infohub.Parse.Api.csproj -c Release -o /app --configfile Nuget.Config

FROM publish AS unit-test
WORKDIR /build
RUN dotnet test

FROM 827461279864.dkr.ecr.sa-east-1.amazonaws.com/easynvest.dotnet.core.sonar:2.2 as quality
WORKDIR /build
COPY --from=unit-test /build .
RUN dotnet sonarscanner begin /k:"Easynvest.Infohub.Parse" && dotnet build --configfile Nuget.Config && dotnet sonarscanner end

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Homolog
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "Easynvest.Infohub.Parse.Api.dll"]
