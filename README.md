# Banking

## Setup Instructions

**Clone the Repository:**

```
git clone https://github.com/mra-ezera/Banking.git
```

**Enter the project folder:**

```
cd Banking
```

**Create MySQL container and run database migrations:**

```
docker compose up -d --build
```

**Run the Application:**

```
dotnet run --launch-profile https
```

**Access the Swagger UI**

Open your browser and navigate to [https://localhost:7248/swagger](https://localhost:7248/swagger)

## Additional Information

- The project targets .NET 8.
- Ensure you have Docker installed and running on your machine.