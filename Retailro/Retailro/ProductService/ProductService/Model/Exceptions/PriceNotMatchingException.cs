namespace ProductService.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when the price coming from the client of the application does not match the price in the database.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class PriceNotMatchingException : Exception
    {
        public PriceNotMatchingException(string? message) : base(message)
        {
        }
    }
}
