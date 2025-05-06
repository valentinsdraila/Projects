namespace ProductService.Model.Exceptions
{
    public class PriceNotMatchingException : Exception
    {
        public PriceNotMatchingException(string? message) : base(message)
        {
        }
    }
}
