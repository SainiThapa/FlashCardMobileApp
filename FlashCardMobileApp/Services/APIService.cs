using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.ViewModels;
using FlashCardMobileApp.ViewModels.Admin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace FlashCardMobileApp.Services
{
    public class TokenResponse
    {
        public string Token { get; set; }
    }

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
            string token;

            if (url.StartsWith("Adminapi", StringComparison.OrdinalIgnoreCase) || url.Contains("admin"))
            {
                token = await SecureStorage.GetAsync("admin_auth_token");
            }
            else
            {
                token = await SecureStorage.GetAsync("auth_token");
            }

            Debug.WriteLine("TOKEN GENERATED : " + token);
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
                var requestUrl = $"adminapi/user/{email}/updatePassword";
                var passwordModel = new { Password = newPassword };
                var json = JsonConvert.SerializeObject(passwordModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = await CreateAuthenticatedRequest(HttpMethod.Put, requestUrl);

                request.Content = content;
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    // Log the success message
                    Console.WriteLine("Password updated successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdatePassword Exception: {ex.Message}");
                return false;
            }
            return false;
        }

        public async Task<bool> AdminLoginAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("accountapi/admin/login", content);


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Received JSON: " + json);
                try
                {
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
                    if (tokenResponse?.Token != null)
                    {
                        await SecureStorage.SetAsync("admin_auth_token", tokenResponse.Token);
                        Debug.WriteLine("ADMIN SUCCESSFUL LOGIN WITH TOKEN");
                        Debug.WriteLine(tokenResponse.Token);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Deserialization Error: {ex.Message}");
                }
            }
            return false;
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
        public async Task<UserDetailViewModel> GetUserDetailAsync(string userId)
        {
            try
            {
                Debug.WriteLine($"The userid is being carried: {userId} is the userid");

                var request = await CreateAuthenticatedRequest(HttpMethod.Get, $"adminapi/Users/{Uri.EscapeDataString(userId)}/details");


                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully fetched user details: ");

                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response: " + json);

                    return JsonConvert.DeserializeObject<UserDetailViewModel>(json);
                }
                else
                {
                    Debug.WriteLine($"Failed to fetch the details. Status code: {response.StatusCode}");
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Error Response: " + errorResponse);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user details: {ex.Message}");
            }
            return null;
        }


        public async Task<Models.Flashcard[]> GetFlashcardsAsync()
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
                    return parsedResponse["$values"].ToObject<Models.Flashcard[]>(); // Extract array properly
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
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, "adminapi/categories");
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


        public async Task<bool> UpdateFlashcardAsync(Models.Flashcard flashcard)
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
                Debug.WriteLine("No token found. User is not logged in.");

            var isValid = await _authService.IsTokenValidAsync();
            Debug.WriteLine($"Is Token Valid: {isValid}");
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            Debug.WriteLine("Called the getusersasync method");

            try
            {
                var requestUrl = "Adminapi/users";
                Debug.WriteLine("Calling the user page");
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, requestUrl);
                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine($" Called the GetUserAsync: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($" Success to fetch users: {response.StatusCode}");
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<UserViewModel>>(json);
                }
                else
                {
                    Debug.WriteLine($" Failed to fetch users: {response.StatusCode}");
                    return new List<UserViewModel>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetUsersAsync: {ex.Message}");
                return new List<UserViewModel>();
            }
        }

        // ➤ Delete user
        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}";
                var request = await CreateAuthenticatedRequest(HttpMethod.Delete, requestUrl);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"User {userId} deleted successfully.");
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Failed to delete user: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Exception in DeleteUserAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> BulkDeleteFlashcardsAsync(string userId, List<int> flashcardIds)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards/bulk-delete";
                var request = await CreateAuthenticatedRequest(HttpMethod.Post, requestUrl);

                var json = JsonConvert.SerializeObject(new { FlashcardIds = flashcardIds });
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ {flashcardIds.Count} flashcards deleted successfully.");
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Failed to delete flashcards: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Exception in BulkDeleteFlashcardsAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            var requestUrl = "adminapi/categories";
            var request = await CreateAuthenticatedRequest(HttpMethod.Post, requestUrl);

            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            var requestUrl = $"adminapi/categories/{category.Id}";
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Create the authenticated request
            var request = await CreateAuthenticatedRequest(HttpMethod.Put, requestUrl);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }


        // Delete Category
        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            try
            {

            Debug.WriteLine("Deleted Category");

            var requestUrl = $"adminapi/categories/{category.Id}";
            var request = await CreateAuthenticatedRequest(HttpMethod.Delete, requestUrl);
            Debug.WriteLine($"Request URL: {requestUrl}");


                var response = await _httpClient.SendAsync(request);
            Debug.WriteLine($"Response Status Code: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in DeleteCategoryAsync: {e.Message}");
                return false;
            }
        }

        public async Task<bool> IsAdmin()
        {
            try
            {
                var token = await SecureStorage.GetAsync("admin_auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                var response = await _httpClient.GetAsync("api/accountapi/is-admin");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking admin status: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Flashcard>> GetUserFlashcardsAsync(string userId)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards";
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, requestUrl);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Flashcard>>(json);
                }
                else
                {
                    Debug.WriteLine($"Failed to fetch flashcards: {response.StatusCode}");
                    return new List<Flashcard>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetUserFlashcardsAsync: {ex.Message}");
                return new List<Flashcard>();
            }
        }

        public async Task<bool> CreateFlashcardAsync(string userId, CreateFlashcardViewModel model)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards";
                var request = await CreateAuthenticatedRequest(HttpMethod.Post, requestUrl, model);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in CreateFlashcardAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateFlashcardAsync(string userId, int flashcardId, EditFlashcardViewModel model)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards/{flashcardId}";
                var request = await CreateAuthenticatedRequest(HttpMethod.Put, requestUrl, model);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in UpdateFlashcardAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFlashcardAsync(string userId, int flashcardId)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards/{flashcardId}";
                var request = await CreateAuthenticatedRequest(HttpMethod.Delete, requestUrl);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in DeleteFlashcardAsync: {ex.Message}");
                return false;
            }
        }



        public async Task<bool> IsLoggedIn()
        {
            var token = await SecureStorage.GetAsync("auth_token") ?? await SecureStorage.GetAsync("admin_auth_token");
            return !string.IsNullOrEmpty(token);
        }


    }
}
