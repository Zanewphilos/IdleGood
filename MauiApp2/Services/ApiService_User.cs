using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MauiApp2.Model.UserDto;
using System.Net.Http.Headers;

namespace MauiApp2.Services
{
    public class ApiService_User
    {
        private HttpClient _httpClient;

        public ApiService_User()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // ignore SSL certificate validation
            _httpClient = new HttpClient(handler);

        }
        public async Task<(bool IsSuccess, string ErrorMessage)> LoginAsync(string email, string password)
        {
            var uri = new Uri("https://10.0.2.2:7012/api/User/Login");

            var loginRequest = new LoginDto
            {
                Email = email,
                Password = password
            };

            var jsonRequest = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);

                    if (responseObject.TryGetValue("token", out var token) && responseObject.TryGetValue("userId", out var userId))
                    {
                        await SecureStorage.SetAsync("auth_token", token);
                        await SecureStorage.SetAsync("user_id", userId);
                        return (true, string.Empty); // 登录成功，没有错误消息
                    }
                    else
                    {
                        return (false, "Token or userId not found in response."); // 登录失败，返回错误消息
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, errorContent); // 登录失败，返回错误内容
                }
            }
            catch (Exception ex)
            {
                return (false, $"Exception in login: {ex.Message}");
            }
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var uri = new Uri("https://10.0.2.2:7012/api/User/register");

            var registrationRequest = new UserRegistrationDto
            {
                Username = username,
                Email = email,
                Password = password
            };

            var jsonRequest = JsonSerializer.Serialize(registrationRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Registration successful");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Registration failed: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Exception in registration: {ex.Message}");
                return false;
            }
        }


        public async Task<UserDto> GetUserProfileAsync()
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/User/UserInfo");
            var authToken = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Trim(new char[] { '{', '}' }));
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // This makes the deserializer not case-sensitive
                    };
                    var userDto = JsonSerializer.Deserialize<UserDto>(content, options);
                    return userDto;
                }
                else
                {
                    // Handle error response
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error fetching user profile: {errorContent}");
                }
            }
            else
            {
                // Handle case where authToken is null or empty
                Debug.WriteLine("Auth token is not available.");
            }
            return null;
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/User/Update");
            var jsonRequest = JsonSerializer.Serialize(userUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User updated successfully.");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"User update failed: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync()
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/User/Delete");

            try
            {
                var response = await _httpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("User deleted successfully.");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"User deletion failed: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in deleting user: {ex.Message}");
                return false;
            }
        }
        private async Task PrepareHttpClientAsync()
        {
            var authToken = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
