using AutoMapper;
using DTS_DogBarber_Shop.Data;
using DTS_DogBarber_Shop.Data.Models;
using DTS_DogBarber_Shop.Data.Models.Dtos;
using DTS_DogBarber_Shop.Data.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTS_DogBarber_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentsRepo _repository;
        private readonly IMapper _mapper;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AppointmentController(IAppointmentsRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET /api/Appointment
        [HttpGet]
        public async Task<ActionResult> GetAppointments()
        {
            try
            {
                var appointmentsList = _repository.GetAppointments();
                log.Info("GetAppointments() - Succeed");
                //return Ok(new AppoinmentResponse<IEnumerable<AppointmentIdentityDto>>(true, _mapper.Map<IEnumerable<AppointmentIdentityDto>>(appointmentsList)));
                return Ok(new AppointmentResponse<IEnumerable<AppointmentIdentityDto>>()
                {
                    IsSuccess = true,
                    Payload = _mapper.Map<IEnumerable<AppointmentIdentityDto>>(appointmentsList)
                });

            }
            catch
            {
                log.Info("GetAppointments() - Failed to retrieve appointment List ");
                return NotFound(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to retrieve appointment list"
                });
            }
        }

        // GET /api/Appointment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = _repository.GetAppointment(id);
                log.Info("GetAppointmentById() - Succeed");
                return Ok(new AppointmentResponse<AppointmentIdentityDto>()
                {
                    IsSuccess = true,
                    Payload = _mapper.Map<AppointmentIdentityDto>(appointment)
                });
            }
            catch
            {
                log.Error("GetAppointmentById() - Wrong Id, que couldn't be found");
                return NotFound(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to retrieve appointment"
                });
            }
        }

        // GET /api/Appointment/date/{2011-08-08}
        [HttpGet("date/{dateAsString}")]
        public async Task<ActionResult> GetTimeSlot(string dateAsString)
        {
            try
            {
                var dateAsArray = dateAsString.Split('-');
                DateTime date = new DateTime(int.Parse(dateAsArray[0]), int.Parse(dateAsArray[1]), int.Parse(dateAsArray[2]));
                var timeSlot = _repository.GetTimeSlotForDate(date);
                log.Info("GetTimeSlot() - Succeed");
                return Ok(new AppointmentResponse<List<string>>()
                {
                    IsSuccess = true,
                    Payload = timeSlot
                });
            }
            catch
            {
                log.Error("GetTimeSlot() - The Date is incorrect");
                return BadRequest(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to retrieve time slot"
                });
            }
        }

        //POST /api/Appointment
        [HttpPost]
        public ActionResult<AppointmentIdentityDto> CreateNewAppointment(AppointmentIdentityDto queueIdentityDto)
        {
            try
            {
                var queueModel = _mapper.Map<AppointmentIdentity>(queueIdentityDto);

                queueModel.RegistTime = DateTime.Now;
                queueModel.QueueTime = queueModel.QueueTime.ToLocalTime();
                _repository.CreateNewAppointment(queueModel);
                _repository.SaveChanges();

                log.Info("CreateNewHero() - Succeed");
                return Ok(new AppointmentResponse<AppointmentIdentityDto>()
                {
                    IsSuccess = true,
                    Payload = _mapper.Map<AppointmentIdentityDto>(queueModel)
                });
            }
            catch
            {
                return BadRequest(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to save new appointment"
                });
            }
        }

        // PATCH /api/Appointment
        [HttpPatch("{id}")]
        public ActionResult<AppointmentIdentityDto> EditAppoinment(int id, [FromBody] QueueTimeDto newQueueTime)
        {
            try
            {
                var queue = _repository.GetAppointment(id);

                if (queue == null)
                {
                    log.Info("EditAppoinment() -  Coudn't found the queue by id");
                    return NotFound(new AppointmentResponse<string>()
                    {
                        IsSuccess = false,
                        Payload = "Wrong Id"
                    });
                }

                queue.QueueTime = Convert.ToDateTime(newQueueTime.NewQueueAsString);
                queue.RegistTime = DateTime.Now;
                queue.QueueTime = queue.QueueTime.ToLocalTime();
                _repository.SaveChanges();

                log.Info("EditAppoinment() - Succeed");
                return Ok(new AppointmentResponse<AppointmentIdentityDto>()
                {
                    IsSuccess = true,
                    Payload = _mapper.Map<AppointmentIdentityDto>(queue)
                });
            }
            catch
            {
                return BadRequest(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to edit the appointment"
                });
            }
        }

        // Delete /api/Appointment/id
        [HttpDelete("{id}")]
        public ActionResult<AppointmentIdentityDto> DeleteAppoinment(int id)
        {
            try
            {
                var queueFromRepo = _repository.GetAppointment(id);
                if (queueFromRepo == null)
                {
                    log.Error("DeleteAppoinment() - Wrong id, couldn't found the queue");
                    return NotFound(new AppointmentResponse<string>()
                    {
                        IsSuccess = false,
                        Payload = "Wrong Id"
                    });
                }

                _repository.DeleteAppointment(queueFromRepo);
                _repository.SaveChanges();
                log.Info("DeleteAppoinment() - succeed");
                return Ok(new AppointmentResponse<string>()
                {
                    IsSuccess = true,
                    Payload = "Appontment deleted"
                });
            }
            catch
            {
                return BadRequest(new AppointmentResponse<string>()
                {
                    IsSuccess = false,
                    Payload = "Failed to delete the appointment"
                });
            }

        }
    }
}
