namespace PatientApi.Features.Patient.Interfaces;
using PatientEntity = PatientApi.Features.Patient.Entities.Patient;

public interface IPatientRepository
{
    Task<PatientEntity?> GetById(Guid patientId);
    Task<Guid> Create(PatientEntity patient);
    Task<bool> Update(PatientEntity patient);
    Task<bool> Delete(Guid patientId);
    Task<List<PatientEntity>> GetList();
}