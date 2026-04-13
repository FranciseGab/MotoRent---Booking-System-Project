Motor dev system!

Features:
Login
Renting
Transaction 

Simple motor renting system! demonstrating the backend asp dotnet with tests

System Flow:
User logs in --> redirect to Renting --> Select a motor with how many days to rent --> Payment 

Notes:
System only have users, no admins
In renting always payment first.
Renting only has rent now.
Transaction viewing : Just view the rent made

Language & tools:
c# dotnet
swagger 
cors
sqlite


Folder sturcture:
Backend/
    src/
        db/
            models/ - table scheme only
                CustomerModel.cs
            seeds/ - seed datas (user, motor)
                CustomerSeed.cs
                MotorSeed.cs
            dbInit.cs 
            db.sqlite

        controllers/
            CustomerController.cs (Http, Uses service to process and response)

        services/
            CustomerService.cs (does queries(uses db) and business logic )
            
    Program.cs
