using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace HttpTriggerDbFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            var str = Environment.GetEnvironmentVariable("SqlServerConnection");
            using (SqlConnection conn = new(str))
            {
                conn.Open();
                var query =
                    "drop table if exists Product;" +
                    "create table Product (\r\n\tId NVARCHAR(36) NOT NULL,\r\n\tName NVARCHAR(128) NOT NULL,\r\n\tPrice DECIMAL(4,2) NOT NULL\r\n);\r\n\r\n" +
                    "insert into Product values \r\n('31eefa5d-c94a-4810-b48d-69d32d08c41f', 'T-Shirt', '56.33'),\r\n('757e5c54-63c4-44d6-9b16-4582bd83d3e0', 'Jacket', 			'75.18');";

                using SqlCommand cmd = new(query, conn);
                await cmd.ExecuteNonQueryAsync();

                return new OkObjectResult("DB Changes Completed!");
            }
        }
    }
}
