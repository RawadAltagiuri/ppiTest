using FirstWebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public CustomerController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("firstnames")]
    public IActionResult GetFirstNames()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection"); // Replace with your actual connection string key

            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT FirstName FROM customer";

            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            List<string> firstNames = new List<string>();

            while (reader.Read())
            {
                string firstName = reader["FirstName"].ToString();
                firstNames.Add(firstName);
            }

            return Ok(firstNames);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("firstnames")]
    public IActionResult CreateCustomer([FromBody] CustomerCreateModel customerModel)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Find the last customer number used
            string lastCustomerQuery = "SELECT MAX(CustomerID) FROM customer";
            using MySqlCommand lastCustomerCommand = new MySqlCommand(lastCustomerQuery, connection);
            object maxCustomerID = lastCustomerCommand.ExecuteScalar();
            int newCustomerID = (maxCustomerID == DBNull.Value) ? 1 : Convert.ToInt32(maxCustomerID) + 1;

            // Insert the new customer with the incremented customer number
            string insertQuery = "INSERT INTO customer (CustomerID, FirstName, LastName) VALUES (@CustomerID, @FirstName, @LastName)";

            using MySqlCommand command = new MySqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@CustomerID", newCustomerID);
            command.Parameters.AddWithValue("@FirstName", customerModel.FirstName);
            command.Parameters.AddWithValue("@LastName", customerModel.LastName); // Add this line

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return Ok("Customer created successfully.");
            }
            else
            {
                return BadRequest("Failed to create customer.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}