using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using orion.Models;
using orion.Data;

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

        //constructor
        public NotesController(OrionDbContext context)
        {
            _context = context;
        }


        //GET
        [HttpGet]
        public async Task<ActionResult<List<Notes>>> Get()
        {

            return Ok(await _context.Notes.ToListAsync());
        }

        //GET single element
        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> Get(int? id)
        {


            var note = _context.Notes.FindAsync(id);
            if (note == null)
            {
                return BadRequest("hero no found");
            }
            return Ok(note);
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<Notes>> AddHero([FromBody] NoteCreateModel note)
        {
            // Create a new Note instance
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
        [HttpPut]
        public async Task<ActionResult<Notes>> UpdateHero([FromBody] NoteCreateModel updatedNote)
        {
            var dbNote = await _context.Notes.FindAsync(updatedNote.Id);
            if (dbNote == null)
            {
                return BadRequest("note not found");
            }

            dbNote.Title = updatedNote.Title;
            dbNote.Content = updatedNote.Content;

            await _context.SaveChangesAsync();

            return Ok(dbNote);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Notes>>> Delete(int? id)
        {
            var dbNote = await _context.Notes.FindAsync(id);
            if (dbNote == null)
            {
                return BadRequest("note no found");
            }
            _context.Notes.Remove(dbNote);
            await _context.SaveChangesAsync();

            return Ok(await _context.Notes.ToListAsync());
        }
    }
}

