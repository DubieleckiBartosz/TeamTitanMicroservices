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

![image](https://user-images.githubusercontent.com/81367371/234428763-5d403ea0-5547-454e-bee7-4d6667ed7a38.png)


Endpoints:

**Identity:**
<li>api/account/new-owner</li> 
<li>api/account/assignUserCodes</li>
<li>api/account/clearUserCodes</li>
<li>api/account/initUserOrganization</li>
<li>api/account/registerNewUser</li>
<li>api/account/confirmAccountByEmail</li>
<li>api/account/loginUser</li>
<li>api/account/refreshToken</li>
<li>api/account/forgotPassword</li>
<li>api/account/reset-password</li>
<li>api/account/revokeToken</li>
<li>api/account/addNewRoleToUser</li>
<li>api/account/getCurrentUserInfo</li>



/Account 


GET AddOwnerRoleToUser: ALL

PUT AssignUserCodes: User 

PUT ClearUserCodes: Admin,Owner,Manager,Employee

POST InitUserOrganization: Admin,Owner,Manager 

POST RegisterNewUser: ALL

POST ConfirmAccountByEmail: ALL

POST LoginUser: ALL

POST RefreshToken: ALL

POST ForgotPassword: ALL

POST ResetPassword: ALL 

POST RevokeToken: User 

PUT AddNewRoleToUser: ALL (Temporary for tests)

GET GetCurrentUserInfo: User 


**Management:**

<ul>
<li>api/company/getCompanyByCode</li>  
<li>api/company/completeCompanyData</li>  
<li>api/company/updateCommunicationData</li>  
<li>api/company/initCompany</li>  
<li>api/department/getDepartmentsByCompanyId/{companyId}</li>  
<li>api/department/createDepartment</li>  
<li>api/employee/updateContactData</li>  
<li>api/employee/updateAddressData</li>  
<li>api/employee/updateLeader</li>  
<li>api/employee/cancelDayOffRequest</li>  
<li>api/employee/considerDayOffRequest</li>  
<li>api/employee/createEmployee</li>  
<li>api/employee/newDayOffRequest</li> 
<li>Api/employee/newEmployeeContract</li> 
</ul>


Permissions:


/Company 

PUT UpdateCommunication: Admin,Owner 

PUT CompleteCompanyData: Admin,Owner 

POST InitCompany: User 

POST GetCompanyByCode: Admin,Owner,Manager 

 

/Department 

GET GetDepartmentsByCompanyId: Admin,Owner,Manager 

POST CreateDepartment: Admin,Owner 

 

/Employee 


PUT UpdateContactData: Admin,Owner,Manager,Employee 

PUT UpdateAddressData: Admin,Owner,Manager,Employee 

PUT CancelDayOffRequest: Admin,Employee 

PUT ConsiderDayOffRequest: Admin,Owner,Manager 

PUT UpdateLeader: Admin,Owner,Manager 

POST CreateEmployee: Admin,Owner,Manager 

POST NewDayOffRequest: Admin,Owner,Manager,Employee 

POST NewEmployeeContract: Admin,Owner,Manager 

 

**Calculator:**

<li>api/account/searchAccounts</li> 
<li>api/account/addBonusToAccount</li>
<li>api/account/addPieceProduct</li>
<li>api/account/addWorkDay</li>
<li>api/account/activateAccount</li>
<li>api/account/cancelBonus</li>
<li>api/account/deactivateAccount</li>
<li>api/product/getProductById/{productId:guid}</li>
<li>api/product/searchProducts</li>
<li>api/product/createNewProduct</li>
<li>api/product/updateAvailability</li>
<li>api/product/updatePriceProduct</li>



Permissions:


/Account 

POST SearchAccounts: Admin,Owner,Manager 

POST AddBonusToAccount: Admin,Owner,Manager 

POST AddPieceProduct: Admin,Owner,Manager 

POST AddWorkDay: Admin,Owner,Manager 

PUT ActivateAccount: Admin,Owner,Manager 

PUT CancelBonus: Admin,Owner,Manager 

PUT DeactivateAccount: Admin,Owner,Manager 

/Product 

POST GetProductById: Admin,Owner,Manager,Employee 

POST SearchProducts: Admin,Owner,Manager,Employee 

POST CreateNewProduct: Admin,Owner,Manager 

PUT UpdateAvailability: Admin,Owner,Manager 

PUT UpdatePriceProduct: Admin,Owner,Manager 




