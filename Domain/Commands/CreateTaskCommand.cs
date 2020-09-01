using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public Guid memberId { get; set; }
        public string text { get; set; }
        public bool isDone { get; set; }
    }
}
