# Task Manager API

Welcome to the Task Manager API, an open-source project designed to manage your daily tasks efficiently. This API allows users to perform CRUD operations on tasks, retrieve task lists, and update task completion status. Additionally, it provides authentication endpoints for user registration, login, logout, and user deletion.

## Prerequisites
Before getting started, ensure you have the following installed on your system:
- [Visual Studio](https://visualstudio.microsoft.com/) for IDE
- [.NET SDK](https://dotnet.microsoft.com/download) for building and running the application
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for the database

## Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/AlexanderNovoselski/TaskManager.git
    ```

2. Open the project in Visual Studio.

3. Configure the Database:
    - Navigate to `TaskManager/appsettings.json`.
    - Locate the `DefaultConnection` string under `ConnectionStrings`.
    - Replace the connection string with your own database connection details.
  
4. Run the following commands in the Package Manager Console to apply migrations and update the database:
    ```bash
    add-migration {InitMigration}
    update-database
    ```

## Usage

### Authentication
This API uses token-based authentication. Users need to include a valid JWT token in the Authorization header of their requests. Make sure to obtain a token by authenticating through the `/api/auth/login` and to use this token on each request as a Header: Bearer {token}


#### Endpoints

- **Local Development:**
    ```bash
    https://localhost:{port}
    ```
- **Azure Hosted API and Database:**
- Access your API and database hosted on Azure for testing purposes using: 
    ```bash
    https://taskmanager20240106194805.azurewebsites.net/
    ```

#### Authentication Endpoints

- **POST /api/Account/register**
  - Register a new user.
  - Parameters: `username`, `email`, `password`
  - Example:
    ```bash
    POST /api/auth/register
    ```

- **POST /api/Account/login**
  - Log in a user.
  - Parameters: `username`, `password`, `rememberMe` (optional)
  - Example:
    ```bash
    POST /api/auth/login
    ```
  - JSON Parameters:
    - `Token` (string): The unique token to access each points of the API.

- **POST /api/Account/logout**
  - Log out the currently authenticated user.
  - Example:
    ```bash
    POST /api/auth/logout
    ```

- **DELETE /api/Account/delete**
  - Delete the currently authenticated user.
  - Example:
    ```bash
    DELETE /api/auth/delete
    ```

### Task Endpoints

- **Each endpoint is authorized and you must be logged in before you use it**

- **GET /api/Task/Search**
    - Retrieve tasks based on search criteria.
    - Parameters: `searchCriteria` (query string)
    - Example:
      ```bash
      GET /api/Task/Search?searchCriteria=important
      ```

- **GET /api/Task/GetAll**
    - Retrieve all tasks paginated.
    - Parameters: `pageNumber` (query string, optional)
    - Example:
      ```bash
      GET /api/Task/GetAll?pageNumber=1
      ```

- **GET /api/Task/GetAllIncompleted**
    - Retrieve all non-completed tasks by a specified due date.
    - Parameters: `DueDate` (query string, optional, default value = today)
    - Example:
      ```bash
      GET /api/Task/GetAllIncompleted?DueDate=2024-01-22
      ```

- **GET /api/Task/GetTask**
    - Retrieve a specific task by ID.
    - Example:
      ```bash
      GET /api/Task/GetTask
      ```
    - JSON Parameters:
      - `Id` (string): The unique identifier of the task.

- **PUT /api/Task/Update**
    - Update an existing task.
    - Example:
      ```bash
      PUT /api/Task/Update
      ```
    - JSON Parameters:
      - `Id` (string): The unique identifier of the task.
      - `Title` (string): The updated title of the task.
      - `Description` (string): The updated description of the task.
      - `DueDate` (string): The updated due date of the task (format: "yyyy-MM-dd").
      - `IsCompleted` (boolean): The updated completion status of the task.

- **POST /api/Task/Create**
    - Create a new task.
    - Example:
      ```bash
      POST /api/Task/Create
      ```
    - JSON Parameters:
      - `Title` (string): The title of the new task.
      - `Description` (string): The description of the new task.
      - `DueDate` (string): The due date of the new task (format: "yyyy-MM-dd").
      - `IsCompleted` (boolean): The completion status of the new task.

- **DELETE /api/Task/Delete**
    - Delete a task by ID.
    - Example:
      ```bash
      DELETE /api/Task/Delete
      ```
    - JSON Parameters:
      - `Id` (string): The unique identifier of the task.

- **PATCH /api/Task/UpdateCompletion**
    - Update the completion status of a task.
    - Example:
      ```bash
      PATCH /api/Task/UpdateCompletion
      ```
    - JSON Parameters:
      - `Id` (string): The unique identifier of the task.
      - `IsCompleted` (boolean): The updated completion status of the task.

- **GET /api/Task/GetCount**
    - Retrieve the total count of tasks.
    - Example:
      ```bash
      GET /api/Task/GetCount
      ```
    - JSON Parameters:
      - None

## Running the API (Self-Hosted)
When self-hosted, you can access the API at `https://localhost:{your_port}/api/Task/...`. Make sure to replace `{your_port}` with the port number you specify.

## Contributing
Feel free to contribute to the development of this API. Fork the repository, make your changes, and submit a pull request.

## License
This Task Manager API is licensed under the [MIT License](https://github.com/AlexanderNovoselski/TaskManager/blob/main/LICENSE).

For any issues or questions, please refer to the [GitHub repository](https://github.com/AlexanderNovoselski/TaskManager) or contact the project owner.

Happy task managing!
