namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>(); // Siempre declararlo con el mayor nivel de abstracción posible
                                                                                                                // - lo seteamos a una nieva colección para evitar que retorne un null

        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count; }
        }
    }
}
