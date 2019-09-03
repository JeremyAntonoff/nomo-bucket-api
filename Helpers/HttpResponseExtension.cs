using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NomoBucket.API.Helpers
{
    public static class HttpResponseExtension
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalCount, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalCount, totalPages);
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, jsonSettings));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}