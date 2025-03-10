using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FlashCardMobileApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace FlashCardMobileApp.Services
{
    public class ApiService
    {
        private const string TokenKey = "AuthToken";

        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.1.84:5000/api/")
            };
            _authService = new AuthenticationService();
        }

        private async Task<HttpRequestMessage> CreateAuthenticatedRequest(HttpMethod method, string url, object body = null)
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("User is not authenticated.");

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return request;
        }
        public async Task<bool> UpdatePasswordAsync(string email, string newPassword)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Put, $"AccountApi/user/{email}/updatePassword", new { Password = newPassword });
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdatePassword Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<UserProfile> GetUserProfileAsync()
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, "AccountApi/profile");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<UserProfile>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserProfile Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfile profile)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Put, "AccountApi/updateProfile", profile);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateUserProfile Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<Flashcard[]> GetFlashcardsAsync()
        {
            try
            {
                Debug.WriteLine("GetFlashCardAsync trigerred");

                var token = await GetTokenAsync();
                Debug.WriteLine($"Token Retrieved: {token}");

                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("Error: No valid token found. User must log in again.");
                    await SecureStorage.SetAsync("AuthToken", ""); // Clear invalid token
                    return null;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, "FlashcardAPI/list");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"API Response: {jsonResponse}");

                var parsedResponse = JsonConvert.DeserializeObject<JObject>(jsonResponse);

                if (parsedResponse != null && parsedResponse["$values"] != null)
                {
                    return parsedResponse["$values"].ToObject<Flashcard[]>(); // Extract array properly
                }

                Debug.WriteLine("Error: No flashcards found in response.");
                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetFlashcards Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, "FlashCardAPI/categories");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<List<Category>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCategories Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<string> GetUserIdAsync()
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, "AccountApi/userId");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var userId = await response.Content.ReadAsStringAsync();
                return userId.Trim('"'); // Remove extra quotes if returned as a string
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserIdAsync Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddFlashcardAsync(object flashcard)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Post, "FlashCardAPI/create", flashcard);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddFlashcard Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateFlashcardAsync(Flashcard flashcard)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Put, $"FlashCardAPI/update/{flashcard.Id}", new
                {
                    CategoryId = flashcard.CategoryId,
                    Question = flashcard.Question,
                    Answer = flashcard.Answer
                });

                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateFlashcardAsync Exception: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteFlashcardAsync(int flashcardId)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Delete, $"FlashCardAPI/delete/{flashcardId}");
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteFlashcard Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> BulkDeleteFlashcardsAsync(List<int> flashcardIds)
        {
            try
            {
                if (flashcardIds == null || !flashcardIds.Any())
                {
                    Debug.WriteLine("BulkDeleteFlashcardsAsync: No IDs to delete.");
                    return false;
                }

                Debug.WriteLine("BulkDeleteFlashcardsAsync: Sending request with IDs - " + string.Join(", ", flashcardIds));

                var request = await CreateAuthenticatedRequest(HttpMethod.Post, $"FlashCardAPI/bulk-delete", flashcardIds);
                var response = await _httpClient.SendAsync(request);

                string responseBody = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"Bulk Delete Response: {response.StatusCode}");
                Debug.WriteLine($"Response Body: {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BulkDeleteFlashcards Exception: {ex.Message}");
                return false;
            }
        }
        public async Task<Flashcard> GetRandomFlashcardAsync(int? categoryId = null)
        {
            try
            {
                var url = categoryId.HasValue ? $"FlashCardAPI/quiz/random?CategoryId={categoryId}" : "FlashCardAPI/quiz/random";
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, url);
                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Flashcard>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRandomFlashcard Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        public async Task DebugTokenAsync()
        {
            var token = await GetTokenAsync();
            Console.WriteLine($"Token: {token}");

            if (string.IsNullOrEmpty(token))
                Console.WriteLine("⚠️ No token found. User is not logged in.");

            var isValid = await _authService.IsTokenValidAsync();
            Console.WriteLine($"Is Token Valid: {isValid}");
        }


    }
}
