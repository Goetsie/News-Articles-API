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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes(int articleID, int userID)
        {
            //if (articleID != 0)
            //{
            //    // Find all the likes for an article
            //    return await _context.Likes.Include(u => u.User).Include(a => a.Article).Where(l => l.ArticleID == articleID).ToListAsync();
            //}else if(userID != 0)
            //{
            //    return await _context.Likes.Include(u => u.User).Include(a => a.Article).Where(l => l.UserID == userID).ToListAsync();
            //}
            //else
            //{
            //    return NotFound();
            //}

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

            like.User = await _context.Users.FindAsync(like.UserID);
            like.Article = await _context.Articles.FindAsync(like.ArticleID);

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.LikeID }, like);
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
