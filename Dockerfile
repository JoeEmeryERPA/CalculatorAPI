#: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application and build
COPY . ./
RUN dotnet publish -c Release -o out

# : Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set environment variables for JWT configuration 
ENV JWT__Issuer="YourIssuer"
ENV JWT__Audience="YourAudience"
ENV JWT__SecretKey="YourSuperSecretKeyHere1331451515151231"
ENV JWT__ExpiryInMinutes="60"

# Expose port 80
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "CalculatorAPI.dll"]
