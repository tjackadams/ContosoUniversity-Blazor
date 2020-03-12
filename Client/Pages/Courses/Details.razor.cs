using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        public Features.Courses.Details.Command Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Courses.Details.Command>($"courses/{Id}/details");
        }
    }
}