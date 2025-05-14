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

        private async Task<PortfolioRepository> MapToPortfolioAsync(Repository repo)
        {
            var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
            var languageList = languages.Select(l => l.Name).ToList();

            CommitInfo? commitInfo = null;
            try
            {
                var commits = await _client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name, new CommitRequest());
                var lastCommit = commits.FirstOrDefault();

                if (lastCommit != null)
                {
                    commitInfo = new CommitInfo
                    {
                        Message = lastCommit.Commit.Message,
                        Author = lastCommit.Commit.Author?.Name ?? "Unknown",
                        Date = lastCommit.Commit.Author?.Date ?? DateTimeOffset.MinValue
                    };
                }
            }
            catch (ApiException ex) when (ex.ApiError?.Message == "Git Repository is empty.")
            {
                // No commits yet – skip commitInfo
            }

            var prs = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);

            return new PortfolioRepository
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Languages = languageList,
                LastCommit = commitInfo,
                Stars = repo.StargazersCount,
                PullRequests = prs.Count
            };
        }

        public async Task<List<PortfolioRepository>> GetPortfolioAsync()
        {
            var repos = await _client.Repository.GetAllForUser(_options.UserName);
            var result = new List<PortfolioRepository>();

            //foreach (var repo in repos)
            //{
            //    var portfolioRepo = await MapToPortfolioAsync(repo);
            //    result.Add(portfolioRepo);
            //}
            await Parallel.ForEachAsync(repos, async (repo, cancellationToken) =>
            {
                var portfolioRepo = await MapToPortfolioAsync(repo);
                result.Add(portfolioRepo);
            });

            return result;
        }

        public async Task<List<PortfolioRepository>> SearchRepositoriesAsync(string? repositoryName, string? language, string? username)
        {
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            try
            {
                return await InternalSearchAsync(repositoryName, language, username, timeoutCts.Token);
            }
            catch (OperationCanceledException)
            {
                // Timeout נגרם - מחזירים ריק
                return new List<PortfolioRepository>();
            }
        }


        public async Task<List<PortfolioRepository>> InternalSearchAsync(string? repositoryName, string? language, string? username, CancellationToken token)
        {
            var queryParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(repositoryName))
                queryParts.Add(repositoryName);

            if (!string.IsNullOrWhiteSpace(language))
                queryParts.Add($"language:{language}");

            if (!string.IsNullOrWhiteSpace(username))
                queryParts.Add($"user:{username}");

            var query = string.Join(" ", queryParts);

            var request = new SearchRepositoriesRequest(query)
            {
                SortField = RepoSearchSort.Updated,
                Order = SortDirection.Descending,
                PerPage = 20
            };

            var result = await _client.Search.SearchRepo(request);
            var output = new List<PortfolioRepository>();

            //foreach (var repo in result.Items)
            //{
            //    var portfolioRepo = await MapToPortfolioAsync(repo);
            //    output.Add(portfolioRepo);
            //}
            //await Parallel.ForEachAsync(result.Items, async (repo, cancellationToken) =>
            //{
            //    var portfolioRepo = await MapToPortfolioAsync(repo);
            //    output.Add(portfolioRepo);
            //});
            await Parallel.ForEachAsync(result.Items, token, async (repo, ct) =>
            {
                var mapped = await MapToPortfolioAsync(repo);
                output.Add(mapped);
            });

            return output;
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
