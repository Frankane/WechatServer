FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 7895
EXPOSE 44389

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["WeChatServer/WeChatServer.csproj", "WeChatServer/"]
RUN dotnet restore "WeChatServer/WeChatServer.csproj"
COPY . .
WORKDIR "/src/WeChatServer"
RUN dotnet build "WeChatServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WeChatServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WeChatServer.dll"]