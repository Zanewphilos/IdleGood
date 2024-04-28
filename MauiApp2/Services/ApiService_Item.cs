using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp2.Model.ItemDto;
using Microsoft.Maui.Controls.PlatformConfiguration;
using MauiApp2.Model.UserDto;
using Azure.Storage.Blobs; 
using Microsoft.Maui.Media;
using System.IO;
using System.Net.Http.Headers;

namespace MauiApp2.Services
{
    public class ApiService_Item
    {
        private HttpClient _httpClient;

        public ApiService_Item()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // 忽略 SSL 证书验证
            _httpClient = new HttpClient(handler);

        }

        public async Task<List<ItemDto>> GetItemsYouMightLikeAsync()
        {
            var uri = new Uri("https://10.0.2.2:7012/api/Items/ItemRecommend");
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                     var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Debug.WriteLine(content);
                    try
                    {
                        var items = JsonSerializer.Deserialize<List<ItemDto>>(content, options);
                        
                        return items;
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"JSON parsing error: {ex.Message}");
                    }
               
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                Debug.WriteLine($"Unable to get items: {ex.Message}");
                Debug.WriteLine($"Exception details: {ex}");
            }
            
            return new List<ItemDto>();
        }

        public async Task<List<ItemDto>> GetItemsPopularAsync()
        {
            var uri = new Uri("https://10.0.2.2:7012/api/Items/ItemPopular");
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Debug.WriteLine(content);
                    try
                    {
                        var items = JsonSerializer.Deserialize<List<ItemDto>>(content, options);

                        return items;
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"JSON parsing error: {ex.Message}");
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                Debug.WriteLine($"Unable to get items: {ex.Message}");
                Debug.WriteLine($"Exception details: {ex}");
            }

            return new List<ItemDto>();
        }

        public async Task<List<ItemDto>> GetItemsAllAsync()
        {
            var uri = new Uri("https://10.0.2.2:7012/api/Items/ItemAll");
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Debug.WriteLine(content);
                    try
                    {
                        var items = JsonSerializer.Deserialize<List<ItemDto>>(content, options);

                        return items;
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine($"JSON parsing error: {ex.Message}");
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                Debug.WriteLine($"Unable to get items: {ex.Message}");
                Debug.WriteLine($"Exception details: {ex}");
            }

            return new List<ItemDto>();
        }

        public async Task<bool> PostItemForSaleAsync(ItemCreateDto itemCreateDto)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/Items/Create");

            var jsonRequest = JsonSerializer.Serialize(itemCreateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(uri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to post item for sale: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ItemDto>> GetMyItemsForSaleAsync()
        {
            await PrepareHttpClientAsync();
            var uri = new Uri("https://10.0.2.2:7012/api/Items/MyItems");
            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var items = JsonSerializer.Deserialize<List<ItemDto>>(content, options);
                    return items ?? new List<ItemDto>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get items: {ex.Message}");
            }

            return new List<ItemDto>();
        }

        public async Task<bool> UpdateItemDescriptionAsync(int itemId, ItemUpdateDto itemUpdateDto)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri($"https://10.0.2.2:7012/api/Items/UpdateDescription/{itemId}");
            var jsonRequest = JsonSerializer.Serialize(itemUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync(uri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to update item description: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int itemId)
        {
            await PrepareHttpClientAsync();
            var uri = new Uri($"https://10.0.2.2:7012/api/Items/Delete/{itemId}");

            try
            {
                var response = await _httpClient.DeleteAsync(uri);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to delete item: {ex.Message}");
                return false;
            }
        }



        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string fileType)
        {
            await PrepareHttpClientAsync();
            // base on the file type, upload to the corresponding container
            string containerName = fileType switch
            {
                "image" => "image",   
                "video" => "video",    
                _ => "other" 
            };

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=idlegoodstorage;AccountKey=7Hc3UOa6iCLRO9gLTWp7Xwgbc08uT4cTOBTz79w9Mmtuq5ZWR3toFBpYTmdags0+KtUZ5ARrT9Yf+AStcMAfSw==;EndpointSuffix=core.windows.net";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";
            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

            try
            {
                await blobClient.UploadAsync(fileStream, overwrite: true);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"An error occurred while uploading the file: {ex.Message}");
                return null; 
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
