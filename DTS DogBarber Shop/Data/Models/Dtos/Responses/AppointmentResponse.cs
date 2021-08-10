
namespace DTS_DogBarber_Shop.Data.Models.Dtos.Responses
{
    public class AppointmentResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Payload { get; set; }
    }
}
