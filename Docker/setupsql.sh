CONN_STRING="-S localhost -U sa -P a?i0/cEFB@v3dweF7C"

run_command() {
  local command="$1"
  echo "Starting: $command"
  for i in {1..30}; do
    $command
    if [ $? -eq 0 ]; then
      echo "Operation completed"
      return 0
    else
      echo "Operation not ready yet..."
      sleep $i
    fi
  done
  echo "Operation failed"
  return 1
}

declare -a SCRIPTS=(
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d master -i Identity-create-db.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanIdentity -i Identity-create-tables.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanIdentity -i Identity-create-storedProcedures.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanIdentityTests -i Identity-create-tables.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanIdentityTests -i Identity-create-storedProcedures.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d master -i TeamTitanCalculator-create-db.sql"  
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanCalculator -i TeamTitanCalculator-create-tables.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanCalculator -i TeamTitanCalculator-create-storedProcedures.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanCalculatorTests -i TeamTitanCalculator-create-tables.sql"
  "/opt/mssql-tools/bin/sqlcmd $CONN_STRING -d TeamTitanCalculatorTests -i TeamTitanCalculator-create-storedProcedures.sql"
)

# Loop through the array and run each SQLCMD command
for command in "${SCRIPTS[@]}"; do
  run_command "$command"
done