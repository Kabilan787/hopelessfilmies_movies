﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopelessFilmies.Domain.Models
{
    public class Admin
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
