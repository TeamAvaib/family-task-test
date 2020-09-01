using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class UpdateTaskMemberCommand
    {
        public Guid id { get; set; }
        public Guid memberId { get; set; }
    }
}
