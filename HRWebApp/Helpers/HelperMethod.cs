using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using HRWebApp.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace HRWebApp.Helpers
{
    public class HelperMethod
    {
        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri("http://localhost:44323")};
            return client;
        }

        public static StringContent GetStringOfObject(Object obj)
        {
            var stringContent = new StringContent(
                JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json");
            return stringContent;
        }
        
        public static User GetUser(int id)
        {
            var client = GetHttpClient();
            var response = client.GetAsync($"api/user/get/{id}").Result;
            var httpResponseContent = response.Content.ReadAsStringAsync().Result;
            var user = new JavaScriptSerializer().Deserialize<User>(httpResponseContent);
            return user;
        }
        public static Vacation GetVacation(string id)
        {
            var client = GetHttpClient();
            var response = client.GetAsync($"api/vacation/get/{id}").Result;
            var httpResponseContent = response.Content.ReadAsStringAsync().Result;
            var vacation = new JavaScriptSerializer().Deserialize<Vacation>(httpResponseContent);
            return vacation;
        }

        public static bool UpdateUser(User user)
        {
            var client = GetHttpClient();
            var stringContent = GetStringOfObject(user);
            var response = client.PutAsync("api/user/Update", stringContent).Result;
            return response.IsSuccessStatusCode;
        }

        public static IEnumerable<UserView> GetUsers(string url)
        {
            var client = GetHttpClient();
            var httpResponse = client.GetAsync(url).Result;
            var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
            var user = new JavaScriptSerializer().Deserialize<IEnumerable<UserView>>(httpResponseContent);
            return user;
        }

        public static IEnumerable<Vacation> GetUserVacation(string url)
        {
            var client = GetHttpClient();
                var httpResponse = client.GetAsync(url).Result;
                var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
                var vacations = new JavaScriptSerializer().Deserialize<IEnumerable<Vacation>>(httpResponseContent);
                return vacations;
        }
        public static IEnumerable<EmployeeVacation> GetEmployeeVacation(string url)
        {
            var client = GetHttpClient();
            var httpResponse = client.GetAsync(url).Result;
            var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
            var vacations = new JavaScriptSerializer().Deserialize<IEnumerable<EmployeeVacation>>(httpResponseContent);
            return vacations;
        }

    }
}