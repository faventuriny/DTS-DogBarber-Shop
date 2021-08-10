using DTS_DogBarber_Shop.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTS_DogBarber_Shop.Data
{
    public class SqlAppoinmentsRepo : IAppointmentsRepo
    {
        private ApplicationDbContext _context;
        public SqlAppoinmentsRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void CreateNewAppointment(AppointmentIdentity queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }
            if (!IsAppointmentAvailable(queue.QueueTime))
            {
                throw new Exception("This appointment is already taken");
            }
            _context.Queue.Add(queue);
        }

        public void DeleteAppointment(AppointmentIdentity queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }
            _context.Queue.Remove(queue);
        }

        public AppointmentIdentity GetAppointment(int id)
        {
            return _context.Queue.FirstOrDefault(q => q.Id == id);
        }

        public IEnumerable<AppointmentIdentity> GetAppointments()
        {
            return _context.Queue.FromSqlRaw(@"EXECUTE SPGetCustomerQueue").ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAppointment(AppointmentIdentity queue)
        {
            if (!IsAppointmentAvailable(queue.QueueTime))
            {
                throw new Exception("This appointment is already taken");
            }
            _context.Queue.Update(queue);
        }
        public List<string> GetTimeSlotForDate(DateTime date)
        {
            var queuesInSpecificDate = _context.Queue.Where(q => q.QueueTime.Date == date.Date).ToList();

            //extract the hours as ints
            List<int> reservedTimeSlot = new List<int>();
            foreach (var queue in queuesInSpecificDate)
            {
                reservedTimeSlot.Add(queue.QueueTime.Hour);
            }

            //remove the the reserved hours from all free time slot
            List<int> freeTimeSlot = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            foreach (var time in reservedTimeSlot)
            {
                freeTimeSlot.Remove(time);
            }

            //create a list of houes as a string
            List<string> newFreeTimeSlot = new List<string>();
            foreach (var houe in freeTimeSlot)
            {
                if (houe == 9)
                {
                    newFreeTimeSlot.Add("09:00");
                }
                else
                {
                    newFreeTimeSlot.Add(houe + ":00");
                }
            }

            return newFreeTimeSlot;
        }

        private bool IsAppointmentAvailable(DateTime date)
        {
            var dates = _context.Queue.Where(q => q.QueueTime == date).ToList();
            if (dates.Count() == 0)
            {
                return true;
            }
            return false;

        }
    }
}
