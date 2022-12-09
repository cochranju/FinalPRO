namespace FinalPRO.Models
{
    public class Resident
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitNumber { get; set; }
        public string EmailAddress { get; set; }
        public List<Package> Packages { get; set; }
    }
}
