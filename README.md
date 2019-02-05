# Dagpay

Welcome to Dagpay, a custom app that allows users to track payroll benefits deductions for employees and their dependents.  Dagpay consists of a simple HTML, CSS and vanilla JavaScript single page app and C# Azure Function endpoints supported by an Azure SQL database.

## Dagpay front-end client

The Dagpay client is hosted in Microsoft Azure, and reached at the following URL:

**_https://dagpay.z13.web.core.windows.net/index.html_**

The client has four sections:

**Total Deductions** - the sum of deductions for all employees and dependents, which is updated as new beneficiaries are added.

**Beneficiaries** - a list of all employees, their individual deductions, and the sum of deductions for their dependents (click an employee to view individual deductions for dependents).

**Add New Employee** - provide a unique numeric Employee Id, First Name, and Last Name

**Add New Dependent** - provide a First Name and Last Name, and select an associated employee

## Dagpay API

Use the following endpoints to interact with the Dagpay C# Azure Functions API:

**_https://dagpaypayroll.azurewebsites.net/api/AddEmployee_** - add a new employee

**_https://dagpaypayroll.azurewebsites.net/api/AddDependent_** - add a new dependent

**_https://dagpaypayroll.azurewebsites.net/api/GetEmployeesAndDependents_** - get a JSON response with all employees and dependents, and their individual payroll deductions

_* Based on total deductions of $1000/year for each employee and $500/year for each dependent, Dagpay calculates the portion owed for each of 26 yearly pay periods. Beneficiaries whose first names begin with 'A' receive a 10% discount._
