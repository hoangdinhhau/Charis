using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Common;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Charis.WebApp.Services.UserService
{
    public class UserClientService : IUserClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/User/login", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<ApiResult<bool>> CreateUser(UserCreateRequest equest)
        {
            var json = JsonConvert.SerializeObject(equest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/User", httpContent);
            var user = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(user);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(user);
        }

        public async Task<ApiResult<UserViewModel>> GetByEmail(string email)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.GetAsync($"/api/user/email/{email}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserViewModel>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<UserViewModel>>(body);
        }
    }
}