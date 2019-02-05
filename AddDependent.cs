
using System;
using System.Collections.Generic;
using System.Web;
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

namespace DagpayPayroll
{
    public static class AddDependent
    {
        private static string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

        [FunctionName("AddDependent")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string[] pairs = requestBody.Replace("?", "").Split('&');
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string employeeId;
            string firstName;
            string lastName;
            int cost;
            int discountFactor;

            foreach (string pair in pairs)
            {
                parameters.Add(pair.Split('=')[0], pair.Split('=')[1]);
            }

            employeeId = parameters["employeeid"].Replace(";", "");
            firstName = parameters["firstname"].Replace(";", "");
            lastName = parameters["lastname"].Replace(";", "");
            cost = 500;
            discountFactor = CalculateDiscountFactor(firstName);


            if ((employeeId != null) && (firstName != null) && (lastName != null))
            {
                int employeeIdInt = Convert.ToInt32(employeeId);
                var RowsAffected = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(@"INSERT INTO dbo.Dependents
                                                                 (EmployeeId, FirstName, LastName, Cost, DiscountFactor)
                                                                 VALUES (@EmployeeId, @FirstName, @LastName, @Cost, @DiscountFactor)", connection))
                    {
                        command.Parameters.Add("@EmployeeId", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@Cost", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@DiscountFactor", System.Data.SqlDbType.Int);

                        command.Parameters["@EmployeeId"].Value = employeeIdInt;
                        command.Parameters["@FirstName"].Value = firstName;
                        command.Parameters["@LastName"].Value = lastName;
                        command.Parameters["@Cost"].Value = cost;
                        command.Parameters["@DiscountFactor"].Value = discountFactor;

                        command.Connection.Open();
                        RowsAffected = await command.ExecuteNonQueryAsync();
                    }
                }

                if (RowsAffected == 1)
                {
                    return (ActionResult)new OkObjectResult("Dependent was successfully added.");
                }
                else
                {
                    return new BadRequestObjectResult("Dependent was not added.  Please try again.");
                }
            }
            else
            {
                return new BadRequestObjectResult("Please submit values for employeeId, firstName, and lastName when adding a dependent.");
            }
        }

        static int CalculateDiscountFactor(string firstName)
        {
            int discountFactor = 100;

            if (firstName.Substring(0, 1) == "A")
            {
                discountFactor = 90;
            }

            return discountFactor;
        }
    }
}
