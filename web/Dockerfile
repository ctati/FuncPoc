FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5101

ENV ASPNETCORE_URLS=http://+:5101

ARG DEFINES
RUN echo ${DEFINES}

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["web.csproj", "./"]
RUN dotnet restore "web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "web.csproj" -c Release -o /app/build /p:DefineConstants=${DEFINES}
# TODO: doesn't look like the command line option is working... temporarly using csproj to achieve the same

FROM build AS publish
RUN dotnet publish "web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web.dll"]
