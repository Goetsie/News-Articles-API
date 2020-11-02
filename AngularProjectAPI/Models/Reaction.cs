using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Reaction
    {
        public int ReactionID { get; set; }
        public int UserID { get; set; }
        public int ArticleID { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        //Relations
        public User User { get; set; }
        public Article Article { get; set; }

    }
}
