using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        public Department[] Departments { get; set; }

        public Features.Courses.Edit.Command Data { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        public async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("courses/edit", Data);

            Navigation.NavigateTo("courses");
        }

        protected override async Task OnInitializedAsync()
        {
            Departments = await Client.GetJsonAsync<Department[]>("departments");
            Data = await Client.GetJsonAsync<Features.Courses.Edit.Command>($"courses/{Id}/edit");
        }
    }
}