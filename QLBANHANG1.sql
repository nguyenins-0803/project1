DROP DATABASE QLBANHANG1;
ALTER DATABASE QLBANHANG1 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE QLBANHANG1;


-- Create database
CREATE DATABASE QLBANHANG1;
GO 

-- Use the created database
USE QLBANHANG1;

-- Create Customers table
CREATE TABLE Customers (
    CustomerID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(15),
    Address VARCHAR(MAX),
    City VARCHAR(100),
    Country VARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Categories table
CREATE TABLE Categories (
    CategoryID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    CategoryName VARCHAR(100) NOT NULL,
    Description VARCHAR(MAX)  -- Use VARCHAR instead of TEXT
);

-- Create Products table
CREATE TABLE Products (
    ProductID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    ProductName VARCHAR(200) NOT NULL,
    Description VARCHAR(MAX),
    Price DECIMAL(15, 0) NOT NULL,  -- Use DECIMAL for accurate currency values
    StockQuantity INT DEFAULT 0,
    CategoryID VARCHAR(50),
    ImageUrl VARCHAR(255),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Create Carts table
CREATE TABLE Carts (
    CartID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    CustomerID VARCHAR(50),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create CartItems table
CREATE TABLE CartItems (
    CartItemID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    CartID VARCHAR(50),
    ProductID VARCHAR(50),
    Quantity INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CartID) REFERENCES Carts(CartID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Create Orders table
CREATE TABLE Orders (
    OrderID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    CustomerID VARCHAR(50),
    OrderDate DATETIME2 DEFAULT GETDATE(),
    TotalAmount DECIMAL(15, 0) NOT NULL,  -- Use DECIMAL for accurate currency values
    Status VARCHAR(50) DEFAULT 'Pending',
    ShippingAddress VARCHAR(MAX),
    City VARCHAR(100),
    Country VARCHAR(100),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create OrderItems table
CREATE TABLE OrderItems (
    OrderItemID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    OrderID VARCHAR(50),
    ProductID VARCHAR(50),
    Quantity INT NOT NULL,
    Price DECIMAL(15, 0) NOT NULL,  -- Use DECIMAL for accurate currency values
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Create Reviews table
CREATE TABLE Reviews (
    ReviewID VARCHAR(50) PRIMARY KEY,  -- Use VARCHAR for custom IDs
    ProductID VARCHAR(50),
    CustomerID VARCHAR(50),
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    Comment VARCHAR(MAX),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
INSERT INTO Customers (CustomerID, FullName, Email, Password, PhoneNumber, Address, City, Country, CreatedAt)
VALUES 
('CUST001', 'Nguyễn Thị Châm', 'Cham@gmail.com', 'password123', '0000000000', '72 Trần Đại Nghĩa', 'Hoài Đức', 'Hà Nội', GETDATE()),
('CUST002', 'Phạm Diễm Quỳnh', 'Quynh@gmail.com', 'password123', '0000000001', '72 Trần Đại Nghĩa', 'Hà Đông', 'Hà Nội', GETDATE()),
('CUST003', 'Alice Johnson', 'alice.johnson@example.com', 'password789', '0000000002', '789 Pine Road', 'Chicago', 'USA', GETDATE());

INSERT INTO Categories (CategoryID, CategoryName, Description)
VALUES 
('CAT001', 'Túi da nữ', 'Các mẫu mã túi xách và ví da thời trang cho nữ'),
('CAT002', 'Túi da nam', 'Các mẫu mã túi xách và ví da thời trang cho nam'),
('CAT003', 'Phụ kiện da', 'Phụ kiện da hiện đại sang trọng, lịch sự dễ sử dụng');

INSERT INTO Products (ProductID, ProductName, Description, Price, StockQuantity, CategoryID, ImageUrl, CreatedAt)
VALUES 
('PROD001', 'Túi xách thời trang TX00001', 'Túi da màu trắng hiệu verchi', 16990000, 50, 'CAT001', 'TX00001.jpg', GETDATE()),
('PROD002', 'Túi đựng Laptop da bò TD00001', 'Túi đựng laptop với đủ mọi size, đơn giản tiện lợi', 28990000, 30, 'CAT003', 'PK00001.jpg', GETDATE()),
('PROD003', 'Túi cầm tay da bò TD00002', 'Túi da màu đen hiệu verchi', 499000, 100, 'CAT002', 'TD00002.jpg', GETDATE()),
('PROD004', 'Túi xách thời trang TX00002', 'Túi xách size trung dễ phối đồ nhiều màu sắc', 349000, 200, 'CAT001', 'TX00002.jpg', GETDATE());

-- Manage cart data
INSERT INTO Carts (CartID, CustomerID, CreatedAt)
VALUES 
('CART001', 'CUST001', GETDATE()),
('CART002', 'CUST002', GETDATE());

INSERT INTO CartItems (CartItemID, CartID, ProductID, Quantity, CreatedAt)
VALUES 
('ITEM001', 'CART001', 'PROD001', 2, GETDATE()),
('ITEM002', 'CART001', 'PROD003', 1, GETDATE()),
('ITEM003', 'CART002', 'PROD002', 1, GETDATE()),
('ITEM004', 'CART002', 'PROD004', 3, GETDATE());

SELECT * FROM CartItems
SELECT * FROM Customers
SELECT * FROM Categories
SELECT * FROM Products

SELECT * FROM Products
