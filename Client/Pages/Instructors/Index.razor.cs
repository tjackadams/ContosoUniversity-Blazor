using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Model = ContosoUniversity.Features.Instructors.Index.Model;

namespace ContosoUniversity.Client.Pages.Instructors
{
    public partial class Index
    {
        public Model Data { get; private set; }

        [Inject]
        protected HttpClient Client { get; set; }
        
        protected async Task SelectInstructorAsync(int id)
        {
            string url = "instructors/index";
            url = QueryHelpers.AddQueryString(url, "id", id.ToString());

            Data = await Client.GetJsonAsync<Model>(url);
        }

        protected async Task SelectCourseAsync(int id, int courseId)
        {
            string url = "instructors/index";
            url = QueryHelpers.AddQueryString(url,
                new Dictionary<string, string> { { "id", id.ToString() }, { "courseId", courseId.ToString() } });

            Data = await Client.GetJsonAsync<Model>(url);
        }


        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Model>("instructors/index");
        }
    }
}