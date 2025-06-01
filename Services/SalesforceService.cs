using System;
using System.Threading.Tasks;

namespace SalesforceAIForm.Services
{
    public class SalesforceService
    {
        // Mocked CreateLeadAsync simulates the real Salesforce call
        public async Task<string> CreateLeadAsync(string name, string email, string queryDetails, string interests)
        {
            await Task.Delay(300); // Simulate network delay
            return Guid.NewGuid().ToString("N"); // Simulate Salesforce Lead ID
        }
    }
}
