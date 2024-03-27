# OnlineCoursesWebApi

## Overview
This project serves as a backend Web API for a client application, designed to interact seamlessly with third-party APIs for authentication purposes and to fetch necessary data for frontend consumption. Its core functionality is centered around mapping received data into a format suitable for the client app and providing two essential endpoints. The first endpoint is dedicated to retrieving course information along with the dates they are available, while the second endpoint focuses on handling application data submissions to the database. This setup aims to simplify the data integration process, ensuring that the client application has timely access to the required information for an optimal user experience.
Client app design [here](https://github.com/stefankrstevski/OnlineCoursesWebApi/assets/165183191/e4c81ce8-5b3c-4df5-a97e-32445be47fbe).

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
## Data Integration
The API consumes data from external sources, specifically designed to fetch course information. The data structure obtained from the external API is as follows:
```json
{
  "next_page_link": "<https://external-api.azurewebsites.net/api/courses?offset=10&limit=10>; rel=\"next\"",
  "total_count": 34,
  "max_limit": 10,
  "data": [
    {
      "id": 3,
      "course_name": "Web Development",
      "date": "2023-01-25T00:00:00",
      "is_active": true
    },
    {
      "id": 1,
      "course_name": "Introduction to Computer Science",
      "date": "2023-04-23T00:00:00",
      "is_active": true
    },
    // Additional courses omitted for brevity
  ]
}
