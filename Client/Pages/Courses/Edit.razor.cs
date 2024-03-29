﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        public Department[] Departments { get; set; }

        public ContosoUniversity.Shared.Features.Courses.Edit.Command Data { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        public async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("courses/edit", Data);

            Navigation.NavigateTo("courses");
        }

        protected override async Task OnInitializedAsync()
        {
            Departments = await Client.GetFromJsonAsync<Department[]>("departments");
            Data = await Client.GetFromJsonAsync<ContosoUniversity.Shared.Features.Courses.Edit.Command>($"courses/{Id}/edit");
        }
    }
}