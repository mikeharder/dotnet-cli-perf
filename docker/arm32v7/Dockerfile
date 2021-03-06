FROM arm32v7/buildpack-deps:stretch-scm

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
ENV DOTNET_SDK_VERSION 2.1.300-preview2-008380

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-arm.tar.gz \
    && dotnet_sha512='3f8276cd982f309236d5cac6656c9364cb190d1cb1ac0474acd67e16f35b8bb5d66fcb220342f0edb80c548a073d5e7f7330ceeeebdc88bceb8ea4836cd1abc6' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER true

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER true

ENV NUGET_XMLDOC_MODE skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

# Copy csproj
WORKDIR /app
COPY Common/*.csproj ./Common/
COPY DotNetCliPerf/*.csproj ./DotNetCliPerf/

# Restore
WORKDIR /app/DotNetCliPerf

# Option "--disable-parallel" is required to prevent restore failures on slow machines like Raspberry Pi.
RUN dotnet restore --disable-parallel

# Copy everything else
WORKDIR /app
COPY ./Common ./Common/
COPY ./DotNetCliPerf ./DotNetCliPerf/

# Build
WORKDIR /app/DotNetCliPerf
RUN dotnet publish -c Release -o out

# Required for Util.RepoRoot property to work correctly
WORKDIR /app
COPY *.sln ./

WORKDIR /app/DotNetCliPerf/out
ENTRYPOINT ["dotnet", "DotNetCliPerf.dll"]
