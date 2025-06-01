using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SalesforceAIForm.Services;

namespace SalesforceAIForm.Pages
{
    public class CustomerFormModel : PageModel
    {
        [BindProperty]
        public CustomerInput Customer { get; set; } = new CustomerInput();

        // Mock Salesforce service
        private readonly SalesforceService _salesforceService = new SalesforceService();
        // OpenAI service
        private readonly OpenAIService _openAIService = new OpenAIService("");

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 1. Simulate Salesforce call
            string mockSalesforceId = await _salesforceService.CreateLeadAsync(
                Customer.Name,
                Customer.Email,
                Customer.QueryDetails,
                Customer.CustomerInterests);

            TempData["SalesforceId"] = mockSalesforceId;

            // 2. Call OpenAI for AI insights
            string prompt = $"Summarize this customer query and suggest next actions:\n" +
                            $"Name: {Customer.Name}\n" +
                            $"Email: {Customer.Email}\n" +
                            $"Query Details: {Customer.QueryDetails}\n" +
                            $"Customer Interests: {Customer.CustomerInterests}";

            string aiInsights = await _openAIService.GetAIInsightsAsync(prompt);

            TempData["AIInsights"] = aiInsights;

            return RedirectToPage("ThankYou");
        }

        public class CustomerInput
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? QueryDetails { get; set; }
            public string? CustomerInterests { get; set; }
        }
    }
}
