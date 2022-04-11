using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDto //Puede ser q tengamos entidades que se creen con un Id específico, igualmente en ese caso recomienda usar un Dto aparte para poder hacer
                                               //cambios mas tranquilo. LA IDEA ES SEPARAR DTOs DE CREACIÓN, UPDATE Y CONSULTAS.
    {
        [Required(ErrorMessage = "Agregá un nombre")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
