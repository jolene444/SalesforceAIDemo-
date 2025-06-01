using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesforceAIForm.Services
{
    // This service class is responsible for communicating with OpenAI's API
    public class OpenAIService
    {
        // Store the API key for authentication
        private readonly string _apiKey;

        // Constructor takes the API key as a parameter
        public OpenAIService(string apiKey)
        {
            _apiKey = apiKey;
        }

        // Main method to get AI insights by sending a prompt to OpenAI
        public async Task<string> GetAIInsightsAsync(string prompt)
        {
            // Create a new HttpClient instance for HTTP requests
            using var client = new HttpClient();

            // Set the authorization header to use the OpenAI Bearer token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // Build the request body for OpenAI's Chat Completion API
            var requestBody = new
            {
                model = "gpt-3.5-turbo", // Use the GPT-3.5 Turbo model
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            // Serialize the request body to JSON and set the content type header
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // Send a POST request to the OpenAI API
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            // If the response is not successful, return the error content
            if (!response.IsSuccessStatusCode)
            {
                return $"OpenAI error: {await response.Content.ReadAsStringAsync()}";
            }

            // Parse the JSON response from OpenAI
            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            // Extract the assistant's reply (the first choice)
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // Return the reply, or a default message if not found
            return reply ?? "No response";
        }
    }
}
