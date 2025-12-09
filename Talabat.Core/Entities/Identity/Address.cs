using System.ComponentModel.DataAnnotations;

namespace Talabat.Core.Entities.Identity
{
    public class Address
    {
        [Key]
        public string AppUserId { get; set; }  // foreign key for table AppUser
        public string FName { get; set; }
        public string LName { get; set; }
        public string Street { get; set; }
        public string Ciy { get; set; }
        public string Country { get; set; }
        public AppUser AppUser { get; set; }
    }
}