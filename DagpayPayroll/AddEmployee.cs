
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using DagpayPayroll.Helpers;

namespace DagpayPayroll
{
    public static class AddEmployee
    {
        private static string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

        [FunctionName("AddEmployee")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string[] pairs = requestBody.Replace("?", "").Split('&');
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string employeeId;
            string firstName;
            string lastName;
            string department;
            string experienceString;
            int experience;
            int biweeklySalary;
            int cost;
            int discountFactor;

            foreach (string pair in pairs)
            {
                parameters.Add(pair.Split('=')[0], pair.Split('=')[1]);
            }

            employeeId = parameters["employeeid"].Replace(";", "");
            firstName = parameters["firstname"].Replace(";", "");
            lastName = parameters["lastname"].Replace(";", "");
            department = parameters["department"].Replace(";", "");
            experienceString = parameters["experience"].Replace(";", "");
            biweeklySalary = 2000;
            cost = 1000;
            discountFactor = DiscountFactorCalculator.CalculateDiscountFactor(firstName);

            experience = int.TryParse(experienceString, out int exp) ? exp : 0;

            if ((employeeId != null) && (firstName != null) && (lastName != null))
            {
                int employeeIdInt = Convert.ToInt32(employeeId);
                var RowsAffected = 0; //Placeholder value

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(@"INSERT INTO dbo.Employees
                                                                 (EmployeeId, FirstName, LastName, Department, Experience, BiweeklySalary, Cost, DiscountFactor)
                                                                 VALUES (@EmployeeId, @FirstName, @LastName, @Department, @Experience, @BiweeklySalary, @Cost, @DiscountFactor)", connection))
                    {
                        command.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@Department", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@Experience", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@BiweeklySalary", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@Cost", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@DiscountFactor", System.Data.SqlDbType.Int);

                        command.Parameters["@EmployeeId"].Value = employeeIdInt;
                        command.Parameters["@FirstName"].Value = firstName;
                        command.Parameters["@LastName"].Value = lastName;
                        command.Parameters["@Department"].Value = department;
                        command.Parameters["@Experience"].Value = experience;
                        command.Parameters["@BiweeklySalary"].Value = biweeklySalary;
                        command.Parameters["@Cost"].Value = cost;
                        command.Parameters["@DiscountFactor"].Value = discountFactor;

                        command.Connection.Open();
                        RowsAffected = await command.ExecuteNonQueryAsync();
                    }
                }

                if (RowsAffected == 1)
                {
                    return (ActionResult)new OkObjectResult("Employee was successfully added.");
                }
                else
                {
                    return new BadRequestObjectResult("Employee was not added.  Please try again.");
                }
            }
            else
            {
                return new BadRequestObjectResult("Please submit values for employeeId, firstName, and lastName when adding an employee.");
            }
        }
    }
}
