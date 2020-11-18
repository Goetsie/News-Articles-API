using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Like
    {
        public int LikeID { get; set; }

        //Relations
        public int ArticleID { get; set; }
        public int UserID { get; set; }
        public Article article { get; set; }
        public User User { get; set; }

    }
}
