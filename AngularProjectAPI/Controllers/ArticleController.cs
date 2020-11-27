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
    public class ArticleController : ControllerBase
    {
        private readonly NewsContext _context;

        public ArticleController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(u => u.User.Role).Include(s => s.ArticleStatus).ToListAsync();
        }

        // GET: api/Article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            //var article = await _context.Articles.FindAsync(id);
            //var article = await _context.Articles.SingleOrDefaultAsync(i => i.ArticleID == id);
            var article = await _context.Articles.Include(t => t.Tag).Include(u => u.User).Include(u => u.User.Role).Include(s => s.ArticleStatus).SingleOrDefaultAsync(i => i.ArticleID == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Article/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Article
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            //var user = await _context.Users.FindAsync(article.UserID);
            //var tag = await _context.Tags.FindAsync(article.TagID);
            //var articleStatus = await _context.ArticleStatuses.FindAsync(article.ArticleStatusID);

            // Dubug: 
            //Console.WriteLine("Debugging add article");
            //Console.WriteLine("Title: " + article.Title);
            //Console.WriteLine("Subtitle: " + article.SubTitle);
            //Console.WriteLine("Short summary: " + article.ShortSummary);
            //Console.WriteLine("Body: " + article.Body);
            //Console.WriteLine("TagID: " + article.TagID);
            //Console.WriteLine("UserID: " + article.UserID);
            //Console.WriteLine("article status: " + article.ArticleStatusID);

            article.User = await _context.Users.FindAsync(article.UserID);
            article.User.Role = await _context.Roles.FindAsync(article.User.UserID);
            article.Tag = await _context.Tags.FindAsync(article.TagID);
            article.ArticleStatus = await _context.ArticleStatuses.FindAsync(article.ArticleStatusID);

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.ArticleID }, article);
        }

        // DELETE: api/Article/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
