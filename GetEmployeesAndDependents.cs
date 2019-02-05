
using System;
using System.Text;
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
    public static class GetEmployeesAndDependents
    {
        //private static SqlConnection connection = new SqlConnection();
        private static string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

        [FunctionName("GetEmployeesAndDependents")]
        public static string Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var jsonResult = new StringBuilder();

                using (SqlCommand command = new SqlCommand("SELECT e.EmployeeId AS[employeeid], e.FirstName AS[firstname], e.LastName AS[lastname], e.Cost AS[cost], e.DiscountFactor AS[discountfactor], (SELECT d1.FirstName AS[firstname], d1.LastName AS[lastname], d1.Cost AS[cost], d1.DiscountFactor AS[discountfactor] FROM dbo.employees e1 INNER JOIN dbo.dependents d1 ON e1.EmployeeId = d1.EmployeeId WHERE e1.EmployeeId = e.EmployeeId FOR JSON PATH) AS[dependents] FROM dbo.employees e ORDER BY e.LastName ASC FOR JSON PATH", connection))
                {
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }

                return jsonResult.ToString();
            }
        }
    }
}
