FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
#RUN curl -X GET http://172.23.238.232:8010/api/values
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "QuizServer.dll"]