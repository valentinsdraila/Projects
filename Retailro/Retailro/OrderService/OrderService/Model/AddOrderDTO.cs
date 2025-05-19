namespace OrderService.Model
{
    public class AddOrderDTO
    {
        public List<ProductInfo> ProductInfos { get; set; } = new List<ProductInfo>();
        public DeliveryAddress DeliveryAddress { get; set; } = new DeliveryAddress();
    }
}
