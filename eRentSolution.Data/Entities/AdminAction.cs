﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class AdminAction
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public List<Censor> Censors { get; set; }
    }
}
