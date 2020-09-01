using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class TasksBase : ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected MenuItem[] leftMenuItem;
        protected TaskModel[] tasksToShow;
        protected List<TaskModel> allTasks = new List<TaskModel>();
        protected TaskModel objTask = new TaskModel();
        protected FamilyMember objMember = new FamilyMember();
        protected bool isLoaded;
        protected bool showLister;
        protected bool showCreator;
        protected Guid val = Guid.Empty;
        private readonly IJSRuntime jsRuntime;
        [Inject]
        public IMemberDataService MemberDataService { get; set; }
        public ITaskDataService TaskDataService { get; set; }


        public TasksBase(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        protected override async Task OnInitializedAsync()
        {
            var result = await MemberDataService.GetAllMembers();

            if (result != null && result.Payload != null && result.Payload.Any())
            {
                foreach (var item in result.Payload)
                {
                    members.Add(new FamilyMember()
                    {
                        avtar = item.Avatar,
                        email = item.Email,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        role = item.Roles,
                        id = item.Id
                    });
                }
            }

            var tasks = await TaskDataService.GetAllTasks();

            if (tasks != null && tasks.Payload != null && tasks.Payload.Any())
            {
                foreach (var item in tasks.Payload)
                {
                    allTasks.Add(new TaskModel()
                    {
                        text = item.text,
                        isDone = item.isDone,
                        memberId = item.memberId,
                        id = item.id
                    });
                }
            }

            leftMenuItem = new MenuItem[members.Count];
            leftMenuItem[0] = new MenuItem
            {
                label = "All Tasks",
                referenceId = Guid.Empty,
                isActive = true
            };
            leftMenuItem[0].ClickCallback += showAllTasks;
            for (int i = 1; i < members.Count; i++)
            {
                leftMenuItem[i] = new MenuItem
                {
                    iconColor = members[i - 1].avtar,
                    label = members[i - 1].firstname,
                    referenceId = members[i - 1].id,
                    isActive = false
                };
                leftMenuItem[i].ClickCallback += onItemClick;
            }
            showAllTasks(null, leftMenuItem[0]);
            isLoaded = true;
            SetFocus();
        }

        private void SetFocus()
        {
            jsRuntime.InvokeVoidAsync("binding");
            jsRuntime.InvokeVoidAsync("bindingdrop");
        }

        private void onAddItem()
        {
            showLister = false;
            showCreator = true;
            makeMenuItemActive(null);
            StateHasChanged();
        }

        private void onItemClick(object sender, object e)
        {
            val = (Guid)e.GetType().GetProperty("referenceId").GetValue(e);
            makeMenuItemActive(e);
            if (allTasks != null && allTasks.Count > 0)
            {
                tasksToShow = allTasks.Where(item =>
                {
                    if (item.member != null)
                    {
                        return item.member.id == val;
                    }
                    else
                    {
                        return false;
                    }
                }).ToArray();
            }
            showLister = true;
            showCreator = false;
            StateHasChanged();
        }
        private void showAllTasks(object sender, object e)
        {
            val = Guid.Empty;
            tasksToShow = allTasks.ToArray();
            showLister = true;
            showCreator = false;
            makeMenuItemActive(e);
            StateHasChanged();
        }

        private void makeMenuItemActive(object e)
        {
            foreach (var item in leftMenuItem)
            {
                item.isActive = false;
            }
            if (e != null)
            {
                e.GetType().GetProperty("isActive").SetValue(e, true);
            }
        }

        private void onMemberAdd()
        {
            Console.WriteLine("on member add");
        }

        protected async Task onCreateTaskClick(TaskModel taskmodel)
        {
            objTask.id = Guid.NewGuid();
            objTask.isDone = false;

            var result = await TaskDataService.CreateTask(new Domain.Commands.CreateTaskCommand()
            {
                text = taskmodel.text,
                isDone = taskmodel.isDone,
                memberId = taskmodel.memberId
            });
            if (result != null && result.Payload != null && result.Payload.id != Guid.Empty)
            {
                allTasks.Add(new TaskModel()
                {
                    text = result.Payload.text,
                    isDone = result.Payload.isDone,
                    memberId = result.Payload.memberId,
                    id = result.Payload.id
                }) ;
            }

            StateHasChanged();
            SetFocus();
            objTask = new TaskModel();

        }

        protected async Task onAssgineTask(Guid id, Guid memberId) {

            var result = await TaskDataService.UpdateTaskMember(new Domain.Commands.UpdateTaskMemberCommand()
            {
                id = id,
                memberId = memberId
            });
        }

        protected async Task onCompletedTask(Guid id, bool isDone)
        {

            var result = await TaskDataService.UpdateTaskStatus(new Domain.Commands.UpdateTaskStatusCommand()
            {
                Id = id,
                isDone = isDone
            });
        }
    }
}
