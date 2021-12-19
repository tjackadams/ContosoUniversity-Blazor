using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Create
    {
        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Instructor[] Administrators { get; set; }

        protected ContosoUniversity.Shared.Features.Departments.Create.Command Data { get; set; } =
            new ContosoUniversity.Shared.Features.Departments.Create.Command { Administrator = new Instructor() };

        protected override async Task OnInitializedAsync()
        {
            Administrators = await Client.GetFromJsonAsync<Instructor[]>("instructors");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("departments/create", Data);
            Navigation.NavigateTo("departments");
        }
    }
}