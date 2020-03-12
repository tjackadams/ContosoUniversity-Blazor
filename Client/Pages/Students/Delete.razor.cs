using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Students
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }


        public Features.Students.Delete.Command Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Students.Delete.Command>($"students/{Id}/delete");
        }

        protected async Task OnClickAsync()
        {
            await Client.PostJsonAsync("students/delete", Data);
            Navigation.NavigateTo("students");
        }
    }
}