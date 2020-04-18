using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazoring.Breadcrumbs
{
    public static class Register
    {
        public static void AddBreadcrumb(this IServiceCollection services, Assembly assembly)
        {
            Console.WriteLine(assembly.FullName);
            services.AddSingleton<BreadcrumbService>();
        }
    }
}
