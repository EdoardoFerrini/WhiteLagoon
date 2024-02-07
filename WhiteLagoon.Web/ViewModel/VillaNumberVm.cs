﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.ViewModel
{
    public class VillaNumberVm
    {
        public VillaNumber? VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DropdownVilla {  get; set; }
    }
}
