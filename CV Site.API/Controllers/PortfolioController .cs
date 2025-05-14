using CV_Site.Service;
using Microsoft.AspNetCore.Mvc;

namespace CV_Site.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public PortfolioController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPortfolio()
        {
            var portfolio = await _gitHubService.GetPortfolioAsync();
            return Ok(portfolio);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories([FromQuery] string? repositoryName, [FromQuery] string? language, [FromQuery] string? username)
        {
            var results = await _gitHubService.SearchRepositoriesAsync(repositoryName, language, username);
            return Ok(results);
        }

    }
}
