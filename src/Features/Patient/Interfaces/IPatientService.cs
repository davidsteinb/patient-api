using PatientApi.Features.Patient.DTOs;

namespace PatientApi.Features.Patient.Interfaces;

public interface IPatientService
{
    Task<string> CreatePatient(PatientDto dto);
    Task<PatientDto?> GetPatientById(string id);
    Task<bool> UpdatePatient(PatientDto dto);
    Task<bool> DeletePatient(string id);
    Task<List<PatientDto>> GetAllPatients();
}