﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthenticateResponseDto
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
