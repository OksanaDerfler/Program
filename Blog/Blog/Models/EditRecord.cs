﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EditRecord
    {
        public int idrec { get; set; }
        public string tag { get; set; }
        public string title { get; set; }
        public string textarea { get; set; }
        public HttpPostedFileBase uplfile { get; set; } 
    }
}