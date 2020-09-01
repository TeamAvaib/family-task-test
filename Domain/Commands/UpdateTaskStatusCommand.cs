using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class UpdateTaskStatusCommand
    {
        public Guid Id { get; set; }
        public bool isDone { get; set; }
    }
}
