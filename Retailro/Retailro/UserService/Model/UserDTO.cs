namespace UserService.Model
{
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<DeliveryAddressDTO> DeliveryAddresses { get; set; } = new List<DeliveryAddressDTO>();
    }
}
