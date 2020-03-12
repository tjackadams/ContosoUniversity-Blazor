using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace ContosoUniversity.Client.Pages.Students
{
    public partial class Index
    {
        public Features.Students.Index.Result Data { get; private set; }

        [Inject]
        protected HttpClient Client { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>("students/index");
        }

        protected async Task SearchAsync()
        {
            var url = GenerateUrl(string.Empty, string.Empty, Data.SearchString, null);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        protected async Task ResetSearchAsync()
        {
            var url = GenerateUrl(string.Empty, string.Empty, string.Empty, null);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        protected async Task SortByNameAsync()
        {
            var url = GenerateUrl(Data.NameSortParm, Data.CurrentFilter, string.Empty, null);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        protected async Task SortByDateAsync()
        {
            var url = GenerateUrl(Data.DateSortParm, Data.CurrentFilter, string.Empty, null);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        protected async Task PreviousPageAsync()
        {
            var url = GenerateUrl(Data.CurrentSort, Data.CurrentFilter, string.Empty, Data.Results.PageIndex - 1);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        protected async Task NextPageAsync()
        {
            var url = GenerateUrl(Data.CurrentSort, Data.CurrentFilter, string.Empty, Data.Results.PageIndex + 1);
            Data = await Client.GetJsonAsync<Features.Students.Index.Result>(url);
        }

        private static string GenerateUrl(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            const string url = "students/index";
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(currentFilter)) { queryParams["currentFilter"] = currentFilter; }

            if (pageIndex.HasValue) { queryParams["page"] = pageIndex.Value.ToString(); }

            if (!string.IsNullOrEmpty(searchString)) { queryParams["searchString"] = searchString; }

            if (!string.IsNullOrEmpty(sortOrder)) { queryParams["sortOrder"] = sortOrder; }

            return QueryHelpers.AddQueryString(url, queryParams);
        }
    }
}