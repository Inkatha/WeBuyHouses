using System;
using System.ComponentModel.DataAnnotations;

namespace WeBuyHouses.Models
{
    public class BaseColumns
    {
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
    }
}