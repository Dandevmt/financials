﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Security.UseCases
{
    public class LoginInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}