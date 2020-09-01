using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;

namespace WebClient.Abstractions
{
    public interface ITaskDataService
    {
        public Task<CreateTaskCommandResult> CreateTask(CreateTaskCommand command);
        public Task<UpdateTaskStatusCommandResult> UpdateTaskStatus(UpdateTaskStatusCommand command);
        public Task<UpdateTaskStatusCommandResult> UpdateTaskMember(UpdateTaskMemberCommand command);
        public Task<GetAllTasksQueryResult> GetAllTasks();

    }
}
