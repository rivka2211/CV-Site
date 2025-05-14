using CV_Site.Service.Entities;
using Octokit;

namespace CV_Site.Service
{
    public interface IGitHubService
    {
        Task<int> GetUserFollowesAsync(string userName);

        Task<List<Repository>> SearchRepositoriesInCSharp();

        Task<List<PortfolioRepository>> GetPortfolioAsync();

        Task<List<PortfolioRepository>> SearchRepositoriesAsync(string? repositoryName, string? language, string? username);

    }
}
