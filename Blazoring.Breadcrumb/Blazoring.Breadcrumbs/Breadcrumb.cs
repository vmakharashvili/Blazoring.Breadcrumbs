using System;
using System.Collections.Generic;
using System.Text;

namespace Blazoring.Breadcrumbs
{
    public class Breadcrumb
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public Breadcrumb? Child { get; set; }

        public bool ContainsUrl(string url, int level)
        {
            if (level == 0)
            {
                return Url?.ToLower() == url?.ToLower();
            }
            else if (level == 1)
            {
                return Child?.Url?.ToLower() == url?.ToLower();
            }
            else
            {
                return GetChild(level)?.Url?.ToLower() == url?.ToLower();
            }
        }

        private Breadcrumb? GetChild(int level)
        {
            var c = Child;
            for (var i = 1; i < level; i++)
            {
                c = Child?.Child;
            }
            return c;
        }
    }
}
