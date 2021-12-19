using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Instructors
{
    public partial class Delete
    {
        [Parameter]
        public int Id { get; set; }


        public ContosoUniversity.Shared.Features.Instructors.Delete.Command Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<ContosoUniversity.Shared.Features.Instructors.Delete.Command>($"instructors/{Id}/delete");
        }

        protected async Task OnClickAsync()
        {
            await Client.PostAsJsonAsync("instructors/delete", Data);
            Navigation.NavigateTo("instructors");
        }
    }
}