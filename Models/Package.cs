namespace FinalPRO.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string Agency { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool PickedUp { get; set; }
    }
}
