using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Students
{
    public partial class Edit
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected Features.Students.Edit.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Students.Edit.Command>($"students/{Id}/edit");
        }

        protected async Task HandleValidSubmit()
        {
            await Client.PostJsonAsync("students/edit", Data);
            Navigation.NavigateTo("students");
        }
    }
}