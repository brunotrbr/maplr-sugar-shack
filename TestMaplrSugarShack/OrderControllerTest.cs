using maplr_api.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;
using TestMaplrSugarShack.TestConfig;

namespace TestMaplrSugarShack
{
    public class Tests
    {
        private ComponentTestConfig TestConfig { get; set; } = new ComponentTestConfig();
        private HttpClient _httpClient;
        private HttpClient _httpClientAuth;

        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }

        public static void ValidateResponse(OrderValidationResponseDto expected, HttpResponseMessage httpResponse)
        {
            var ovrDTO = JsonConvert.DeserializeObject<OrderValidationResponseDto>(httpResponse.Content.ReadAsStringAsync().Result);
            Assert.That(ovrDTO.IsOrderValid, Is.EqualTo(expected.IsOrderValid));
            Assert.That(ovrDTO.Errors, Is.EqualTo(expected.Errors));
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
            _httpClient = new HttpClient { BaseAddress = new Uri(TestConfig.ServiceUri) };

            _httpClientAuth = new HttpClient { BaseAddress = new Uri(TestConfig.ServiceUri) };
            _httpClientAuth.DefaultRequestHeaders.Add("Authorization", $"Basic {Base64Encode($"maplr:maplr")}");
        }

        #region No Authentication
        [Test]
        public async Task TestPlaceOrder__no_authentication__expected_401_unauthorized()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = "880c684d-770b-4274-9891-c00e08d37f2f", Qty = 1 };
            orders.Add(orderLineDto);
            var json = (JsonConvert.SerializeObject(orders));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var httpResponse = await _httpClient.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        #endregion

        #region Authentication OK
        [Test]
        public async Task TestPlaceOrder__empty_order__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var json = (JsonConvert.SerializeObject(orders));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var expected = new OrderValidationResponseDto(false, new string[] { "Empty order" });
            
            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__order_already_placed__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            // Add to card
            // Post Order
            // Try to post order again, expect error
            var json = (JsonConvert.SerializeObject(orders));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var expected = new OrderValidationResponseDto(false, new string[] { "Empty order" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }
        #endregion
    }
}