﻿using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Create
    {
        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Department[] Departments { get; set; }

        protected Features.Courses.Create.Command Data { get; set; } =
            new Features.Courses.Create.Command { Department = new Department() };

        protected override async Task OnInitializedAsync()
        {
            Departments = await Client.GetJsonAsync<Department[]>("departments");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("departments", Data);
            Navigation.NavigateTo("courses");
        }
    }
}