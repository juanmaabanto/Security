FROM mcr.microsoft.com/dotnet/aspnet:6.0
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-bin/Debug/net6.0/publish} .
ENTRYPOINT ["dotnet", "N5.Challenge.Services.Security.API.dll"]