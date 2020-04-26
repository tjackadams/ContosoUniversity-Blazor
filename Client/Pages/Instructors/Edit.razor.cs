using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContosoUniversity.Features.Instructors;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Instructors
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected CreateEdit.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<CreateEdit.Command>($"instructors/{Id}/edit");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("instructors/edit", Data);
            Navigation.NavigateTo("instructors");
        }
    }
}