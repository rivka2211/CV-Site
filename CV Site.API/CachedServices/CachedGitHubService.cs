using CV_Site.Service;
using CV_Site.Service.Entities;
using Octokit;

namespace CV_Site.API.CachedServices
{
    public class CachedGitHubService:IGitHubService
    {
        private readonly IGitHubService _gitHubService;

        public CachedGitHubService(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        Task<List<PortfolioRepository>> IGitHubService.GetPortfolioAsync()
        {
           return _gitHubService.GetPortfolioAsync();
        }

        Task<int> IGitHubService.GetUserFollowesAsync(string userName)
        {
            return _gitHubService.GetUserFollowesAsync(userName);
        }

        Task<List<PortfolioRepository>> IGitHubService.SearchRepositoriesAsync(string? repositoryName, string? language, string? username)
        {
            return _gitHubService.SearchRepositoriesAsync(repositoryName,language,username);
        }

        Task<List<Repository>> IGitHubService.SearchRepositoriesInCSharp()
        {
            return _gitHubService.SearchRepositoriesInCSharp();
        }
    }
}
