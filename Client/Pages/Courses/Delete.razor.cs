using System.Net.Http;
using System.Net.Http.Json;
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

        protected ContosoUniversity.Shared.Features.Courses.Delete.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<ContosoUniversity.Shared.Features.Courses.Delete.Command>($"courses/{Id}/delete");
        }

        protected async Task OnClickAsync()
        {
            await Client.PostAsJsonAsync("courses/delete", Data);
            Navigation.NavigateTo("courses");
        }
    }
}