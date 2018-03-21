FROM microsoft/dotnet:sdk as build-env

# Copy csproj
WORKDIR /app
COPY Common/*.csproj ./Common/
COPY DotNetCliPerf/*.csproj ./DotNetCliPerf/

# Restore
WORKDIR /app/DotNetCliPerf
RUN dotnet restore

# Copy everything else
WORKDIR /app
COPY ./Common ./Common/
COPY ./DotNetCliPerf ./DotNetCliPerf/

# Build
WORKDIR /app/DotNetCliPerf
RUN dotnet publish -c Release -o out

# Build runtime image
FROM buildpack-deps:stretch-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        libunwind8 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.300-preview3-008387

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

WORKDIR /app
COPY --from=build-env /app/DotNetCliPerf/out ./

# Required for Util.RepoRoot property to work correctly
COPY *.sln ./

ENTRYPOINT ["dotnet", "DotNetCliPerf.dll"]
