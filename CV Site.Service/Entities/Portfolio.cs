using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Site.Service.Entities
{
    public class PortfolioRepository
    {
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public List<string> Languages { get; set; }
        public CommitInfo LastCommit { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
    }
}
