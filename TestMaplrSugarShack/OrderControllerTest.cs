using Microsoft.Extensions.Configuration;
using System.Net;
using TestMaplrSugarShack.TestConfig;

namespace TestMaplrSugarShack
{
    public class Tests
    {
        private ComponentTestConfig TestConfig { get; set; } = new ComponentTestConfig();
        private HttpClient _httpClient;

        private StringContent createPayload(string productId, int qty)
        {
            var payload = new
            {
                productId = "880c684d-770b-4274-9891-c00e08d37f2f",
                qty = 2
            };
            return new StringContent(payload.ToString());
        }

        [OneTimeSetUp]
        public void InitialSetup()
        {
            var json_file = "appsettings.development.json";
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env))
            {
                json_file = "appsettings.development.json";
            }
            else
            {
                if (env.Equals("docker"))
                {
                    json_file = "appsettings.docker.json";
                }
                else if (env.Equals("local"))
                {
                    json_file = "appsettings.local.json";
                }
                else
                {
                    json_file = "appsettings.development.json";
                }
            }
   
            TestConfig = new ConfigurationBuilder()
                .AddJsonFile(json_file, false, false)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("ComponentTests")
                .Get<ComponentTestConfig>();
        }

        [SetUp]
        public void Setup()
        {
            // Arrange
            _httpClient = new HttpClient { BaseAddress = new Uri(TestConfig.ServiceUri) };
        }

        [Test]
        public async Task TestPlaceOrder__no_authentication__expected_401_unauthorized()
        {
            // Arrange
            var stringContent = createPayload("880c684d-770b-4274-9891-c00e08d37f2f", 2);

            // Act
            var httpResponse = await _httpClient.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}