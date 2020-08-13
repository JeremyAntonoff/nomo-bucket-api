FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

WORKDIR /nomobucket

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /nomobucket
ENV ASPNETCORE_URLS=http://*:6000
COPY --from=build /nomobucket/out .
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.2.1/wait /wait
RUN chmod +x /wait
CMD /wait && dotnet NomoBucket.API.dll