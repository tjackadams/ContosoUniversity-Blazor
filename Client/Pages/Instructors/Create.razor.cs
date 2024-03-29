﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Features.Instructors;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Instructors
{
    public partial class Create
    {
        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected CreateEdit.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<CreateEdit.Command>("instructors/create");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("instructors/create", Data);
            Navigation.NavigateTo("instructors");
        }
    }
}