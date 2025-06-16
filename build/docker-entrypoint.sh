#!/bin/bash

# Wait for MongoDB to be ready
until nc -z mongodb 27017; do
  echo "Waiting for MongoDB to be ready..."
  sleep 1
done

# Start the application
dotnet SCPArchiveApi.dll
