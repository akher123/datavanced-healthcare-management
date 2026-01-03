# datavanced-healthcare-management
Senior software engineer - 2nd round (assignment)
# Datavanced Healthcare Management System

I was trired to develop a modern healthcare management system built using **.NET Core Web API**, **Entity Framework Core**, and **JWT-based authentication**. The system supports role-based access control, audit logging, and efficient CRUD and search operations for **Patients**, **Caregivers**, and **Offices**.

---

## Table of Contents

1. [High-Level Architecture](#high-level-architecture)  
2. [Architecture Layers](#architecture-layers)  
3. [Database Relationships](#database-relationships)  
4. [High-Level Flow](#high-level-flow)  
5. [Benefits](#benefits)  

---

## 1.High-Level Architecture

The system is designed using a **modern N-Layer architecture**, with separation of concerns across **API**, **Service**, **Repository**, and **Shared Kernel** layers.  

**Key technologies:**  
- **.NET Core Web API** – RESTful endpoints  
- **Entity Framework Core** – Data access  
- **JWT Authentication** – Secure, role-based access  
- **Audit Logging** – Track CRUD and search activity  

---

## Architecture Layers

### API Layer (Presentation Layer)
- Exposes **RESTful endpoints** for client applications (Angular Web UI, Mobile).  
- Handles **HTTP requests**, request validation, and response formatting.  
- Validates **JWT tokens** and enforces **role-based access**.  
- Supports **pagination**, **sorting**, and **search parameters**.

### Service Layer (Business Layer)
- Implements business rules, e.g., **patient-caregiver assignment**.  
- Coordinates multiple repositories for **complex operations** (e.g., many-to-many relationships).  
- Maps **entities to DTOs** for API responses.  
- Handles **audit logging** for search activity.

### Repository / Data Access Layer (DAL)
- Uses **EF Core (Code-First)** or optionally **Dapper** for high-performance queries.  
- Implements CRUD operations and optimized queries for **search, pagination, and sorting**.  
- Protects against **SQL injection** via parameterized queries.  
- Manages **many-to-many relationships** via junction tables (`PatientCaregiver`).

### Shared Kernel / Common Layer
- Contains reusable **DTOs, enums, constants, and helper classes**.  
- Includes **pagination and search models**, JWT helpers, and **audit logging utilities**.

---

## Database Relationships

- **Office → Patients:** One-to-Many  
- **Office → Caregivers:** One-to-Many  
- **Patient → Caregivers:** Many-to-Many (via junction table `PatientCaregiver`)  
- **Caregiver → Office:** Many-to-One  

---

## High-Level Flow

1. **Client Request:** User searches patients or caregivers via Angular UI.  
2. **API Layer:** Receives request, validates JWT, forwards search parameters to Service Layer.  
3. **Service Layer:** Applies business logic, handles role-based access, logs audit events, calls Repository.  
4. **Repository Layer:** Executes optimized database queries with filtering, pagination, and sorting.  
5. **API Response:** Returns results along with **total record count** to support pagination in the UI.  

---

## Benefits

- **Separation of Concerns:** Each layer has a **single responsibility**.  
- **Scalable & Maintainable:** Easily extendable with new features or entities.  
- **Secure:** JWT authentication and **role-based access control**.  
- **Auditable:** Search activity and CRUD operations are **logged**.  
- **Performance-Oriented:** Optimized queries, pagination, and sorting ensure **response times < 300ms**.  

##2 Search Optimization

To ensure fast and efficient search across **Patients**, **Caregivers**, and **Offices**, the system applies several optimization strategies while supporting **pagination**, **sorting**, and **keyword search**.

---

### Implementation Details

- Uses **EF Core** with `Include` to load related entities (`Office` and `PatientCaregivers → Caregiver`).  
- Supports **keyword search** across multiple fields: `FirstName`, `LastName`, `Email`, `Phone`, `OfficeName`, and caregiver names.  
- Applies **server-side pagination** and sorting to minimize database load.  
- Maps entities to **DTOs** to reduce response payload and improve performance.  
- Uses **output caching** for frequently accessed queries.

**Example EF Core Query:**

```csharp
var patientsQuery = _context.Patients
    .Include(p => p.Office)
    .Include(p => p.PatientCaregivers)
        .ThenInclude(pc => pc.Caregiver)
    .AsQueryable();

if (!string.IsNullOrWhiteSpace(query.Keyword))
{
    var keyword = $"%{query.Keyword.Trim()}%";
    patientsQuery = patientsQuery.Where(p =>
        EF.Functions.Like(p.FirstName, keyword) ||
        EF.Functions.Like(p.LastName, keyword) ||
        EF.Functions.Like(p.Email, keyword) ||
        EF.Functions.Like(p.Phone, keyword) ||
        EF.Functions.Like(p.Office.OfficeName, keyword) ||
        p.PatientCaregivers.Any(pc =>
            EF.Functions.Like(pc.Caregiver.FirstName, keyword) ||
            EF.Functions.Like(pc.Caregiver.LastName, keyword))
    );
}
```
var total = await patientsQuery.CountAsync(cancellationToken);
var patients = await patientsQuery
    .Skip((query.PageIndex - 1) * query.PageSize)
    .Take(query.PageSize)
    .ToListAsync(cancellationToken);


## Heare are appkied Best Practices for Faster Search 

- **Database Indexing**  
  Add indexes on frequently searched columns such as `FirstName`, `LastName`, `Email`, `Phone`, and `OfficeName`.

- **Full-Text Search**  
  For large datasets, use full-text indexes instead of `LIKE` queries to achieve better search performance.

- **DTO Projection**  
  Select only required fields when querying data to reduce memory usage and improve response time.

- **AsNoTracking**  
  Use `AsNoTracking()` for read-only queries to avoid Entity Framework Core change tracking overhead.

- **Server-Side Pagination & Sorting**  
  Apply `Skip`, `Take`, and `OrderBy` operations at the database level instead of in-memory processing.

- **Caching**  
  Use `MemoryCache` or `Redis` for frequently accessed queries such as patient or caregiver lists.

- **Avoid N+1 Queries**  
  Use `Include` or projection-based queries to efficiently load related data in a single database call.

## 3.Authorization and Auditing (Custom Middleware & Interceptors)

The system implements secure **token-based authorization** and **comprehensive auditing** to ensure data integrity, compliance, and full traceability of user actions.

---

## Authorization Strategy

Authorization is implemented using **JWT-based Role-Based Access Control (RBAC)**.

---

### JWT Authentication

- Users authenticate using **username and password**.
- On successful login, a **JWT token** is issued containing:
  - User identity
  - Role claims
  - Token expiration
- All protected API endpoints require a **valid JWT token**.

---

### JWT Validation

- Token validation is centralized using a **custom extension method**.
- Validates:
  - Issuer
  - Audience
  - Token lifetime
  - Signing key
- Prevents expired or invalid tokens from accessing the API.

**JWT Configuration Example:**

```csharp
services.AddJwtAuthentication(jwtSettings);
```
## 4.Caching Strategy (In-Memory & Redis)

To achieve fast response times (**< 300ms**) and minimize database load, the system uses a **hybrid caching strategy** combining **In-Memory caching** and **Redis distributed caching**. This approach balances **speed**, **scalability**, and **data consistency**.

---

## Caching Objectives

- Reduce repetitive database queries
- Improve performance for search and read-heavy endpoints
- Support scalability across multiple API instances
- Maintain data consistency for healthcare records

---

## In-Memory Caching

In-Memory caching is used for **single-instance**, high-speed access to frequently requested data.

### Use Cases
- Reference data (e.g., Offices, Roles)
- Small, frequently accessed lookup data
- Short-lived cache entries

### Implementation
- Uses `IMemoryCache`
- Cache expiration controlled using absolute and sliding expiration
- Cached per application instance

### Benefits
- Extremely fast access
- Simple to implement
- Ideal for low-latency, non-distributed scenarios

---

## Redis Distributed Caching

Redis is used for **distributed caching** to support scalability across multiple API instances.

### Use Cases
- Patient and caregiver search results
- Frequently accessed read-heavy endpoints
- Shared cache across multiple servers

### Implementation
- Uses `IDistributedCache` with Redis
- Serialized cache entries (JSON)
- Centralized cache store for all API instances

### Benefits
- Shared cache across multiple instances
- High availability and scalability
- Prevents cache inconsistency in load-balanced environments

---

## Cache Invalidation Strategy

To maintain data consistency, the following cache invalidation rules are applied:

- Cache entries are invalidated on:
  - Create
  - Update
  - Delete operations
- Time-based expiration as a fallback mechanism
- Selective cache key removal to avoid full cache flush

---

## Caching Best Practices

- Cache only **read-heavy** and **frequently accessed** data
- Avoid caching sensitive or frequently changing data
- Use **short TTLs** for healthcare-related records
- Apply caching at the **service layer**
- Combine caching with pagination and filtering
- Monitor cache hit/miss ratios

---

## Benefits

- Ensures API response time **< 300ms**
- Significantly reduces database load
- Improves scalability and reliability
- Maintains consistency across distributed systems
- Optimized for healthcare compliance and performance




