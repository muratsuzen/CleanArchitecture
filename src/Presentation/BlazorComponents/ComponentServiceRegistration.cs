using BlazorComponents.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorComponents
{
    public static class ComponentServiceRegistration
    {
        public static IServiceCollection AddComponentServices(this IServiceCollection services)
        {
            services.AddSingleton<ProductService>();

            services.AddHttpClient("WebAPI", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5029/api");
            });

            return services;
        }
    }
}
