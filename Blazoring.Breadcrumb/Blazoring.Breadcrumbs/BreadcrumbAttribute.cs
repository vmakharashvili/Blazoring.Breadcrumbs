using System;
using System.Collections.Generic;
using System.Text;

namespace Blazoring.Breadcrumbs
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class BreadcrumbAttribute : Attribute
    {
        public string Name { get; set; }
        public BreadcrumbAttribute(string name)
        {
            Name = name;
        }
    }
}
