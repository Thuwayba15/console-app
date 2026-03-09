# Online Shopping Backend System

## What is the Online Shopping Backend System?

The **Online Shopping Backend System** is a C# console-based application that simulates the core functionality of an e-commerce platform. It allows customers to browse products, manage a shopping cart, place orders, and track purchases, while administrators manage products, orders, and reports.

The system demonstrates fundamental backend programming concepts including:

- Object-Oriented Programming (OOP)
- Clean code practices
- LINQ queries
- Exception handling
- Menu-driven console interaction
- Data persistence using JSON files

The goal of this project is to implement a **simple but structured backend system** that models the typical workflow of an online store.

---

# Documentation

## Software Requirement Specification

### Overview

The system supports two primary user roles:

- **Customer**
- **Administrator**

Customers can browse products, add items to a cart, place orders, and review purchased products.

Administrators manage the product catalog, monitor orders, update order statuses, and generate sales reports.

The application runs as a **console-based program** and stores data locally using JSON persistence.

---

## Components and Functional Requirements

### 1. Authentication and Authorization

Users interact with the system through login and registration functionality.

Features include:

- User registration
- User login
- Role-based access (Customer / Administrator)

---

### 2. Product Management

Administrators manage the product catalog.

Features include:

- Add new products
- Update product details
- Delete products
- Restock product inventory
- View all products

---

### 3. Product Browsing and Searching

Customers can view and search available products.

Features include:

- View product catalog
- Search products using LINQ queries
- Filter products by criteria

---

### 4. Shopping Cart Management

Customers can manage their shopping cart.

Features include:

- Add products to cart
- Update product quantities
- View cart contents
- Remove items from cart

---

### 5. Payment and Wallet System

Customers use a simulated wallet to pay for orders.

Features include:

- View wallet balance
- Add funds to wallet
- Pay for orders using wallet balance

---

### 6. Order Management

Customers can place and track orders.

Features include:

- Checkout cart
- Generate order records
- Track order status
- View order history

Administrators can:

- View all orders
- Update order status

---

### 7. Reviews and Feedback

Customers can review products after purchase.

Features include:

- Submit product reviews
- View reviews for products

---

### 8. Reporting and Analytics

Administrators can generate reports about the system.

Features include:

- View low stock products
- Generate sales reports
- View overall order activity

LINQ is used to perform filtering, sorting, and aggregation operations.

---

# Design

## System Architecture

The application follows a **layered console architecture** to separate responsibilities and maintain clean code structure.

```
Program
   ?
   ??? Menus
   ?      Handles console interaction
   ?
   ??? Services
   ?      Business logic
   ?
   ??? Models
   ?      Core domain entities
   ?
   ??? Interfaces
   ?      Contracts for services
   ?
   ??? Data
   ?      Persistence and seed data
   ?
   ??? Helpers
   ?      Utility functions
   ?
   ??? Enums
          Application constants
```

---

## Domain Model

Key entities in the system include:

- User
- Customer
- Administrator
- Product
- Cart
- CartItem
- Order
- OrderItem
- Payment
- Review

These models represent the core business objects used by the system.

---

# Running the Application

## Requirements

Before running the application ensure you have:

- **.NET 8 or later**
- **Visual Studio 2022 / Visual Studio 2025**
- or the **.NET CLI**

---

## Running in Visual Studio

1. Open the solution in Visual Studio.
2. Set **OnlineShoppingSystem** as the startup project.
3. Build the project.
4. Run the application using:

```
F5
```

or

```
Ctrl + F5
```

The console application will start and display the main menu.

---

## Running using the .NET CLI

Navigate to the project folder and run:

```
dotnet run
```

---

## Example Workflow

### Customer Flow

1. Register a new user
2. Login as customer
3. Browse products
4. Add products to cart
5. Add funds to wallet
6. Checkout
7. View order history

### Administrator Flow

1. Login as administrator
2. Add or update products
3. Restock inventory
4. View orders
5. Generate reports

---

# Data Persistence

The system uses **JSON file persistence** instead of a database.

Data stored includes:

- Users
- Products
- Orders
- Reviews

This allows the system to persist data between application runs while keeping the implementation simple.

---

# Development Principles

The project follows clean code practices including:

- Short, readable methods
- Single Responsibility Principle
- Meaningful variable and method naming
- Guard clauses for validation
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple)

Code is organized to ensure maintainability and clarity.

---

# Project Structure

```
OnlineShoppingSystem/
??? Program.cs                              # Entry point, service initialization
?
??? Models/                                 # Domain entities (10 files)
?   ??? User.cs
?   ??? Customer.cs
?   ??? Administrator.cs
?   ??? Product.cs
?   ??? Cart.cs
?   ??? CartItem.cs
?   ??? Order.cs
?   ??? OrderItem.cs
?   ??? Payment.cs
?   ??? Review.cs
?
??? Enums/                                  # Type definitions (2 files)
?   ??? UserRole.cs
?   ??? OrderStatus.cs
?
??? Interfaces/                             # Service contracts (8 files)
?   ??? IAuthService.cs
?   ??? IProductService.cs
?   ??? ICartService.cs
?   ??? IOrderService.cs
?   ??? IPaymentService.cs
?   ??? IReviewService.cs
?   ??? IReportService.cs
?   ??? IPersistenceService.cs
?
??? Services/                               # Business logic (8 files)
?   ??? AuthService.cs
?   ??? ProductService.cs
?   ??? CartService.cs
?   ??? OrderService.cs
?   ??? PaymentService.cs
?   ??? ReviewService.cs
?   ??? ReportService.cs
?   ??? PersistenceService.cs
?
??? Menus/                                  # User interface (3 files)
?   ??? MainMenu.cs
?   ??? CustomerMenu.cs
?   ??? AdministratorMenu.cs
?
??? Helpers/                                # Utilities (6 files)
?   ??? ConsoleHelper.cs
?   ??? InputHelper.cs
?   ??? ValidationHelper.cs
?   ??? ProductDisplayHelper.cs
?   ??? OrderDisplayHelper.cs
?   ??? ReportDisplayHelper.cs
?
??? Data/                                   # Data management (2 files)
?   ??? AppDataStore.cs
?   ??? SeedData.cs
?
??? bin/Debug/net10.0/Data/Storage/         # JSON persistence (6 files)
?   ??? users.json
?   ??? products.json
?   ??? carts.json
?   ??? orders.json
?   ??? payments.json
?   ??? reviews.json
?
??? Documentation/                          # Project documentation
?   ??? PROJECT-FILE-STRUCTURE.md
?   ??? SUBMISSION-COMPLETE.md
?   ??? ADMIN-FEATURES-COMPLETE.md
?   ??? ADMIN-REFACTORING-COMPLETE.md
?   ??? TESTING-ADMIN-FEATURES.md
?   ??? WALLET-PERSISTENCE-FIX.md
?   ??? WALLET-PERSISTENCE-FINAL-FIX.md
?   ??? WALLET-SERIALIZATION-FIX.md
?   ??? FIX-PRODUCT-ID-CONFLICT.md
?
??? OnlineShoppingSystem.csproj             # Project configuration
```

**Total:** 39 source files organized in clean architecture

---

# Author

Developed as part of a **C# backend systems assignment**, demonstrating object-oriented design, LINQ usage, and console application architecture.