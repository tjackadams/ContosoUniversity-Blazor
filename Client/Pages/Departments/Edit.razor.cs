using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Domain.UniversityAggregate;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        public Features.Departments.Edit.Command Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Instructor[] Administrators { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Administrators = await Client.GetJsonAsync<Instructor[]>("instructors");

            Data = await Client.GetJsonAsync<Features.Departments.Edit.Command>($"departments/{Id}/edit");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("departments/edit", Data);
            Navigation.NavigateTo("departments");
        }
    }
}