# datavanced-healthcare-management
Senior software engineer - 2nd round (assignment)
# Datavanced Healthcare Management System

A modern healthcare management system built using **.NET Core Web API**, **Entity Framework Core**, and **JWT-based authentication**. The system supports role-based access control, audit logging, and efficient CRUD and search operations for **Patients**, **Caregivers**, and **Offices**.

---

## Table of Contents

1. [High-Level Architecture](#high-level-architecture)  
2. [Architecture Layers](#architecture-layers)  
3. [Database Relationships](#database-relationships)  
4. [High-Level Flow](#high-level-flow)  
5. [Benefits](#benefits)  

---

## High-Level Architecture

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
