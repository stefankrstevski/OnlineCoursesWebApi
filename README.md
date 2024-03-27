# OnlineCoursesWebApi

## Overview
This project was developed as a part of the interview process.

## Key Features
- **EF Core**: Leveraged Entity Framework Core for data access.
- **Database Diagram**: The structure of the database is illustrated [here](https://github.com/stefankrstevski/OnlineCoursesWebApi/assets/165183191/60e669f3-4b5b-4ca7-82c1-3340b10e358d).
- **Authentication Service**: Implemented a service to obtain an auth token necessary for accessing courses from an external endpoint.
- **Optimized Token Fetching**: To minimize the frequency of token requests, I utilized a memory cache to store the auth token, refreshing it only when nearing its expiration (20 minutes).
- **Background Service for Course Fetching**: A background service runs daily to fetch course data, ensuring up-to-date information.
- **Repository Pattern with Unit of Work**: This approach optimizes database transactions, facilitating efficient data saving and updating processes.
- **Unit Testing**: Employed xUnit and Moq for robust unit testing, ensuring code reliability.
- **Endpoints**: I have two endpoints, one for saving the Application in the database, and one for getting the courses with the available dates to the Client app.

## Live demo
I've hosted my solution on Azure using App Service and Azure Sql Database. Here is a [link](https://academywebapi.azurewebsites.net/swagger/index.html) of the solution.

### Running the App Locally

If you're wondering how to get the app up and running on your local machine, follow these steps:

1. **Provide Environment Variables**:
   - You will need to set the following environment variables:
     - `ApiSettings:ApiKey`: Your API key
     - `ApiSettings:ApiUrl`: The API URL (e.g., `https://external-api.azurewebsites.net/api`)
     - `ConnectionStrings:DefaultConnection`: Connection string for the database (e.g., `Server=(localdb)\\MSSQLLocalDB; Database=AcademyDatabase; Trusted_Connection=True; Trust Server Certificate=True;`)

2. **Examples**:
   ```json
   "ApiSettings": {
     "ApiKey": "api_key",
     "ApiUrl": "https://external-api.azurewebsites.net/api"
   }
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB; Database=AcademyDatabase; Trusted_Connection=True; Trust Server Certificate=True;"
   }
