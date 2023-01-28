namespace maplr_api.DTO
{
    public class OrderValidationResponseDto
    {
        public bool IsOrderValid { get; set; }
        public string[] Errors { get; set; }

        public OrderValidationResponseDto(bool isOrderValid, string[] errors)
        {
            IsOrderValid = isOrderValid;
            Errors = errors;
        }
    }
}
