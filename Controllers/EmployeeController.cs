using department.DTO;
using department.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace department.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
            
        }
        // Method to map Employee to EmployeeDTO
        private EmployeeDTO MapToDTO(Employee employee) => new EmployeeDTO
        {
            id = employee.id,
            name = employee.name,
            age=employee.age,
            DepartmentName=employee.department?.name


           
        };
        //get all employees
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var employees = await _db.employees.ToListAsync();
                var employeeDTOs = employees.Select(e => MapToDTO(e));

                return Ok(employeeDTOs);
            }
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }
        //get employee by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var employee = await _db.employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(MapToDTO(employee));
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not shown)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }
        //create employee
        [HttpPost]
        public async Task<IActionResult> Post(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }
                await _db.employees.AddAsync(employee);
                await _db.SaveChangesAsync();
                // Map to EmployeeDTO before returning
                var employeeDTO = MapToDTO(employee); 
                return CreatedAtAction(nameof(Get), new { id = employee.id }, employeeDTO); // for get location in header as we use in angular 
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not shown)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data");
            }
        }
        //edit employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Employee employee)
        {
            try
            {
                // Validate the model state and check if the IDs match
                if (!ModelState.IsValid || id != employee.id)
                {
                    return BadRequest(ModelState); //Return 400
                }
                // Check if the employee exists
                var existingEmployee = await _db.employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound(); // Return 404 if the employee does not exist
                }
                _db.Entry(existingEmployee).CurrentValues.SetValues(employee);
                await _db.SaveChangesAsync();
                // Map to EmployeeDTO before returning
                var employeeDTO = MapToDTO(existingEmployee); 
                return Ok(employeeDTO); // Return 200 with the updated EmployeeDTO
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not shown)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        //delete employee
          [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _db.employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound("Employee with Id = " + id.ToString() + " not found");
                }
                _db.employees.Remove(employee);
                await _db.SaveChangesAsync();
                // Map to EmployeeDTO before returning
                var employeeDTO = MapToDTO(employee); 
                return Ok(employeeDTO); // Return 200 with the deleted EmployeeDTO
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not shown)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
