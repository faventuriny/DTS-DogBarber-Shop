using System.ComponentModel.DataAnnotations;

namespace DTS_DogBarber_Shop.Data.Models.Dtos
{
    public class ChangeAppointmentRequest
    {
        [Required]
        public string QueueTime { get; set; }
    }
}
