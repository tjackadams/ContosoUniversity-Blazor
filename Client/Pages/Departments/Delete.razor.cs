using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Features.Departments.Delete.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<Features.Departments.Delete.Command>($"departments/{Id}/delete");
        }

        protected async Task OnClickAsync()
        {
            await Client.PostAsJsonAsync("departments/delete", Data);
            Navigation.NavigateTo("courses");
        }
    }
}