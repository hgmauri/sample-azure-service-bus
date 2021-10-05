FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.AzureServiceBus.WebApi/Sample.AzureServiceBus.WebApi.csproj", "src/Sample.AzureServiceBus.WebApi/"]
RUN dotnet restore "src/Sample.AzureServiceBus.WebApi/Sample.AzureServiceBus.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Sample.AzureServiceBus.WebApi"
RUN dotnet build "Sample.AzureServiceBus.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.AzureServiceBus.WebApi.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.AzureServiceBus.WebApi.dll"]