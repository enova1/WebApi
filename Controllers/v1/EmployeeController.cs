
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers.v1;

/// <summary>
/// Employee Controller for the API.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("v1/[controller]")]
public class EmployeeController : Controller
{
    /// <summary>
    /// Create a new employee. 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost()]
    public Task<IActionResult> Create(Employees data)
    {
        Random rand = new ();
        var rando = rand.Next(100);

        data.EmployeeId = rando;
        data.EmployeeAddresses[0].EmployeeId = rando;
        data.EmployeePhones[0].EmployeeId = rando;
        return Task.FromResult<IActionResult>(StatusCode(200, data));
    }

    /// <summary>
    /// Get an employee by their employeeId.
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpGet("employeeId")]
    public Task<IActionResult> GetTask(int employeeId)
    {

        // This is just a mock response
        var data = new Employees
        {
            EmployeeId = employeeId,
            FirstName = "John",
            LastName = "Doe",
            HireDate = DateTime.Now,
            EmployeePhones = [
                new EmployeePhones
                {
                    PhoneNumberId = 1,
                    PhoneType = "Mobile",
                    PhoneNumber = "123-456-7890",
                    EmployeeId = employeeId
                },
                new EmployeePhones
                {
                    PhoneNumberId = 2,
                    PhoneType = "Home",
                    PhoneNumber = "321-654-0987",
                    EmployeeId = employeeId
                }

            ],
            EmployeeAddresses = [
                new EmployeeAddresses
                {
                    AddressId = 1,
                    Address1 = "123 Main St",
                    City = "Largo",
                    State = "FL",
                    ZipCode = "12345",
                    EmployeeId = employeeId
                },
                new EmployeeAddresses
                {
                    AddressId = 2,
                    Address1 = "123 Star Rd",
                    City = "Brooksville",
                    State = "FL",
                    ZipCode = "12345",
                    EmployeeId = employeeId
                }

            ]
        };
        return Task.FromResult<IActionResult>(StatusCode(200, data));
    }
}