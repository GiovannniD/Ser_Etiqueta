using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ser_Etiqueta.Areas.Identity.Data;
using Ser_Etiqueta.Data;

[assembly: HostingStartup(typeof(Ser_Etiqueta.Areas.Identity.IdentityHostingStartup))]
namespace Ser_Etiqueta.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}