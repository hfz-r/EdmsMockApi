FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY ./EdmsMockApi/bin/Debug/netcoreapp2.2/publish/ .
ENTRYPOINT ["dotnet", "EdmsMockApi.dll"]