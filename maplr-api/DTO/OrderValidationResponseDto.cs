namespace maplr_api.DTO
{
    public class OrderValidationResponseDto
    {
        public bool IsOrderValid { get; set; }
        public string[] errors { get; set; }

        public OrderValidationResponseDto(bool isOrderValid, string[] errors)
        {
            IsOrderValid = isOrderValid;
            this.errors = errors;
        }
    }
}
