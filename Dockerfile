FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 2703
EXPOSE 44387

ARG ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT $ENVIRONMENT

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /thafiles
COPY ["WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/thafiles/WebApi"

RUN dotnet build "WebApi.csproj" -c Release -o /app

FROM build AS publish
ARG VERSION
RUN dotnet publish "WebApi.csproj" -c Release -o /app -p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApi.dll"]