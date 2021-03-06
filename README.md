# Dagpay

Welcome to Dagpay, a serverless app that allows users to track payroll benefits deductions for employees and their dependents.  Dagpay consists of a single page JavaScript application, and a serverless FaaS backend (C# Azure Functions) supported by an Azure SQL database.

## Dagpay front-end client

The Dagpay client is hosted in Microsoft Azure, and reached with the following URL:

**_https://dagpaypayroll.z20.web.core.windows.net/_**

Dagpay React, built with React using the same serverless backend as its vanilla JS predecessor, is hosted on Heroku:

**_http://dagpay.herokuapp.com/_**

The client has four sections:

**Total Deductions** - the sum of deductions for all employees and dependents, which is updated as new beneficiaries are added.

**Beneficiaries** - a list of all employees, their individual deductions, and the sum of deductions for their dependents (click an employee to view individual deductions for dependents).

**Add New Employee** - provide a unique numeric Employee Id, First Name, and Last Name

**Add New Dependent** - provide a First Name and Last Name, and select an associated employee

## Dagpay API

Use the following endpoints to interact with the Dagpay C# Azure Functions API:

**_https://dagpay2.azurewebsites.net/api/AddEmployee_** - add a new employee

**_https://dagpay2.azurewebsites.net/api/AddDependent_** - add a new dependent

**_https://dagpay2.azurewebsites.net/api/GetEmployeesAndDependents_** - get a JSON response with all employees and dependents, and their individual payroll deductions

_* Based on total deductions of $1000/year for each employee and $500/year for each dependent, Dagpay calculates the portion owed for each of 26 biweekly pay periods in a year. Beneficiaries whose first names begin with 'A' receive a 10% discount._
