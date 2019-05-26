IF OBJECT_ID('dbo.Dependents') IS NOT NULL
    DROP TABLE dbo.Dependents
GO

IF OBJECT_ID('dbo.Employees') IS NOT NULL
    DROP TABLE dbo.Employees
GO

CREATE TABLE dbo.Employees
(
    EmployeeId     int PRIMARY KEY NOT NULL,
    FirstName      nvarchar(50) NOT NULL,
    LastName       nvarchar(50) NOT NULL,
    Department     nvarchar(50) NOT NULL,
    Experience     int NOT NULL,
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

INSERT INTO dbo.Employees(EmployeeId, FirstName, LastName, Department, Experience, BiweeklySalary, Cost, DiscountFactor)
VALUES
(001, 'Thomas', 'Keller', 'Accounting', 4, 2000, 1000, 100),
(002, 'James', 'Richter', 'Accounting', 1, 2000, 1000, 100),
(003, 'Mark', 'Hall', 'Software Development', 1, 2000, 1000, 100),
(004, 'Darren', 'Graves', 'IT', 12, 2000, 1000, 100),
(005, 'Alex', 'Rogers', 'IT', 1, 2000, 1000, 90),
(006, 'Karen', 'Blackburn', 'Personnel', 15, 2000, 1000, 100),
(006, 'Diane', 'Harvey', 'IT', 11, 2000, 1000, 100),
(007, 'Anne', 'Perkins', 'Software Development', 9, 2000, 1000, 90),
(008, 'Steve', 'Rogers', 'Personnel', 6, 2000, 1000, 100),
(009, 'Olivia', 'Dunham', 'Security', 4, 2000, 1000, 100),
(010, 'Brandon', 'Stark', 'Management', 8, 2000, 1000, 100),
(011, 'Cersei', 'Lannister', 'Management', 11, 2000, 1000, 100),
(012, 'Arthur', 'Dayne', 'Software Development', 5, 2000, 1000, 90)
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
