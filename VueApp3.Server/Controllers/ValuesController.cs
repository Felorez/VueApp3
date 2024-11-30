using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MyDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get all users or filter by userName and/or descriptionKeyword, with optional sorting
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyData>>> Get(
            [FromQuery] string? userName,
            [FromQuery] string? descriptionKeyword,
            [FromQuery] int? sortByUID,
            [FromQuery] int? sortByUD)
        {
            IQueryable<MyData> query = _context.MyData;

            // Фильтрация по userName, если он передан
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(d => d.UserName == userName);
            }

            // Фильтрация по descriptionKeyword, если он передан
            if (!string.IsNullOrEmpty(descriptionKeyword))
            {
                query = query.Where(d => d.UserDescription != null && d.UserDescription.Contains(descriptionKeyword));
            }

            // Сортировка по UserName
            if (sortByUID.HasValue)
            {
                query = sortByUID.Value == 0
                    ? query.OrderBy(d => d.UserName)
                    : query.OrderByDescending(d => d.UserName);
            }

            // Сортировка по UserDescription
            if (sortByUD.HasValue)
            {
                query = sortByUD.Value == 0
                    ? query.OrderBy(d => d.UserDescription)
                    : query.OrderByDescending(d => d.UserDescription);
            }

            return Ok(await query.ToListAsync());
        }

        //Get only 1 user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<MyData>> Get(int id)
        {
            var data = await _context.MyData.FindAsync(id);

            if (data == null)
            {
                return NotFound("User not found");
            }

            return Ok(data);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] MyData updatedData)
        {
            var existingData = await _context.MyData.FindAsync(updatedData.UserId);

            if (existingData == null)
            {
                return NotFound("User not found");
            }


            existingData.UserName = updatedData.UserName;
            existingData.UserDescription = updatedData.UserDescription;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<MyData>> Post([FromBody] MyData newData)
        {
            _context.MyData.Add(newData);
            await _context.SaveChangesAsync(); 

            return CreatedAtAction(nameof(Get), new { id = newData.UserId }, newData);
        }

     
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.MyData.FindAsync(id); 
            if (user == null)
            {
                return NotFound();
            }

           
            _context.MyData.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

     
        [HttpOptions]
        public ActionResult AllowRequest()
        {
            return Ok("Allow: GET, POST, PUT, DELETE, OPTIONS");
        }

    }
}