using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {

        private readonly NewsContext _context;

        public ReactionController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Reaction -- Get all reactions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reaction>>> GetReactions(int articleID)
        {
            if (articleID != 0 && articleID!=null)
            {
                return await _context.Reactions.Where(r => r.ArticleID == articleID).Include(u => u.User).Include(u => u.User.Role).Include(a => a.Article).Include(a => a.Article.ArticleStatus).ToListAsync();
            }
            else
            {
                return await _context.Reactions.Include(u => u.User).Include(u => u.User.Role).Include(a => a.Article).Include(a => a.Article.ArticleStatus).ToListAsync();
            }

            return await _context.Reactions.Include(u => u.User).Include(u => u.User.Role).Include(a => a.Article).Include(a => a.Article.ArticleStatus).ToListAsync();
            //return await _context.Reactions.ToListAsync();

        }

        // GET: api/Reaction/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Reaction>> GetReaction(int id)
        {

            var reaction = await _context.Reactions.Include(u => u.User).Include(u => u.User.Role).Include(a => a.Article).Include(a => a.Article.ArticleStatus).SingleOrDefaultAsync(i => i.ReactionID == id);
            //var reaction = await _context.Reactions.SingleOrDefaultAsync(i => i.ReactionID == id);

            if (reaction == null)
            {
                return NotFound();
            }

            return reaction;
        }


        // PUT: api/Reaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReaction(int id, Reaction reaction)
        {
            if (id != reaction.ReactionID)
            {
                return BadRequest();
            }

            _context.Entry(reaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(true);
        }

        // POST: api/Reaction
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<Reaction>> PostReaction(Reaction reaction)
        {

            reaction.Date.ToLocalTime();

            //reaction.User = await _context.Users.FindAsync(reaction.UserID);
            //reaction.Article = await _context.Articles.FindAsync(reaction.ArticleID);

            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetReaction", new { id = reaction.ReactionID }, reaction);
            return Ok(reaction);
        }

        // DELETE: api/Reaction/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reaction>> DeleteReaction(int id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction == null)
            {
                return NotFound();
            }

            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();

            return reaction;
        }

        private bool ReactionExists(int id)
        {
            return _context.Reactions.Any(e => e.ReactionID == id);
        }


    }
}
