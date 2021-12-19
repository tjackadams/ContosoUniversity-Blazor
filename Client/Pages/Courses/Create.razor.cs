using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Create
    {
        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Department[] Departments { get; set; }

        protected ContosoUniversity.Shared.Features.Courses.Create.Command Data { get; set; } =
            new ContosoUniversity.Shared.Features.Courses.Create.Command { Department = new Department() };

        protected override async Task OnInitializedAsync()
        {
            Departments = await Client.GetFromJsonAsync<Department[]>("departments");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostAsJsonAsync("departments", Data);
            Navigation.NavigateTo("courses");
        }
    }
}