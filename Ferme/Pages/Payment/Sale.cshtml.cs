﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ferme.Pages.Payment
{
    [Authorize]
    public class SaleModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}