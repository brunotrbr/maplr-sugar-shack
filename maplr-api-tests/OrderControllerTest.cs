using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api_tests.TestConfig;
using Microsoft.Extensions.Configuration;

namespace maplr_api_tests
{
    public class Tests
    {
        private ComponentTestConfig TestConfig { get; set; } = new ComponentTestConfig();
        private HttpClient _httpClient;
        private HttpClient _httpClientAuth;
        const string AMBER = "f6fb258d-c33f-4de3-9288-253bc86234b0";
        const string DARK = "880c684d-770b-4274-9891-c00e08d37f2f";
        const string CLEAR = "d7e356c8-5aa1-4cc6-b5a8-c76f51d7906e";

        private static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }

        private static StringContent GenerateStringContent(string json_serialized){
            return new StringContent(json_serialized, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private void ValidateResponse(OrderValidationResponseDto expected, HttpResponseMessage httpResponse)
        {
            var ovrDTO = JsonSerializer.Deserialize<OrderValidationResponseDto>(httpResponse.Content.ReadAsStringAsync().Result);
            Assert.That(ovrDTO.IsOrderValid, Is.EqualTo(expected.IsOrderValid));
            Assert.That(ovrDTO.Errors, Is.EqualTo(expected.Errors));
        }

        private void AddToCart(string productId)
        {
            UriBuilder uriBuilder = new UriBuilder(TestConfig.ServiceUri);
            uriBuilder.Query = "";

            var json = (JsonSerializer.Serialize(string.Empty));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponse = _httpClientAuth.PutAsync($"{TestConfig.ServiceUri}/cart?productId={productId}", stringContent).Result;
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        }

        private void ChangeCartQty(string productId, int newQty)
        {
            UriBuilder uriBuilder = new UriBuilder(TestConfig.ServiceUri);
            uriBuilder.Query = "";

            var json = (JsonSerializer.Serialize(string.Empty));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponse = _httpClientAuth.PatchAsync($"{TestConfig.ServiceUri}/cart?productId={productId}&qty={newQty}", stringContent).Result;
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        }

        private void RemoveFromCart(string productId)
        {
            UriBuilder uriBuilder = new UriBuilder(TestConfig.ServiceUri);
            uriBuilder.Query = "";

            _ = _httpClientAuth.DeleteAsync($"{TestConfig.ServiceUri}/cart?productId={productId}").Result;
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
            var orderLineDto = new OrderLineDto() { ProductId = DARK, Qty = 1 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));

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
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { "Empty order" });
            
            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__cart_has_product_order_not_in_cart__expected_order_invalid()
        {
            // Arrange
            AddToCart(AMBER);
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = CLEAR, Qty = 2 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {CLEAR} not in cart" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
            RemoveFromCart(AMBER);
        }

        [Test]
        public async Task TestPlaceOrder__order_has_product_qty_differ_from_cart__expected_order_invalid()
        {
            // Arrange
            AddToCart(CLEAR);
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = CLEAR, Qty = 5 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {CLEAR} has different quantities in order and in cart" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
            RemoveFromCart(CLEAR);
        }

        [Test]
        public async Task TestPlaceOrder__cart_has_no_product_order_not_in_cart__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = AMBER, Qty = 1 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {AMBER} not in cart" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__order_qty_greather_than_stock__expected_order_invalid()
        {
            // Arrange
            AddToCart(CLEAR);
            ChangeCartQty(CLEAR, 100);
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = CLEAR, Qty = 100 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {CLEAR} quantity is greather than quantity in stock" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
            RemoveFromCart(CLEAR);
        }

        [Test]
        public async Task TestPlaceOrder__product_not_in_catalogue__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = "other", Qty = 100 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonSerializer.Serialize(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { "ProductId other not in cart", "ProductId other not in catalogue"  });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }
        #endregion
    }
}