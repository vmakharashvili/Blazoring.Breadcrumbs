using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazoring.Breadcrumbs
{
    public abstract class ComponentBaseBreadcrumbs : ComponentBase
    {
        [Inject]
        protected BreadcrumbService? _BreadcrumbService { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {

                _BreadcrumbService.InitBreadCrumb();
            }
        }
    }
}
