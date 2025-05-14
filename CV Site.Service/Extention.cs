using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CV_Site.Service
{
   public static class Extention
    {
        public static void AddgitHubIntegration(this IServiceCollection services,Action<GitHubOptions> configuration)
        {
            services.Configure(configuration);
            services.AddScoped<IGitHubService,GitHubService>();
        }
    }
}
