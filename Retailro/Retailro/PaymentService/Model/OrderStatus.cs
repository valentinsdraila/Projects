namespace PaymentService.Model
{
    /// <summary>
    /// All the possible statuses of an order
    /// </summary>
    public enum OrderStatus
    {
        None,
        Paid,
        Shipping,
        Completed,
        Cancelled,
        Valid,
        Processing
    }
}
