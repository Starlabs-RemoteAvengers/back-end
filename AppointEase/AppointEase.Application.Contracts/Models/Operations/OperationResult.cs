using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Operations
{
    public class OperationResult
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

        public static OperationResult SuccessResult(string message = "Operation completed successfully.")
        {
            return new OperationResult (true, message);
        }

        public static OperationResult ErrorResult(string message, IEnumerable<string> errors)
        {
            return new OperationResult(false, message, errors);
        }

    }
}
