#CONN_STRING="-S localhost -U sa -P sql123456(!)"
#SQL_CMD="/opt/mssql-tools/bin/sqlcmd" 

#!/bin/bash

SCRIPTS[0]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d master -i Identity-create-db.sql" 
SCRIPTS[1]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanIdentity -i Identity-create-tables.sql" # -v VERBOSITY=1
SCRIPTS[2]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanIdentity -i Identity-create-storedProcedures.sql" 
SCRIPTS[3]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanIdentityTests -i Identity-create-tables.sql" 
SCRIPTS[4]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanIdentityTests -i Identity-create-storedProcedures.sql" 
SCRIPTS[5]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d master -i Calculator-create-db.sql" 
SCRIPTS[6]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanCalculator -i Calculator-create-tables.sql" 
SCRIPTS[7]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d TeamTitanCalculator -i Calculator-create-storedProcedures.sql" 
SCRIPTS[8]="/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P sql123456(!) -d master -i Background-create-db.sql" 

for ((i = 0; i < ${#SCRIPTS[@]}; i++))
do
    echo "Starting operation ${i}"
    for x in {1..30};
    do
        ${SCRIPTS[$i]}
        if [ $? -eq 0 ]
        then
            echo "Operation number ${i} completed"
            break
        else
            echo "Operation number ${i} not ready yet..."
            sleep 2
        fi
    done
done
