FROM microsoft/dotnet:2.0.0-sdk-nanoserver

COPY . .

ENTRYPOINT [ "powershell" ]
