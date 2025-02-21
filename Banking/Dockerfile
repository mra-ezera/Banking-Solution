FROM mysql:8.0

# Set environment variables
ENV MYSQL_ALLOW_EMPTY_PASSWORD=yes
ENV MYSQL_DATABASE=banking

# Copy the SQL file and init script
COPY Data/init.sql /docker-entrypoint-initdb.d/
COPY ShellScripts/init-script.sh /docker-entrypoint-initdb.d/

# Make scripts executable
RUN chmod 644 /docker-entrypoint-initdb.d/init.sql
RUN chmod +x /docker-entrypoint-initdb.d/init-script.sh

EXPOSE 3306
