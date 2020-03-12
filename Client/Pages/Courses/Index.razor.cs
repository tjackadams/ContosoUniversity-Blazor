using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ContosoUniversity.Client.Pages.Courses
{
    public partial class Index
    {
        public Features.Courses.Index.Result Data { get; private set; }

        [Inject]
        protected HttpClient Client { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Courses.Index.Result>("courses/index");
        }
    }
}