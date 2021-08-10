using DTS_DogBarber_Shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTS_DogBarber_Shop.Data
{
    public interface IAppointmentsRepo
    {
        bool SaveChanges();

        IEnumerable<AppointmentIdentity> GetAppointments();
        AppointmentIdentity GetAppointment(int id);
        void UpdateAppointment(AppointmentIdentity queue);
        void CreateNewAppointment(AppointmentIdentity queue);
        void DeleteAppointment(AppointmentIdentity queue);
        List<string> GetTimeSlotForDate(DateTime date);
    }
}
