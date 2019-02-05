DROP TABLE dbo.Dependents
GO

DROP TABLE dbo.Employees
GO

CREATE TABLE dbo.Employees
(
    EmployeeId     int PRIMARY KEY NOT NULL,
    FirstName      nvarchar(50) NOT NULL,
    LastName       nvarchar(50) NOT NULL,
    BiweeklySalary int NOT NULL,
    Cost           int NOT NULL,
    DiscountFactor int NOT NULL
)
GO

CREATE TABLE dbo.Dependents
(
    DependentId    int PRIMARY KEY IDENTITY (1, 1),
    FirstName      nvarchar(50) NOT NULL,
    LastName       nvarchar(50) NOT NULL,
    EmployeeId     int NOT NULL,
    Cost           int NOT NULL,
    DiscountFactor int NOT NULL
    UNIQUE (FirstName, LastName, EmployeeId)
    CONSTRAINT FK_Dependents_Employees FOREIGN KEY (EmployeeId)
    REFERENCES dbo.Employees (EmployeeId)
    ON DELETE CASCADE
    ON UPDATE CASCADE
)
GO

INSERT INTO dbo.Employees(EmployeeId, FirstName, LastName, BiweeklySalary, Cost, DiscountFactor)
VALUES
(001, 'Thomas', 'Keller', 2000, 1000, 100),
(002, 'James', 'Richter', 2000, 1000, 100),
(003, 'Mark', 'Hall', 2000, 1000, 100),
(004, 'Darren', 'Graves', 2000, 1000, 100),
(005, 'Alex', 'Rogers', 2000, 1000, 90)
GO

INSERT INTO dbo.Dependents(FirstName, LastName, EmployeeId, Cost, DiscountFactor)
VALUES
('Olivia', 'Keller', 001, 500, 100),
('Kayla', 'Keller', 001, 500, 100),
('Erin', 'Richter', 002, 500, 100),
('Alice', 'Richter', 002, 500, 90),
('Ellen', 'Richter', 002, 500, 100),
('Liam', 'Hall', 003, 500, 100),
('Delaney', 'Graves', 004, 500, 100),
('Alexis', 'Graves', 004, 500, 90),
('Malia', 'Rogers', 005, 500, 100),
('Rhiannon', 'Rogers', 005, 500, 100)
GO
