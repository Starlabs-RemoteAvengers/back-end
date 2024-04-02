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
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public IEnumerable<string> Errors { get; private set; }


        private OperationResult(bool success, string message, IEnumerable<string> errors)
        {
            Succeeded = success;
            Message = message;
            Errors = errors;
        }

        public OperationResult(bool success, string message)
        {
            Succeeded = success;
            Message = message;
        }

        public OperationResult()
        {
        }

        public OperationResult SuccessResult(string message = "Operation completed successfully.")
        {
            return new OperationResult(true, message);
        }

        public OperationResult ErrorResult(string message, IEnumerable<string> errors)
        {
            return new OperationResult(false, message, errors);
        }

        // Implement the missing method from IOperationResult interface
        public OperationResult ErrorResult(string message, object errors)
        {
            // Convert the 'errors' object to an IEnumerable<string> if needed
            // For example, you might check if it's already a collection of strings or perform other conversions.
            // For simplicity, assuming 'errors' is a string or can be converted to a string.
            var errorList = new List<string> { errors.ToString() };

            return new OperationResult(false, message, errorList);
        }

        public OperationResult ErrorResult(string v)
        {
            throw new NotImplementedException();
        }
    }
}
