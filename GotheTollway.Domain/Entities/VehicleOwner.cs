using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    /// <summary>
    /// This is the entity class for the vehicle owner.
    /// A vehicle owner can have multiple vehicles.
    /// We need to store the owner's first name, last name, address, and zip code.
    /// So that we can send the toll bill to the owner.
    /// </summary>
    public class VehicleOwner
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public List<Vehicle> Vehicles { get; set; } = [];
    }
}
