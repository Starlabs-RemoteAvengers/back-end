using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Models;

namespace AppointEase.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IDoctorService _doctorService;
        private readonly IClinicService _clinicService;

        public SearchService(IDoctorService doctorService, IClinicService clinicService)
        {
            _doctorService = doctorService;
            _clinicService = clinicService;
        }

        public async Task<IEnumerable<object>> GetAllDoctors()
        {
            try
            {
                var allDoctors = await _doctorService.GetAllDoctors();
                var allClinics = await _clinicService.GetAllClinics();

                var doctorClinicPairs = allDoctors.Join(
                    allClinics,
                    doctor => doctor.ClinicId,
                    clinic => clinic.Id,
                    (doctor, clinic) => new
                    {
                        DoctorId = doctor.Id,
                        DoctorName = doctor.Name,
                        DoctorEmail = doctor.Email,
                        ClinicId = clinic.Id,
                        ClinicName = clinic.Name,
                        ClinicLocation = clinic.Location,
                        DoctorPhoto = doctor.PhotoData,
                        PictureFormat = doctor.PhotoFormat,
                        Specialisation = doctor.Specialisation
                    });

                return doctorClinicPairs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching doctors: " + ex.Message);
            }
        }

        private async Task<IEnumerable<object>> SearchDoctorsAsync(IEnumerable<ClinicRequest> clinics, SearchRequest searchRequest)
        {
            var filteredDoctors = await _doctorService.FilterDoctors(query =>
            {
                var filteredQuery = query.Join(clinics, doctor => doctor.ClinicId, clinic => clinic.Id, (doctor, clinic) => new { Doctor = doctor, Clinic = clinic });

                if (!string.IsNullOrEmpty(searchRequest.SearchTerm) || !string.IsNullOrEmpty(searchRequest.Location) || !string.IsNullOrEmpty(searchRequest.Category))
                {
                    filteredQuery = filteredQuery.Where(pair =>
                        (string.IsNullOrEmpty(searchRequest.SearchTerm) || pair.Doctor.Name.ToLower().Contains(searchRequest.SearchTerm.ToLower())) &&
                        (string.IsNullOrEmpty(searchRequest.Location) || pair.Clinic.Location.ToLower().Contains(searchRequest.Location.ToLower())) 
                    );
                }

                return filteredQuery.Select(pair => new
                {
                    DoctorId = pair.Doctor.Id,
                    DoctorName = pair.Doctor.Name,
                    ClinicName = pair.Clinic.Name,
                    ClinicLocation = pair.Clinic.Location,
                    DoctorEmail = pair.Doctor.Email,
                    DoctorPhoto = pair.Doctor.PhotoData,
                    PictureFormat = pair.Doctor.PhotoFormat,
                    Specialisation = pair.Doctor.Specialisation

                });
            });
            return filteredDoctors;
        }

        private async Task<List<object>> GetFilteredDoctorsAsync(SearchRequest searchRequest)
        {
            var clinics = await _clinicService.GetAllClinics();
            var doctorResults = await SearchDoctorsAsync(clinics, searchRequest);
            return doctorResults.Cast<object>().ToList();
        }

        public async Task<List<object>> GetSerach(SearchRequest searchRequest)
        {
            var searchResults = new List<object>();

            try
            {
                switch (searchRequest.SearchType)
                {
                    case "Doctor":
                        var filteredDoctors = await GetFilteredDoctorsAsync(searchRequest);
                        searchResults.AddRange(filteredDoctors);
                        break;
                    case "Clinic":
            
                        break;
                    
                    default:
                        return searchResults;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return searchResults;
        }

    }
}
 