using BlazorComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorComponents.Data
{
    public class ProductService
    {
        IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ProductModel>> Get() {
            string url = $"product/get";
            HttpClient client = _httpClientFactory.CreateClient(name:"WebAPI");
            HttpRequestMessage request = new(method:HttpMethod.Get,requestUri:url);
            HttpResponseMessage response = await client.SendAsync(request);

            List<ProductModel> model = await response.Content.ReadFromJsonAsync<List<ProductModel>>();

            return model;
        }
    }
}
