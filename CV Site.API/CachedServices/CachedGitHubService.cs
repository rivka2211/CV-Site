using CV_Site.Service;
using CV_Site.Service.Entities;
using Microsoft.Extensions.Caching.Memory;
using Octokit;

namespace CV_Site.API.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        const string userPortfolioKey = "userPortfolioKey";

        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        async Task<List<PortfolioRepository>> IGitHubService.GetPortfolioAsync()
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out List<PortfolioRepository> portfolio))
                return portfolio;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            portfolio = await _gitHubService.GetPortfolioAsync();
            _memoryCache.Set(userPortfolioKey, portfolio,cacheOptions);
            return portfolio;
        }

        Task<int> IGitHubService.GetUserFollowesAsync(string userName)
        {
            return _gitHubService.GetUserFollowesAsync(userName);
        }

        Task<List<PortfolioRepository>> IGitHubService.SearchRepositoriesAsync(string? repositoryName, string? language, string? username)
        {
            return _gitHubService.SearchRepositoriesAsync(repositoryName, language, username);
        }

        Task<List<Repository>> IGitHubService.SearchRepositoriesInCSharp()
        {
            return _gitHubService.SearchRepositoriesInCSharp();
        }
    }
}
