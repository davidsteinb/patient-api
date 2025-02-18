using PatientApi.Features.Patient.DTOs;
using PatientApi.Features.Patient.Interfaces;
using PatientApi.Infrastructure.Security;
using PatientEntity = PatientApi.Features.Patient.Entities.Patient;

namespace PatientApi.Features.Patient.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IEncryptionHelper _encryptionHelper;

    public PatientService(IPatientRepository patientRepository, IEncryptionHelper encryptionHelper)
    {
        _patientRepository = patientRepository;
        _encryptionHelper = encryptionHelper;
    }

    public async Task<string> CreatePatient(PatientDto dto)
    {
        var patientEntity = new PatientEntity
        {
            Id = Guid.NewGuid(),
            FirstName = _encryptionHelper.Encrypt(dto.FirstName),
            LastName = _encryptionHelper.Encrypt(dto.LastName),
            Email = _encryptionHelper.Encrypt(dto.Email)
        };

        var patientId = await _patientRepository.Create(patientEntity);
        return _encryptionHelper.Encrypt(patientId.ToString());
    }

    public async Task<PatientDto?> GetPatientById(string id)
    {
        string decryptedId = _encryptionHelper.Decrypt(id);
        if (!Guid.TryParse(id, out var patientId))
            return null;

        var patient = await _patientRepository.GetById(patientId);
        if (patient == null) return null;
                
        return new PatientDto
        {
            Id = _encryptionHelper.Encrypt(patient.Id.ToString()),
            FirstName = _encryptionHelper.Encrypt(patient.FirstName),
            LastName = _encryptionHelper.Encrypt(patient.LastName),
            Email = _encryptionHelper.Encrypt(patient.Email)
        };
    }

    public async Task<List<PatientDto>> GetAllPatients()
    {
        var patients = await _patientRepository.GetList();
        
        var dtoList = new List<PatientDto>();
        foreach (var patient in patients)
        {
            dtoList.Add(new PatientDto
            {
                Id = _encryptionHelper.Encrypt(patient.Id.ToString()),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email
            });
        }
        return dtoList;
    }

    public async Task<bool> UpdatePatient(PatientDto dto)
    {
        if (dto.Id == null)
            return false;

        string decryptedId = _encryptionHelper.Decrypt(dto.Id);
        if (!Guid.TryParse(decryptedId, out var patientId))
            return false;

        var patientEntity = new PatientEntity
        {
            Id = patientId,
            FirstName = _encryptionHelper.Encrypt(dto.FirstName),
            LastName = _encryptionHelper.Encrypt(dto.LastName),
            Email = _encryptionHelper.Encrypt(dto.Email)
        };

        return await _patientRepository.Update(patientEntity);
    }

    public async Task<bool> DeletePatient(string encryptedIdentifier)
    {
        string decryptedId = _encryptionHelper.Decrypt(encryptedIdentifier);
        if (!Guid.TryParse(decryptedId, out var patientId))
            return false;

        return await _patientRepository.Delete(patientId);
    }
}
