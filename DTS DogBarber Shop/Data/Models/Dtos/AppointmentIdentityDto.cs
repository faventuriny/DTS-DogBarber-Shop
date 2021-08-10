using System;

namespace DTS_DogBarber_Shop.Data.Models.Dtos
{
    public class AppointmentIdentityDto
    {
        public int Id { get; set; }
        public DateTime? QueueTime { get; set; }
        public DateTime? RegistTime { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
