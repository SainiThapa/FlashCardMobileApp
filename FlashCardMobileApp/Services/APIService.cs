using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Xamarin.Forms;

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
                Debug.WriteLine("Admin token retrieved: " + token);

            }
            else
            {
                token = await SecureStorage.GetAsync("AuthToken");
                Debug.WriteLine("User token retrieved: " + token);
            }

            if (string.IsNullOrEmpty(token))
            {
                Debug.WriteLine("Token not found: ");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }    

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
                var requestdata = new
                {
                    Email = profile.Email,
                    Username = profile.Username
                };                
                var requestUrl = $"Accountapi/users/update";
                var json = JsonConvert.SerializeObject(requestdata);
                Debug.WriteLine($"JSON DATA {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Debug.WriteLine("[Content Created] Successfully created JSON content.");

                var request = await CreateAuthenticatedRequest(HttpMethod.Put, requestUrl);
                Debug.WriteLine("[Authenticated Request] Request created successfully.");


                request.Content = content;
                Debug.WriteLine("[Request] Content assigned to request.");

                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine($"[Response Status] {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Profile updated successfully");
                    return true;
                }
                return false;
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
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, "Flashcardapi/categories");
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

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var requestUrl = "accountapi/forgot-password";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);
            return response.IsSuccessStatusCode;
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

        public async Task<List<Flashcard>> GetUserFlashcardAsync(string userId, int flashcardId)
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

        public async Task<bool> CreateFlashcardAsync(string userId, CreateUserCardViewModel model)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards";

                var requestData = new
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    CategoryId = model.SelectedCategory.Id
                };
                var json = JsonConvert.SerializeObject(requestData);
                Debug.WriteLine($"[CreateFlashcardAsync] Sending Data: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = await CreateAuthenticatedRequest(HttpMethod.Post, requestUrl);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[CreateFlashcardAsync] Error Response: {errorResponse}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CreateFlashcardAsync] Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"[CreateFlashcardAsync] Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        public async Task<bool> UpdateUserFlashcardAsync(string userId, int flashcardId, UpdateFlashCardViewModel model)
        {
            try
            {
                var requestUrl = $"adminapi/users/{userId}/flashcards/{flashcardId}";

                var request = await CreateAuthenticatedRequest(HttpMethod.Put, requestUrl, model);

                var requestData = new
                {
                    Question = model.Question,
                    Answer = model.Answer,
                    CategoryId = model.CategoryId
                };
                var json = JsonConvert.SerializeObject(requestData);
                Debug.WriteLine($"[CreateFlashcardAsync] Sending Data:{userId} : {json}");

                var response = await _httpClient.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateUserFlashcardAsync] Exception: {ex.Message}");
                return false;
            }
        }
        public async Task<Flashcard> GetFlashcardByIdAsync(int flashcardId)
        {
            try
            {
                var requestUrl = $"adminapi/users/flashcards/{flashcardId}";

                var request = await CreateAuthenticatedRequest(HttpMethod.Get, requestUrl);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Flashcard>(content);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetFlashcardByIdAsync] Error: {ex.Message}");
                return null;
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

        // Delete users in bulk
        public async Task<bool> DeleteUsersAsync(List<string> userIds)
        {
            try
            {
                Debug.WriteLine("Delete users async in bulk");
                Debug.WriteLine($"User IDs to delete: {string.Join(", ", userIds)}");

                var requestUrl = "adminapi/bulk-delete-users";
                var payload = new { userIds = userIds }; 
                //var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                //Debug.WriteLine($"Request body: {JsonConvert.SerializeObject(payload)}");

                var request = await CreateAuthenticatedRequest(HttpMethod.Post, requestUrl, payload);

                var response = await _httpClient.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"Bulk Delete Response: {response.StatusCode}");
                Debug.WriteLine($"Response Body: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Delete request successful");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Delete request failed with status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in DeleteUsersAsync: {ex.Message}");
                return false;
            }
        }
        // User Flashcards Summary 
        public async Task<bool> DownloadUserFlashCardsSummaryAsync()
        {
            return await DownloadFileAsync("adminapi/download-user-flashcards", "UserFlashCardsSummary.csv");
        }

        // All Flashcards with Owners 
        public async Task<bool> DownloadAllFlashCardsWithOwnersAsync()
        {
            return await DownloadFileAsync("adminapi/download-all-flashcards", "AllFlashCardsWithOwners.csv");
        }

        // Common method to download files
        private async Task<bool> DownloadFileAsync(string endpoint, string fileName)
        {
            try
            {
                var request = await CreateAuthenticatedRequest(HttpMethod.Get, endpoint);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                    File.WriteAllBytes(filePath, content);

                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(filePath)
                    });

                    return true;
                }
                else
                {
                    Debug.WriteLine($"Download failed. Status: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to download the file.", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                return false;
            }
        }

        // Create Authenticated HTTP Request
          public async Task<bool> IsLoggedIn()
        {
            var token = await SecureStorage.GetAsync("auth_token") ?? await SecureStorage.GetAsync("admin_auth_token");
            return !string.IsNullOrEmpty(token);
        }


    }

}
