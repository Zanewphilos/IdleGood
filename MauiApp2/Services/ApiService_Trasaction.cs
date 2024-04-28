using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MauiApp2.Model.TransactionDto;
using System.Net.Http.Headers;

namespace MauiApp2.Services
{
    public class ApiService_Transaction
    {
        private HttpClient _httpClient;

        public ApiService_Transaction()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // 忽略 SSL 证书验证
            _httpClient = new HttpClient(handler);

        }

        public async Task<List<TransactionHistoryDto>> GetUserTransactionsAsync()
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/Transaction/GetUserHistory");
            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var transactions = JsonSerializer.Deserialize<List<TransactionHistoryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return transactions ?? new List<TransactionHistoryDto>();
            }
            else
            {
                Debug.WriteLine("Failed to fetch user transactions");
                return new List<TransactionHistoryDto>();
            }
        }

        public async Task<bool> PostTransactionAsync(TransactionCreateDto model)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/Transaction/Create");
            var jsonRequest = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int id, TransactionStatusUpdateDto model)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri($"https://10.0.2.2:7012/api/Transaction/Status/{id}");
            var jsonRequest = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserTransactionAsync(int id)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri($"https://10.0.2.2:7012/api/Transaction/Delete{id}");
            var response = await _httpClient.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
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
