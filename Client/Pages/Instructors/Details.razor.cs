using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Model = ContosoUniversity.Features.Instructors.Details.Model;

namespace ContosoUniversity.Client.Pages.Instructors
{
    public partial class Details
    {
        [Parameter]
        public int Id { get; set; }

        public Model Data { get; set; }

        [Inject]
        protected HttpClient Client { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<Model>($"instructors/{Id}/details");
        }
    }
}