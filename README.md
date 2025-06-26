# DevSpeedMindGame 
About the game:
This is a backend-only math game where players answer random equations by calling APIs (no UI needed). I built it using ASP.NET Core and Entity Framework Core, and everything works directly through Swagger.
-It tracks the player’s name, difficulty level, questions, answers, time taken, and final results * all through clean API calls.*

# Tech Used
.NET 8 – ASP.NET Core Web API
Entity Framework Core + SQL Server
Swagger for API testing
3-Tier Architecture:
    Presentation Layer: Controllers (API endpoints)
    Business Logic Layer: Services folder (handles game logic)
    Data Access Layer: DbContext + Models (manages data storage)

# Setup Instructions
1. Clone the repo ( git clone 'https://github.com/ToqaKhashashneh/DevSpeedMindGame.git')
2. Setup the sql by excuting these comands on Microsoft SQL Management Studio:
   CREATE DATABASE DevMindSpeedGame;
      USE DevMindSpeedGame;
   
      CREATE TABLE Sessions (
          ID INT IDENTITY(1,1) PRIMARY KEY,
          Name NVARCHAR(100) NOT NULL,
          Difficulty INT NOT NULL CHECK (Difficulty IN (1, 2, 3, 4)),
          StartTime DATETIME,
          EndTime DATETIME,
          TotalTimeSpentSeconds FLOAT NULL  
      );
      
      CREATE TABLE Questions (
          ID INT IDENTITY(1,1) PRIMARY KEY,
          SessionID INT NOT NULL FOREIGN KEY REFERENCES Sessions(ID),
          QuestionContent NVARCHAR(MAX) NOT NULL,
          CorrectAnswer FLOAT NOT NULL,
          SubmittedAnswer FLOAT NULL,
          TimeStarted DATETIME,
          TimeSubmitted DATETIME,
          TimeTakenSeconds FLOAT NULL,      
          Score FLOAT NULL                  
      );

3. Update DB Connection in appsettings.json (Put your server name)
   "ConnectionStrings": {
  "YourConnectionString": "Server=.;Database=DevMindSpeedDb;Trusted_Connection=True;"
}

 # Run the app and enjoy the game :)!

# Bonus Touches:
* Clean 3-tier separation
* Clean DTOs for responses
* Null values removed from response
* Full history and performance tracking
* Code separation into Services, DTOs, Models, and Controllers
  

