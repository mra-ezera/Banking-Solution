# Banking

## Overview
This API provides endpoints to manage bank accounts. The API is secured and requires authentication for access. The endpoints support pagination for retrieving large sets of data.

## Setup Instructions

Ensure you have Docker installed and running on your machine.

**Clone the Repository:**

```
git clone https://github.com/mra-ezera/Banking.git
```

**Enter the project folder:**

```
cd Banking/Banking
```

**Create MySQL container and run database migrations:**

```
docker compose up -d --build
```

**Run the Application:**

```
dotnet run --launch-profile https
```

### Accessing Swagger UI

Open your browser and navigate to [https://localhost:7248/swagger](https://localhost:7248/swagger)

## Authentication
The API uses token-based authentication. To access the endpoints, you need to include a valid JWT token in the
`Authorization` header of your requests.

## Pagination
The API supports pagination for endpoints that return collections of data. You can control the pagination using the
following query parameters:

- `PageNumber`: The page number to retrieve (default is 1).
- `PageSize`: The number of items per page (default is 10, maximum is 50).

### Example

```
GET /api/account?pageNumber=1&pageSize=10 HTTP/1.1
Host: example.com
Authorization: Bearer your_jwt_token

```
