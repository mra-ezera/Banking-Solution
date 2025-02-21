#!/bin/bash
set -e

until mysql -u root -e "SELECT 1"; do
  echo "Waiting for MySQL to be ready..."
  sleep 1
done

mysql -u root banking < /docker-entrypoint-initdb.d/init.sql