using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace HelloWorld.Api.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private const string connectionString = "Server=192.168.90.209,1433;Database=mydatabase;User Id=sa;Password=Password.1234";

        // GET api/messages
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var messages = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from dbo.Messages";
                    command.CommandType = System.Data.CommandType.Text;
                    using (var dataReader = command.ExecuteReader())
                    {
                        while(dataReader.Read())
                        {
                            messages.Add(dataReader.GetString(0));
                        }
                    }
                }
            }

            return messages;
        }

        // POST api/messages
        [HttpPost]
        public void Post([FromBody] string message)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"Insert into dbo.Messages (message) values ('{message}')";
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}