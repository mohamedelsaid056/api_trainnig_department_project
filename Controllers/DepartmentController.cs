using department.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace department.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }


        //get all departments
        [HttpGet]
        public async Task<IActionResult> Get(){


            var departments=await _db.departments.ToListAsync();

            return Ok(departments);
        }
        //get department by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var department = await _db.departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        //create department
        [HttpPost]
        public async Task<IActionResult> Post(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return the model state for better error details
            }

            await _db.departments.AddAsync(department); 
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = department.id }, department); // Use nameof for refactoring safety
        }
        //edit department
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Department department)
        {
            // Validate the model state and check if the IDs match
            if (!ModelState.IsValid || id != department.id)
            {
                return BadRequest(ModelState); //Return 400
            }

            // Check if the department exists
            var existingDepartment = await _db.departments.FindAsync(id);
            if (existingDepartment == null)
            {
                return NotFound(); // Return 404 if the department does not exist
            }

            // Update the department
            _db.Entry(department).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        //delete department
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _db.departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _db.departments.Remove(department);
            await _db.SaveChangesAsync();
            // Return 204 and i need to write a message
            return StatusCode(204,"Department deleted successfully");

        }
    }
}
