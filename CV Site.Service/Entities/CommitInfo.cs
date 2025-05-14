using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Site.Service.Entities
{
    public class CommitInfo
    {
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
