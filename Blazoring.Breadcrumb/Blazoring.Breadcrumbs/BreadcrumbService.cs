using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazoring.Breadcrumbs
{
    public class BreadcrumbService
    {
        public Breadcrumb? BreadCrumb { get; set; }

        private List<Breadcrumb> MyBreadcrumbs { get; set; }

        private NavigationManager _navigationManager { get; set; }
        public Assembly Assembly { get; set; }

        public BreadcrumbService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            GetComponentUrls();
        }

        private void GetComponentUrls()
        {
            MyBreadcrumbs = new List<Breadcrumb>();
            
            foreach (var c in Assembly.ExportedTypes.Where(t => t.IsSubclassOf(typeof(ComponentBase))))
            {
                Console.WriteLine(c.FullName);
                var bc = new Breadcrumb();
                bool isChild = false;
                string? route = null;
                string? name = null;
                foreach (var attr in c.GetCustomAttributes())
                {
                    if (attr is RouteAttribute ra)
                    {
                        if (ra.Template == "/")
                        {
                            route = ra.Template;
                            continue;
                        }
                        else if (ra.Template.Length > 1)
                        {
                            var firstSlashless = ra.Template.Substring(1, ra.Template.Length - 1);
                            if (firstSlashless.Contains("/"))
                            {
                                var li = firstSlashless.LastIndexOf("/");
                                var parentRoute = "/" + firstSlashless.Substring(0, li);
                                var childRoute = firstSlashless.Substring(li, firstSlashless.Length - li);
                                if (MyBreadcrumbs.Any(x => x.Url == parentRoute))
                                {
                                    isChild = true;
                                    var parent = MyBreadcrumbs.Find(x => x.Url == parentRoute);
                                    bc.Name = parent.Name;
                                    bc.Url = parent.Url;
                                    route = childRoute;
                                }
                                else
                                {
                                    route = ra.Template;
                                }
                            }
                            else
                            {
                                route = ra.Template;
                            }
                        }
                    }
                    if (attr is BreadcrumbAttribute b)
                    {
                        name = b.Name;
                    }
                }
                if (!isChild)
                {
                    bc.Name = name;
                    bc.Url = route;
                }
                else
                {
                    bc.Child = new Breadcrumb() { Name = name, Url = route };
                }
                if (bc.Name != null && bc.Url != null)
                    MyBreadcrumbs.Add(bc);
            }
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(MyBreadcrumbs));
        }

        public void InitBreadCrumb()
        {
            var route = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            Console.WriteLine(route);
            var myBreadcrumbs = MyBreadcrumbs;
            if (route.Contains("/"))
            {
                var routes = route.Split('/');
                var i = 0;
                foreach (var r in routes)
                {
                    if (MyBreadcrumbs.Any(x => x.ContainsUrl("/" + r, i)))
                    {
                        myBreadcrumbs = MyBreadcrumbs.Where(x => x.ContainsUrl("/" + r, i)).ToList();
                        i++;
                    }
                }
            }
            else if (route == "")
            {
                BreadcrumbInstantiated?.Invoke(this, null);
                return;
            }
            else
            {
                if (MyBreadcrumbs.Any(x => x.ContainsUrl("/" + route, 0)))
                    myBreadcrumbs = MyBreadcrumbs.Where(x => x.ContainsUrl("/" + route, 0)).ToList();
            }
            if (myBreadcrumbs?.Count > 0)
            {
                BreadcrumbInstantiated?.Invoke(this, myBreadcrumbs.FirstOrDefault());
            }
        }

        public EventHandler<Breadcrumb?>? BreadcrumbInstantiated;
    }
}
