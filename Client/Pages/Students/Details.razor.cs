using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Model = ContosoUniversity.Features.Students.Details.Model;

namespace ContosoUniversity.Client.Pages.Students
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        public Model Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected ILogger<Details> Logger {get;set;} 

        protected override async Task OnInitializedAsync()
        {
            var val = await Client.GetAsync($"students/{Id}/details");
            Logger.LogDebug("Full Response: {Response}", await val.Content.ReadAsStringAsync());

            var abc = await Client.GetStringAsync($"students/{Id}/details");
            Logger.LogDebug("abc {abc}", abc);
            Console.WriteLine("result: " + abc);
            Data = await Client.GetFromJsonAsync<Model>($"students/{Id}/details");
        }
    }
}