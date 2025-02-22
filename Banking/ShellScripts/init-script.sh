#!/bin/bash

# Check if the database is empty
DB_NAME="banking"
TABLE_COUNT=$(mysql -u root -e "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '$DB_NAME';" -s -N)

if [ "$TABLE_COUNT" -eq 0 ]; then
  echo "Database is empty. Running init.sql..."
  mysql -u root $DB_NAME < /docker-entrypoint-initdb.d/init.sql
else
  echo "Database is not empty. Skipping init.sql..."
fi
