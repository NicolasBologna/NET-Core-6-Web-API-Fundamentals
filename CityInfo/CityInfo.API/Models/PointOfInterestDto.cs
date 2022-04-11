namespace CityInfo.API.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; } // Esto solo me sirve para hacer el get, pero no para crear.
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
