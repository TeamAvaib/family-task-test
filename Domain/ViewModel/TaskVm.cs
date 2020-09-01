using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModel
{
    public class TaskVm
    {
        public Guid id { get; set; }
        public Guid memberId { get; set; }
        public string text { get; set; }
        public bool isDone { get; set; }
        public MemberVm member { get; set; }
    }
}
