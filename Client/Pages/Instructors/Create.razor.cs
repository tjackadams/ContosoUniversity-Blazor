using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Features.Instructors;
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
            Data = await Client.GetJsonAsync<CreateEdit.Command>("instructors/create");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("instructors/create", Data);
            Navigation.NavigateTo("instructors");
        }
    }
}