using System;

namespace DTS_DogBarber_Shop.Data.Models.Dtos
{
    public class CreateNewQueueDto
    {
        public DateTime? QueueTime { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
