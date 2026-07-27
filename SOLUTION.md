# SOLUTION.md

## Overview
This solution implements a full stack **Product Catalog Management System** with:
- **FRONTEND** : Angular 20.3
- **BACKEND** : ASP.NET MVC + Entity Framework 6
- **DATABASE** : MySQL (5.7.18)

---

## Design Decisions

### [Service-Repository-Controller Architecture]
- **Why**: Enforces seperation of concerns, improves maintainabilitty, and makes unit testing simple if implemented.
- **Trade-offs**: Slight more boilerplate code compared to direct service calls. But cleaner and easier to structure and understand boundaries.

### [Entity Framework 6 with MySql]
- **Why**: Well supported mature ORM that integrates well with ASP.NET MVC. Not to mention familiarity and cost effectiveness.
- **Trade-off**: Slight issues with collation sometimes. Atleast until the db and tables are in place.

### [Angular @for Syntax]
- **Why**: Cleaner than angular's deprecated `*ngFor` inline attribute style syntax, easy to read and modify.
- **Trade-off**: Had to fimiliarise myself with the new syntax since it complained that old syntax is deprecated.

---

## Trade‑offs Made

- **EF6 vs EF Core**: Chose EF6 for stability with MVC, even though EF Core offers better MySQL support. Accepted extra migration troubleshooting.
- **MySQL vs SQL Server**: MySQL chosen for cost and portability, enven though SQL Server offering smoother integration with .NET .
- **Unit Testing Scope**: Focused on critical paths (form submission, service calls, event emission). Did not exhaustively test every UI detail to balance time vs coverage.
- **Environment Config**: Centralized backend path in Angular `environment.ts` files. Trade‑off: requires manual updates per environment, but avoids hard‑coding URLs.

---

## Requirements coverage

### Backend Core Design Decisions
- **Repository Pattern (Generic Repository<T>)**  
	Implemented to abstract data access and promote testability. One repository uses **in‑memory collections** (`List`, `Dictionary`) to meet the non‑EF requirement.

- **LINQ Extension Methods for Filtering**  
	Custom LINQ extensions were created to handle product filtering, pagination, search logic efficiently.
  
- **Record Types for DTOs**  
	Used for lightweight, immutable data transfer objects, improving clarity and reducing boilerplate.
  
- **Pattern Matching & Nullable Reference Types**  
	Applied throughout controllers and validation logic to ensure safer null handling and cleaner request validation.
  
- **Custom Middleware**  
	Built to handle request logging and error responses without relying onpre-built framework helpers.

- **Caching Layer**  
	Implemented using a `Dictionary<TKey, TValue>` for search results to reduce repeated calls by storing previous requests.

- **Hierarchical Category Tree**  
	Built a category tree with parent‑child links to support nested filtering.
	
- **Custom JSON Serialization**
	Implemented on one endpoint to control output.

---

### Core C# Challenge
The **ProductSearchEngine** was implemented using only core C# features:
- Efficient **in‑memory search algorithm** for products.  
- **Fuzzy matching** (e.g., `"lptop"` matches `"laptop"`).  
- Weighted scoring across multiple fields.  
- Generic design for reuse with other entities.

---

### Outcome
The design balances performance and flexibility.
Extra work was needed for custom middleware and caching, but these choices show solid implementation use of core C# and ASP.NET Core.