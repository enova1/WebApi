
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Employee;


namespace WebApi.Controllers.v1;

/// <summary>
/// Employee Controller for the API.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("v1/[controller]")]
public class EmployeesController : Controller
{
    private readonly EmployeeDbContext _employeeDbContext;

    /// <inheritdoc />
    public EmployeesController(EmployeeDbContext context)
    {
        _employeeDbContext = context;
    }

    /// <summary>
    /// Get all employees from the database. 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetTask()
    {
        var employees = await _employeeDbContext.Employees!
            .Include(e => e.EmployeePhones)
            .Include(e => e.EmployeeAddresses)
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Distinct()
            .ToListAsync();

        return employees.Count == 0 ? StatusCode(404, "No employees found.") : StatusCode(200, employees);
    }

    /// <summary>
    /// Filter the employees by phone number and zip code and display the results in the view Index view.
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="zipCode"></param>
    /// <returns></returns>
    [HttpGet("filterBy/")]
    public async Task<IActionResult> FilterTask(string phone, string zipCode)
    {
        var employees = await _employeeDbContext.Employees!
            .Include(e => e.EmployeePhones)
            .Include(e => e.EmployeeAddresses)
            .Where(e => e.EmployeePhones != null && e.EmployeePhones.Any(p => p.PhoneNumber.Contains(phone)))
            .Where(e => e.EmployeeAddresses != null && e.EmployeeAddresses.Any(a => a.ZipCode.Contains(zipCode)))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();

        return employees.Count == 0 ? StatusCode(404, "No employees found.") : StatusCode(200, employees);
    }

    /// <summary>
    /// Display the list of employees with their full name, earliest hire date, latest hire date, and average length of employment in years. 
    /// </summary>
    /// <returns></returns>
    [HttpGet("averageEmployment/")]
    public async Task<IActionResult> AverageLengthTask()
    {
        var employees = await _employeeDbContext.Employees!
            .Select(e => new
            {
                FullName = $"{e.FirstName} {e.LastName}",
                EarliestHireDate = e.HireDate,
                LatestHireDate = e.HireDate,
                AverageLengthOfEmployment = (DateTime.Now - e.HireDate).TotalDays / 365
            })
            .ToListAsync();

        return employees.Count == 0 ? StatusCode(404, "No employees found.") : StatusCode(200, employees);
    }

    /// <summary>
    /// Get an employee by their employeeId from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("id")]
    public async Task<IActionResult> GetDetailsTask(int id)
    {
        if (_employeeDbContext.Employees == null) return BadRequest(id);
        var employees = await _employeeDbContext.Employees.FindAsync(id);
        return employees == null ? NotFound(id) : StatusCode(200, employees);
    }
    
    /// <summary>
    /// Create a new employee in the database.
    /// </summary>
    /// <param name="employees"></param>
    /// <returns></returns>
    [HttpPost()]
    public async Task<IActionResult> Create(Employees employees)
    {
        //TODO: Add the employee Create to EmployeeLibrary
        try
        {
            _employeeDbContext.Add(employees);
            await _employeeDbContext.SaveChangesAsync();

            if (_employeeDbContext.Employees != null)
            {
                var saveData = await _employeeDbContext.Employees
                    .Include(e => e.EmployeePhones)
                    .Include(e => e.EmployeeAddresses)
                    .FirstOrDefaultAsync(m => m.EmployeeId == employees.EmployeeId);

                if (saveData == null)
                {
                    return NotFound();
                }

                if (employees.EmployeePhones != null)
                    foreach (var number in employees.EmployeePhones)
                    {
                        saveData.EmployeePhones!.Add(number);
                    }

                if (employees.EmployeeAddresses != null)
                    foreach (var addy in employees.EmployeeAddresses)
                    {
                        saveData.EmployeeAddresses!.Add(addy);
                    }
            }

            await _employeeDbContext.SaveChangesAsync();

            return StatusCode(200);

        }
        catch (Exception e)
        {
            return StatusCode(500,e.InnerException.Message);
        }
    }

    /// <summary>
    /// save the edited employee in the database.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut, HttpPatch]
    public async Task<IActionResult> Edit(Employees data)
    {
        //TODO: Add the employee Edit to EmployeeLibrary
        if (_employeeDbContext.Employees == null) return BadRequest(data);

        var employees = await _employeeDbContext.Employees.FindAsync(data.EmployeeId);

        if (employees == null)
        {
            return NotFound();
        }

        try
        {
            _employeeDbContext.Update(data);
            await _employeeDbContext.SaveChangesAsync();

            return StatusCode(200, data);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (_employeeDbContext.Employees != null && 
                !_employeeDbContext.Employees.Any(e => e.EmployeeId == data.EmployeeId))
            {
                return NotFound(data);
            }
            return StatusCode(500, ex.Message);
        }

    }


    /// <summary>
    /// Delete an employee from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (_employeeDbContext.Employees != null)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(); // HTTP 404 Not Found if employee with the given id is not found
            }

            _employeeDbContext.Employees.Remove(employee);
        }

        await _employeeDbContext.SaveChangesAsync();

        return NoContent(); // HTTP 204 No Content to indicate successful deletion
    }

}