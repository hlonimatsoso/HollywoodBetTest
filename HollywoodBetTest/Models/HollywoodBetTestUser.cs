﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Models
{
    public class HollywoodBetTestUser: IdentityUser
    {
        public string City { get; set; }
        public byte Age { get; set; }

    }
}
