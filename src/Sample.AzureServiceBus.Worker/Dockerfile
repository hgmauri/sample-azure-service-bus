FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Sample.AzureServiceBus.Worker/Sample.AzureServiceBus.Worker.csproj", "src/Sample.AzureServiceBus.Worker/"]
RUN dotnet restore "src/Sample.AzureServiceBus.Worker/Sample.AzureServiceBus.Worker.csproj"
COPY . .
WORKDIR "/src/src/Sample.AzureServiceBus.Worker"
RUN dotnet build "Sample.AzureServiceBus.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.AzureServiceBus.Worker.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.AzureServiceBus.Worker.dll"]