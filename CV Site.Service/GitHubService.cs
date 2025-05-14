using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public GitHubService(IConfiguration configuration)
        {
            _client = new GitHubClient(new ProductHeaderValue("rivka2211"));
            _configuration = configuration;
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
