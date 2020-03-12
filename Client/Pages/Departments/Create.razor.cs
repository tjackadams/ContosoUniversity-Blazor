﻿using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Create
    {
        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Instructor[] Administrators { get; set; }

        protected Features.Departments.Create.Command Data { get; set; } =
            new Features.Departments.Create.Command { Administrator = new Instructor() };

        protected override async Task OnInitializedAsync()
        {
            Administrators = await Client.GetJsonAsync<Instructor[]>("instructors");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("departments/create", Data);
            Navigation.NavigateTo("departments");
        }
    }
}