using maplr_api.DTO;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;
using test_maplr_api.TestConfig;

namespace test_maplr_api
{
    public class Tests
    {
        private ComponentTestConfig TestConfig { get; set; } = new ComponentTestConfig();
        private HttpClient _httpClient;
        private HttpClient _httpClientAuth;
        const string AMBER = "f6fb258d-c33f-4de3-9288-253bc86234b0";
        const string DARK = "880c684d-770b-4274-9891-c00e08d37f2f";
        const string CLEAR = "d7e356c8-5aa1-4cc6-b5a8-c76f51d7906e";
        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }

        public static StringContent GenerateStringContent(string json_serialized){
            return new StringContent(json_serialized, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        public void ValidateResponse(OrderValidationResponseDto expected, HttpResponseMessage httpResponse)
        {
            var ovrDTO = JsonConvert.DeserializeObject<OrderValidationResponseDto>(httpResponse.Content.ReadAsStringAsync().Result);
            Assert.That(ovrDTO.IsOrderValid, Is.EqualTo(expected.IsOrderValid));
            Assert.That(ovrDTO.Errors, Is.EqualTo(expected.Errors));
        }

        public void AddToCart(string productId)
        {
            UriBuilder uriBuilder = new UriBuilder(TestConfig.ServiceUri);
            uriBuilder.Query = "";

            var json = (JsonConvert.SerializeObject(string.Empty));
            var stringContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponse = _httpClientAuth.PutAsync($"{TestConfig.ServiceUri}/cart?productId={productId}", stringContent).Result;
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        }

        public void RemoveFromCart(string productId)
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

        [TearDown]
        public void TearDown()
        {
            try
            {
                RemoveFromCart(AMBER);
            } 
            catch(Exception ex)
            {
                
            }

            try
            {
                RemoveFromCart(DARK);
            }
            catch (Exception ex2)
            {
                
            }

            try
            {
                RemoveFromCart(CLEAR);
            }
            catch (Exception ex3)
            {
            }
        }

        #region No Authentication
        [Test]
        public async Task TestPlaceOrder__no_authentication__expected_401_unauthorized()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = DARK, Qty = 1 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));


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
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
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
            // Arrange - Add to card
            AddToCart(DARK);

            // Arrange - First Order
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = DARK, Qty = 1 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
            var expected = new OrderValidationResponseDto(true, new string[] {  });

            // Act - First Order
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert - First Order
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);

            // Arrange - Second order
            expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {DARK} already placed" });

            // Act - Second Order
            httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert - Second Order
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__cart_has_product_order_not_in_cart__expected_order_invalid()
        {
            // Arrange
            AddToCart(CLEAR);
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = CLEAR, Qty = 2 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {CLEAR} not in cart." });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__order_has_product_qty_differ_from_cart__expected_order_invalid()
        {
            // Arrange
            AddToCart(AMBER);
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = AMBER, Qty = 5 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {AMBER} has different quantities in order and in cart" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__cart_has_no_product_order_not_in_cart__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = AMBER, Qty = 1 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
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
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = DARK, Qty = 100 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
            var expected = new OrderValidationResponseDto(false, new string[] { $"ProductId {DARK} not in cart", $"ProductId {DARK} quantity is greather than quantity in stock" });

            // Act
            var httpResponse = await _httpClientAuth.PostAsync("order", stringContent);

            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ValidateResponse(expected, httpResponse);
        }

        [Test]
        public async Task TestPlaceOrder__product_not_in_catalogue__expected_order_invalid()
        {
            // Arrange
            var orders = new List<OrderLineDto>();
            var orderLineDto = new OrderLineDto() { ProductId = "other", Qty = 100 };
            orders.Add(orderLineDto);
            var stringContent = GenerateStringContent(JsonConvert.SerializeObject(orders));
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