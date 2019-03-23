FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 3185
EXPOSE 44317

FROM microsoft/dotnet:2.2-sdk AS build
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