#!/bin/bash
set -e

if [ "$1" = '/opt/mssql/bin/sqlservr' ]; then
  if [ ! -f /tmp/app-initialized ]; then
    function initialize_app_database() {
      sleep 15s
      /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password-1234 -d master -i init-db.sql
      touch /tmp/app-initialized
    }
    initialize_app_database &
  fi
fi

exec "$@"