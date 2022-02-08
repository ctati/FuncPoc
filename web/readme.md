
opneapi tool -> dotnet tool install --global Microsoft.dotnet-openapi --version 6.0.1

nswag
install global tool -> npm install nswag -g
use .net 6 runtime -> nswag version /runtime:Net60

nswag openapi2csclient /input:MyWebService.json  /classname:MyServiceClient /namespace:MyNamespace /output:MyServiceClient.cs

nswag openapi2csclient /input:MyWebService.json  /classname:MyServiceClient /namespace:MyNamespace /output:MyServiceClient.cs
