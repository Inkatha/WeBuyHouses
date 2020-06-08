using System.ComponentModel.DataAnnotations;

namespace WeBuyHouses.Models
{
    public class Customer : Person
    {
        public int CustomerId { get; set; }

        [Required]
        public string DealFinderCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
        
        [Required]
        public string Zip { get; set; }
    }
}