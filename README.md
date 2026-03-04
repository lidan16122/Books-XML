# Books XML
## a simple project that saves books data in XML
### how to make it work
1. in the serverside create a .env file inside server\server.API with this parameters
```
CLIENT_URL=http://localhost:4200
ASPNETCORE_ENVIRONMENT=Development
APP_URL=https://localhost:7044
BOOKS_XML_PATH=D:\Lidan\books.xml
```
it can be any other value, the values are examples, but it is important the client side will be with the same parameters

now run the server (dotnet run) from server.API

2. in the client side create a env.ts inside client\insurtix-client\src\environments with this parameter
 ```
export const environment = {
  apiUrl: 'https://localhost:7044'
};
   ```
the apiUrl is the same APP_URL in the server side parameter
now use ``` npm i ``` inside client\client
then use ``` npm start ``` to start the app

**failure to perform any of the steps will result in errors!**

