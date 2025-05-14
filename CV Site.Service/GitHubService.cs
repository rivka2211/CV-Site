using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Site.Service
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        public GitHubService()
        {
            _client = new GitHubClient(new ProductHeaderValue("rivka2211"));
        }

        public async Task<int> GetUserFollowesAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }

        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var request = new SearchRepositoriesRequest("repo—nane") { Language = Language.CSharp };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
    }
}
