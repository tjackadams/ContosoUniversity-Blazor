using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Students
{
    public partial class Create
    {
        public Features.Students.Create.Command Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Data = new Features.Students.Create.Command();
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("students/create", Data);
            Navigation.NavigateTo("students");
        }
    }
}