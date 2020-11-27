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
    public class LikeController : ControllerBase
    {
        private readonly NewsContext _context;

        public LikeController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Like -- Get all likes
        // (No Authorize --> total likes for guests)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes(int articleID, int userID)
        {

            return await _context.Likes.Include(u => u.User).Include(a => a.Article).ToListAsync();
            //return await _context.Likes.Include(u => u.User).ToListAsync();
            //return await _context.Likes.ToListAsync();

        }

        // POST: api/Like
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(Like like)
        {

            //Console.WriteLine("Like: " + like);
            //Console.WriteLine(" ");
            //Console.WriteLine("LikeId: " + like.LikeID);
            //Console.WriteLine("UserID: " + like.UserID);
            //Console.WriteLine("ArticleID: " + like.ArticleID);

            like.User = await _context.Users.FindAsync(like.UserID);
            like.Article = await _context.Articles.FindAsync(like.ArticleID);

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetLike", new { id = like.LikeID }, like);
            return Ok(like);
        }

        // DELETE: api/Like/5
        // User wants to delete his like on an article
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Like>> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return like;
        }
    }
}
