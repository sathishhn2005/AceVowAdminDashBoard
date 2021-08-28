using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceVowEntities
{
    public class SchedulePosts
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ImageUrL { get; set; }
        public string EmailId { get; set; }

        public string Text { get; set; }

        public DateTime PostDate { get; set; }

        public TimeSpan PostTime { get; set; }

        public DateTime? PostedDate { get; set; }
        public DateTime? ScheduledTime { get; set; }

        public int? PostStatus { get; set; }
        public string FacebookPost { get; set; }
        public string PageName { get; set; }
        public string Message { get; set; }
    }
}
