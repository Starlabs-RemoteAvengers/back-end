﻿using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IBookAppointmentService
    {
        Task<BookAppointmentRequest> GetBookAppointmentById(string id);
        Task<IEnumerable<BookAppointmentRequest>> GetAllBookAppointment();
        Task<OperationResult> CreateBookAppointment(BookAppointmentRequest bookAppointment);
        Task<OperationResult> UpdateBookAppointment(string id, BookAppointmentRequest bookAppointment);
        Task<OperationResult> DeleteBookAppointment(string id);
    }
}