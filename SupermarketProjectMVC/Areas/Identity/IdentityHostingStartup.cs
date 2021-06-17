using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupermarketProjectMVC.Areas.Identity.Data;
using SupermarketProjectMVC.Data;

[assembly: HostingStartup(typeof(SupermarketProjectMVC.Areas.Identity.IdentityHostingStartup))]
namespace SupermarketProjectMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SupermarketProjectMVCContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SupermarketProjectMVCContextConnection")));

                services.AddDefaultIdentity<SupermarketProjectMVCUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SupermarketProjectMVCContext>();
            });
        }
    }
}