using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Features.Courses.Delete.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Courses.Delete.Command>($"courses/{Id}/delete");
        }

        protected async Task OnClickAsync()
        {
            await Client.PostJsonAsync("courses/delete", Data);
            Navigation.NavigateTo("courses");
        }
    }
}