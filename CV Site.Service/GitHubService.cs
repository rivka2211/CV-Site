using CV_Site.Service.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly GitHubOptions _options;

        public GitHubService(IOptions<GitHubOptions> options)
        {
            _options = options.Value;
            _client = new GitHubClient(new ProductHeaderValue(_options.UserName));
            _client.Credentials = new Credentials(_options.Token);
        }

        public async Task<List<PortfolioRepository>> GetPortfolioAsync()
        {
            var repos = await _client.Repository.GetAllForUser(_options.UserName);
            var result = new List<PortfolioRepository>();

            foreach (var repo in repos)
            {
                // שפות
                var languages = await _client.Repository.GetAllLanguages(_options.UserName, repo.Name);
                var languageList = languages.Select(l => l.Name).ToList();

                // Removed 'PerPage' as it is not a valid property of CommitRequest.
                var commits = await _client.Repository.Commit.GetAll(_options.UserName, repo.Name, new CommitRequest());
                var lastCommit = commits.FirstOrDefault();
                var commitInfo = lastCommit != null ? new CommitInfo
                {
                    Message = lastCommit.Commit.Message,
                    Author = lastCommit.Commit.Author?.Name ?? "Unknown",
                    Date = lastCommit.Commit.Author?.Date ?? DateTimeOffset.MinValue
                } : null;

                // pull requests
                var pullRequests = await _client.PullRequest.GetAllForRepository(_options.UserName, repo.Name);

                result.Add(new PortfolioRepository
                {
                    Name = repo.Name,
                    HtmlUrl = repo.HtmlUrl,
                    Languages = languageList,
                    LastCommit = commitInfo,
                    Stars = repo.StargazersCount,
                    PullRequests = pullRequests.Count
                });
            }

            return result;
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
        //get user public repo
        //get user repos
        //get user portpolio
    }
}
