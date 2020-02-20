FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base

WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS publish
WORKDIR /build
COPY . .
RUN mkdir /app
RUN dotnet publish src/Easynvest.Infohub.Parse.Api/Easynvest.Infohub.Parse.Api.csproj -c Release -o /app 

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Development
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "Easynvest.Infohub.Parse.Api.dll"]
