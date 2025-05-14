﻿using CV_Site.Service;
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
    }
}
