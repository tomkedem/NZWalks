using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:prtnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: https://localhost:prtnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentsNames = new string[] { "Jone", "Jane", "Mark", "Emily", "David", "Tomer", "Israel" };

            return Ok(studentsNames);

        }
    }
}
