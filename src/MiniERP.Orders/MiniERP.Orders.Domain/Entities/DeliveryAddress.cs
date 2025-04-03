namespace MiniERP.Orders.Domain.Entities
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int OrderId { get; set; }
    }
}