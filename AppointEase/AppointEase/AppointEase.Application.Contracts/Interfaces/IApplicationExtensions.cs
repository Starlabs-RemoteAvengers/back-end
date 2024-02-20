using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IApplicationExtensions
    {
        void AddInformationMessage(string message);
        void AddErrorMessage(string message);
    }
}
