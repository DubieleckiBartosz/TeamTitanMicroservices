CONN_STRING="-S localhost -U sa -P sql12345"
SQL_CMD="/opt/mssql-tools/bin/sqlcmd" 

SCRIPTS[0]="$SQL_CMD $CONN_STRING -d master -i Identity-create-db.sql" 
SCRIPTS[1]="$SQL_CMD $CONN_STRING -d TeamTitanIdentity -i Identity-create-tables.sql"   
SCRIPTS[2]="$SQL_CMD $CONN_STRING -d TeamTitanIdentity -i Identity-create-storedProcedures.sql" 
SCRIPTS[3]="$SQL_CMD $CONN_STRING -d TeamTitanIdentityTests -i Identity-create-tables.sql" 
SCRIPTS[4]="$SQL_CMD $CONN_STRING -d TeamTitanIdentityTests -i Identity-create-storedProcedures.sql"
SCRIPTS[5]="$SQL_CMD $CONN_STRING -d master -i Calculator-create-db.sql" 
SCRIPTS[6]="$SQL_CMD $CONN_STRING -d TeamTitanCalculator -i Calculator-create-tables.sql" 
SCRIPTS[7]="$SQL_CMD $CONN_STRING -d TeamTitanCalculator -i Calculator-create-storedProcedures.sql"
SCRIPTS[8]="$SQL_CMD $CONN_STRING -d master -i Background-create-db.sql"

for ((i = 0; i < ${#SCRIPTS[@]}; i++))
do   
    echo "start"
    for x in {1..30};
    do
        ${SCRIPTS[$i]};  
        if [ $? -eq 0 ]
        then
            echo "Operation number ${i} completed"
            break
        else
            echo "Operation number ${i} not ready yet..."
            sleep 1
        fi
    done 
done 