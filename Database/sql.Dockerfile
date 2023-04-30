FROM mcr.microsoft.com/mssql/server:2019-latest 
USER root

ARG PROJECT_DIR=/tmp/mssql-scripts

RUN mkdir -p $PROJECT_DIR
WORKDIR $PROJECT_DIR
ENV SA_PASSWORD=sql123456(!)
ENV ACCEPT_EULA=Y
COPY ./Database/Identity/Identity-create-db.sql ./
COPY ./Database/Identity/Identity-create-tables.sql ./
COPY ./Database/Identity/Identity-create-storedProcedures.sql ./ 
COPY ./Database/Calculator/Calculator-create-db.sql ./
COPY ./Database/Calculator/Calculator-create-tables.sql ./
COPY ./Database/Calculator/Calculator-create-storedProcedures.sql ./ 
COPY ./Database/Background/Background-create-db.sql ./ 
COPY ./Database/Management/Management-create-db.sql ./
COPY ./Database/Management/Management-create-tables.sql ./
COPY ./Database/Management/Management-create-storedProcedures.sql ./ 
COPY ./Database/General/General-create-db.sql ./ 

COPY Database/entrypoint.sh ./ 
COPY Database/setupsql.sh ./ 

RUN chmod +x setupsql.sh
CMD ["/bin/bash", "entrypoint.sh"]