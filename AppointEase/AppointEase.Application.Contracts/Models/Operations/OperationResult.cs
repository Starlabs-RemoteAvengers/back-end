using AppointEase.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Operations
{
    public class OperationResult : IOperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        private OperationResult(bool success, string message, IEnumerable<string> errors)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }
        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public OperationResult()
        {
            
        }

        

        public OperationResult SuccessResult(string message = "Operation completed successfully.")
        {
            return new OperationResult (true, message);
        }

        public OperationResult ErrorResult(string message, IEnumerable<string> errors)
        {
            return new OperationResult(false, message, errors);
        }

    }
}
