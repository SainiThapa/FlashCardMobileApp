using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

public class AuthenticationService
{
    private const string TokenKey = "AuthToken";
    private const string EmailKey = "UserEmail";

    public async Task<bool> LoginAsync(string email, string password)
    {
        var client = new HttpClient();
        var loginUrl = "http://192.168.1.84:5000/api/AccountApi/login";
        var content = new StringContent(JsonConvert.SerializeObject(new { Email = email, Password = password }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(loginUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            string token = result["token"];

            await SecureStorage.SetAsync(TokenKey, token);
            await SecureStorage.SetAsync(EmailKey, email);
            Debug.WriteLine("Stored Token: " + token);

            return true;
        }
        return false;
    }
    public async Task<bool> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        try
        {
            var registerUrl = "http://192.168.1.84:5000/api/AccountApi/register";
            var registerData = new
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            var json = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync(registerUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Registration successful.");
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Registration failed: {errorMessage}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RegisterAsync Exception: {ex.Message}");
            return false;
        }
    }
    public async Task<string> GetUserEmailAsync()
    {
        return await SecureStorage.GetAsync("UserEmail");
    }


    public async Task<bool> IsTokenValidAsync()
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
            return false; // No token found

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken.ValidTo > DateTime.UtcNow.AddMinutes(5);
        }
        catch (Exception)
        {
            return false; // Invalid token format
        }
    }

    public async Task RefreshTokenAsync(string email, string password)
    {
        await LoginAsync(email, password); // Re-login to get a new token
    }

    public void Logout()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(EmailKey);
    }
    public async Task<bool> IsLoggedIn()
    {
        var token = await SecureStorage.GetAsync("AuthToken");
        return !string.IsNullOrEmpty(token);
    }

}
