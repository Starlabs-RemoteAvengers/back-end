using AppointEase.Application.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IPersonService
    {
        PersonDto GetPerson(int personId);
        IEnumerable<PersonDto> GetAllPersons();
        void CreatePerson(PersonDto personDto);
        void UpdatePerson(int personId, PersonDto personDto);
        void DeletePerson(int personId);
    }
}
