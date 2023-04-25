# TeamTitanMicroservices 

TeamTitan is system to management employees. The project is still developed, but the general scope of functionality is visible. TeamTitan has been created for companies that have problems with employee management and goals to automate processes related to employees.

If you found this implementation helpful or used it in your projects, I am very pleased ⭐️

## How can you run the project?

**First step**
You need to go to the docker files location and run the command:

*docker-compose -f docker-compose.yml -f docker-compose.app.yml up*

or if you want to run locally then you can run only *docker-compose -f docker-compose.yml up* command 

**Second step**
1. with docker 
Open the browser and go to:
http://localhost:8013/swagger/index.html - identity service
http://localhost:8011/swagger/index.html - management service
http://localhost:8012/swagger/index.html - calculator service

2. locally
 https://localhost:7147/swagger/index.html - identity service
 https://localhost:7205/swagger/index.html - management service
 https://localhost:7098/swagger/index.html - calculator service

or you can use celot with postman ofc:
docker:
http://localhost:8010/

locally:
https://localhost:5001/

## Current short overview

![image](https://user-images.githubusercontent.com/81367371/234418288-fd850549-6282-4743-ae4b-e39d5d5857c8.png)



## Architecture and technologies


**Architecture**

<ul>
<li>Microservice Architecture</li>
<li>My own EventStore</li>
<li>Command Query Responsibility Segregation (CQRS)</li>
<li>Domain Driven Design (DDD)</li>
<li>Each service has Architecture</li>
</ul>


**Database**

<ul>
<li>SQL Server</li>
<li>MongoDB</li>
</ul>
 
**Technologies**

<ul>
<li>Docker</li>
<li>Smtp4dev for docker and local tests</li>
<li>RabbitMQ</li>
<li>.NET 6.0</li>
<li>Dapper</li>
<li>T-SQL</li>
<li>Hangfire for background jobs</li>
<li>Polly</li>
<li>Swagger</li>
<li>Serilog with seq</li>
</ul>

**Tests*
<ul>
<li>Integration tests</li>
<li>Unit tests</li>
</ul>

![image](https://user-images.githubusercontent.com/81367371/234428091-14395b6b-a846-411f-b359-26cf0adfd3ed.png)


Structure project:
![image](https://user-images.githubusercontent.com/81367371/234428181-d2c85916-c4bd-4476-9c58-4c49e47aaf24.png)
![image](https://user-images.githubusercontent.com/81367371/234428239-8cdf0b52-1dca-4722-8e97-f5a5f4c8cc16.png)
![image](https://user-images.githubusercontent.com/81367371/234428343-e22079a3-2aa5-4863-95ce-f956e5f6eae2.png)
![image](https://user-images.githubusercontent.com/81367371/234428372-f093f60e-3cbd-4651-9490-2df25debfffc.png)

