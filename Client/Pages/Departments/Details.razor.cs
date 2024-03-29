﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Model = ContosoUniversity.Shared.Features.Departments.Details.Model;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        public Model Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<Model>($"departments/{Id}/details");
        }
    }
}