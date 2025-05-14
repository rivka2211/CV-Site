using Octokit;

namespace CV_Site.Service
{
    public interface IGitHubService
    {
          Task<int> GetUserFollowesAsync(string userName);

          Task<List<Repository>> SearchRepositoriesInCSharp();
    }
}