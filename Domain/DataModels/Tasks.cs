using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class Tasks
    {
        public Guid id { get; set; }
        public Guid memberId { get; set; }
        public string text { get; set; }
        public bool isDone { get; set; }
    }
}
