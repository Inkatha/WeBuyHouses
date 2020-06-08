namespace WeBuyHouses.Models
{
    public class DealFinder : Person
    {
        public int DealFinderId { get; set; }
        public string DealFinderCode { get; set; }
        public int DealsFound { get; set; }
        public int DealsClosed { get; set; }
        public string CarColor { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
    }
}