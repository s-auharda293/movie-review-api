using Azure;
using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests
{
    public class AuthTest : IClassFixture<MovieReviewWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public AuthTest(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task Register_WithValidUser_ReturnsSuccess()
        {
            // Arrange
            var registerRequest = new
            {
                request = new{ 
                FirstName = "Test",
                LastName = "User",
                Email = $"user_{Guid.NewGuid()}@test.com",
                Password = "StrongPass123!"
            }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            var json = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Register Response: {json}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("success", json, StringComparison.OrdinalIgnoreCase);

            // Get the refresh token from Set-Cookie header
            var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
            Assert.NotNull(setCookieHeader);
            Assert.Contains("refreshToken", setCookieHeader);

            // Extract the refresh token value
            var cookieParts = setCookieHeader.Split(';');
            var refreshTokenPart = cookieParts.FirstOrDefault(c => c.Trim().StartsWith("refreshToken="));
            Assert.NotNull(refreshTokenPart);

            var refreshTokenValue = refreshTokenPart.Split('=')[1];

            _output.WriteLine($"Registered User: {JsonSerializer.Serialize(response)}, Refresh Token: {refreshTokenValue}");
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ReturnsTokens()
        {
            //Arrange
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Login",
                    LastName = "Tester",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            // Act
            var loginRequest = new
            {
                request = new { 
                Email = email,
                Password = password
                }
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Login Response: {loginJson}");

            // Assert
            loginResponse.EnsureSuccessStatusCode();
            Assert.Contains("accessToken", loginJson, StringComparison.OrdinalIgnoreCase);

            var setCookieHeader = loginResponse.Headers.GetValues("Set-Cookie").FirstOrDefault();
            Assert.NotNull(setCookieHeader);
            Assert.Contains("refreshToken", setCookieHeader);

            var cookieParts = setCookieHeader.Split(';');
            var refreshTokenPart = cookieParts.FirstOrDefault(c => c.Trim().StartsWith("refreshToken="));
            Assert.NotNull(refreshTokenPart);

            var refreshTokenValue = refreshTokenPart.Split('=')[1];
            _output.WriteLine($"Refresh Token: {refreshTokenValue}");
        }


        [Fact]
        public async Task GetCurrentUser_WithValidAccessToken_ReturnsUserDetails()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";
            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Profile",
                    LastName = "Tester",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var loginRequest = new
            { 
                request = new{ 
                    Email = email,
                    Password = password
            }
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Login Response: {loginJson}");

            // Extract access token 
            using var loginDoc = JsonDocument.Parse(loginJson);
            var accessToken = loginDoc.RootElement.GetProperty("value").GetProperty("accessToken").GetString();
            Assert.False(string.IsNullOrEmpty(accessToken));

            // Act 
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var meResponse = await _client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"GetCurrentUser Response: {meJson}");

            // Assert
            meResponse.EnsureSuccessStatusCode();
            Assert.Contains(email, meJson, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("ProfileTester", meJson, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsUpdatedUser()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Update",
                    LastName = "Tester",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

          
            var loginRequest = new {request = new { Email = email, Password = password } };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = System.Text.Json.JsonDocument.Parse(loginJson);
            var accessToken = loginResult.RootElement.GetProperty("value").GetProperty("accessToken").GetString();
            var id = loginResult.RootElement.GetProperty("value").GetProperty("id");

            // Act 
            var updateRequest = new
            {
                id = id,
                request = new
                {
                    FirstName = "UpdatedFirstName",
                    LastName = "UpdatedLastName"
                }
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Put, "/api/auth")
            {
                Content = JsonContent.Create(updateRequest)
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {accessToken}");

            var updateResponse = await _client.SendAsync(httpRequest);
            updateResponse.EnsureSuccessStatusCode();

            var updatedJson = await updateResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"UpdateUser Response: {updatedJson}");

            // Assert
            Assert.Contains("UpdatedFirstName", updatedJson);
            Assert.Contains("UpdatedLastName", updatedJson);
        }

        [Fact]
        public async Task ChangePassword_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "OldPass123!";
            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Change",
                    LastName = "Password",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

          
            var loginRequest = new { request = new { Email = email, Password = password } };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var loginData = JsonSerializer.Deserialize<JsonElement>(loginJson);
            var accessToken = loginData.GetProperty("value").GetProperty("accessToken").GetString();
            Assert.NotNull(accessToken);

            // Act 
            var changePasswordRequest = new
            {
                request = new
                {
                    Email = email,
                    CurrentPassword = password,
                    NewPassword = "NewStrongPass123!"
                }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/auth/change-password")
            {
                Content = JsonContent.Create(changePasswordRequest)
            };
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            var response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Change Password Response: {responseJson}");

            // Assert 
            Assert.Contains("success", responseJson, StringComparison.OrdinalIgnoreCase);

            Assert.Contains("Password changed successfully", responseJson, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AssignRole_AdminCanAssignRoleToNewUser()
        {
            // Arrange
            var newUserEmail = $"user_{Guid.NewGuid()}@test.com";
            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Role",
                    LastName = "User",
                    Email = newUserEmail,
                    Password = "UserPass123!"
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();
            var registerJson = await registerResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Registered User Response: {registerJson}");

            
            var registerData = JsonSerializer.Deserialize<JsonElement>(registerJson);
            var newUserId = registerData.GetProperty("value").GetProperty("id").GetGuid();

            
            var adminLoginRequest = new
            {
                request = new
                {
                    Email = "admin@movies.com",
                    Password = "Admin@123"
                }
            };

            var adminLoginResponse = await _client.PostAsJsonAsync("/api/auth/login", adminLoginRequest);
            adminLoginResponse.EnsureSuccessStatusCode();
            var loginJson = await adminLoginResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Admin Login Response: {loginJson}");

            var loginData = JsonSerializer.Deserialize<JsonElement>(loginJson);
            var adminAccessToken = loginData.GetProperty("value").GetProperty("accessToken").GetString();
            Assert.NotNull(adminAccessToken);

            
            var assignRoleRequest = new
            {
                dto = new
                {
                    UserId = newUserId,
                    Role = "Admin"
                }
            };

            // Act
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/auth/assign-role")
            {
                Content = JsonContent.Create(assignRoleRequest)
            };
            requestMessage.Headers.Add("Authorization", $"Bearer {adminAccessToken}");

            var assignResponse = await _client.SendAsync(requestMessage);
            assignResponse.EnsureSuccessStatusCode();

            var assignJson = await assignResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Assign Role Response: {assignJson}");

            // Assert
            Assert.Contains("Role assigned successfully", assignJson, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task RevokeRefreshToken_WithValidCookieAndAccessToken_ReturnsSuccess()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new { 
                FirstName = "Revoke",
                LastName = "Tester",
                Email = email,
                Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var setCookieHeader = registerResponse.Headers.GetValues("Set-Cookie").FirstOrDefault();
            Assert.NotNull(setCookieHeader);
            var refreshTokenPart = setCookieHeader.Split(';')
                .First(c => c.TrimStart().StartsWith("refreshToken=", StringComparison.OrdinalIgnoreCase));
            var refreshTokenValue = refreshTokenPart.Split('=')[1].Trim();

            _output.WriteLine($"Refresh Token before revoke: {refreshTokenValue}");

            var responseJson = await registerResponse.Content.ReadAsStringAsync();
            var accessToken = System.Text.Json.JsonDocument.Parse(responseJson)
                                .RootElement.GetProperty("value").GetProperty("accessToken").GetString();
            Assert.False(string.IsNullOrEmpty(accessToken));
            _output.WriteLine($"Access Token: {accessToken}");

            // Act 
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/auth/revoke-refresh-token");
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
            requestMessage.Headers.Add("Cookie", $"refreshToken={refreshTokenValue}");

            var revokeResponse = await _client.SendAsync(requestMessage);
            revokeResponse.EnsureSuccessStatusCode();

            var revokeJson = await revokeResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Revoke Refresh Token Response: {revokeJson}");

            // Assert 
            Assert.Contains("Refresh Token revoked successfully", revokeJson, StringComparison.OrdinalIgnoreCase);
        }



        [Fact]
        public async Task GenerateAccessToken_WithRefreshToken_ReturnsNewAccessToken()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Token",
                    LastName = "Tester",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var setCookieHeader = registerResponse.Headers.GetValues("Set-Cookie").FirstOrDefault();
            Assert.NotNull(setCookieHeader);
            var refreshTokenPart = setCookieHeader.Split(';').First(c => c.Trim().StartsWith("refreshToken="));
            var refreshTokenValue = refreshTokenPart.Split('=')[1].Trim();

            _output.WriteLine($"Refresh Token: {refreshTokenValue}");

            var registerJson = await registerResponse.Content.ReadAsStringAsync();
            var accessToken = System.Text.Json.JsonDocument.Parse(registerJson)
                                .RootElement.GetProperty("value").GetProperty("accessToken").GetString();
            Assert.False(string.IsNullOrEmpty(accessToken));
            _output.WriteLine($"Access Token: {accessToken}");

            // Act 
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/auth/generate-access-token");
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
            requestMessage.Headers.Add("Cookie", $"refreshToken={refreshTokenValue}");

            var tokenResponse = await _client.SendAsync(requestMessage);
            tokenResponse.EnsureSuccessStatusCode();

            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Generate Access Token Response: {tokenJson}");

            // Assert 
            Assert.Contains("accessToken", tokenJson, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetUserById_AsAdmin_ReturnsUserInfo()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Fetch",
                    LastName = "User",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var registerJson = await registerResponse.Content.ReadAsStringAsync();
            var userId = System.Text.Json.JsonDocument.Parse(registerJson)
                            .RootElement.GetProperty("value").GetProperty("id").GetString();

            var adminLoginRequest = new
            {
                request = new
                {
                    Email = "admin@movies.com",
                    Password = "Admin@123"
                }
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", adminLoginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var adminAccessToken = System.Text.Json.JsonDocument.Parse(loginJson)
                                    .RootElement.GetProperty("value")
                                    .GetProperty("accessToken").GetString();

            // Act 
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/auth/user/{userId}");
            requestMessage.Headers.Add("Authorization", $"Bearer {adminAccessToken}");

            var response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"GetUserById Response: {responseJson}");

            // Assert 
            Assert.Contains(email, responseJson, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Fetch", responseJson, StringComparison.OrdinalIgnoreCase);
        }


        [Fact]
        public async Task DeleteUser_AuthenticatedUser_ReturnsSuccess()
        {
            // Arrange 
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "StrongPass123!";

            var registerRequest = new
            {
                request = new
                {
                    FirstName = "Delete",
                    LastName = "User",
                    Email = email,
                    Password = password
                }
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var registerJson = await registerResponse.Content.ReadAsStringAsync();
            var userId = System.Text.Json.JsonDocument.Parse(registerJson)
                .RootElement.GetProperty("value").GetProperty("id").GetString();

            var loginRequest = new
            {
                request = new
                {
                    Email = email,
                    Password = password
                }
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginJson = await loginResponse.Content.ReadAsStringAsync();
            var accessToken = System.Text.Json.JsonDocument.Parse(loginJson)
                .RootElement.GetProperty("value").GetProperty("accessToken").GetString();

            // Act 
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "/api/auth");
            requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            var deleteBody = JsonSerializer.Serialize(new { id = userId });
            requestMessage.Content = new StringContent(deleteBody, Encoding.UTF8, "application/json");

            var deleteResponse = await _client.SendAsync(requestMessage);
            deleteResponse.EnsureSuccessStatusCode();

            var responseJson = await deleteResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Delete User Response: {responseJson}");

            // Assert 
            Assert.Contains("true", responseJson, StringComparison.OrdinalIgnoreCase);
        }





    }
}
