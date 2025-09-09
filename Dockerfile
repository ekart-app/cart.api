# Stage 1 
#Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

#Copy Project files and restore project dependencies
COPY *.csproj ./

#Copy entire project and build
COPY . .
RUN dotnet publish -c Release -o /app/publish


# Stage 2
#Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

#Copy published application from the build stage
COPY --from=build /app/publish .

#Expose the port the api will listen to
EXPOSE 8080

#Define the entrypoint to run the application
ENTRYPOINT [ "dotnet","cart.api.dll" ]
