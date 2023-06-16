using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orion.Data;
using orion.Models;

namespace orion.Controllers
{
    public class NoteCreateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly OrionDbContext _context;

        public NotesController(OrionDbContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notes>>> GetAll(int? userId)
        {
            if (userId.HasValue)
            {
                var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
                return Ok(notes);
            }
            else
            {
                var notes = await _context.Notes.ToListAsync();
                return Ok(notes);
            }
        }

        //GET with ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> Get(int? id)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                return BadRequest("Note not found");
            }
            return Ok(note);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<Notes>> AddNote([FromBody] NoteCreateModel note)
        {
            var user = await _context.Users.FindAsync(note.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID");
            }

            var newNote = new Notes
            {
                Title = note.Title,
                Content = note.Content,
                UserId = note.UserId
            };

            _context.Notes.Add(newNote);
            await _context.SaveChangesAsync();

            return Ok(newNote);
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<Notes>> UpdateHero(int id, [FromBody] Notes updatedNote)
        {
            var dbNote = await _context.Notes.FindAsync(id);
            if (dbNote == null)
            {
                return BadRequest("Note not found");
            }

            dbNote.Title = updatedNote.Title;
            dbNote.Content = updatedNote.Content;

            // Only update the UserId if it is provided in the updatedNote object
            if (updatedNote.UserId != 0)
            {
                dbNote.UserId = updatedNote.UserId;
            }

            // Only update the User if it is provided in the updatedNote object
            if (updatedNote.User != null)
            {
                dbNote.User = updatedNote.User;
            }

            await _context.SaveChangesAsync();

            return Ok(dbNote);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notes>> Delete(int? id)
        {
            var dbNote = await _context.Notes.FindAsync(id);
            if (dbNote == null)
            {
                return BadRequest("Note not found");
            }
            _context.Notes.Remove(dbNote);
            await _context.SaveChangesAsync();

            return Ok(dbNote);
        }
    }
}