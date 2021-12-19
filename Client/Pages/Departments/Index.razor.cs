using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Departments
{
    public partial class Index
    {
        public ContosoUniversity.Shared.Features.Departments.Index.Result Data { get; private set; }

        [Inject]
        protected HttpClient Client { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetFromJsonAsync<ContosoUniversity.Shared.Features.Departments.Index.Result>("departments/index");
        }
    }
}