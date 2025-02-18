using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Features.Patient.DTOs;
using PatientApi.Features.Patient.Interfaces;

namespace PatientApi.Features.Patient.Controllers;

[ApiController]
[Route("api/v1/patient")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetPatientById([FromBody] PatientRequestDto patientRequestDto)
    {
        var patient = await _patientService.GetPatientById(patientRequestDto.Id);
        if (patient == null)
            return NotFound(new { Message = "Patient not found" });

        return Ok(patient);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePatient([FromBody] PatientDto request)
    {
        var encryptedId = await _patientService.CreatePatient(request);
        return Created("", new { Message = "Patient created successfully", Id = encryptedId });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePatient([FromBody] PatientDto request)
    {
        var success = await _patientService.UpdatePatient(request);
        if (!success)
            return NotFound(new { Message = "Patient not found" });

        return Ok(new { Message = "Patient updated successfully" });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePatient([FromBody] string encryptedIdentifier)
    {
        var success = await _patientService.DeletePatient(encryptedIdentifier);
        if (!success)
            return NotFound(new { Message = "Patient not found" });

        return Ok(new { Message = "Patient deleted successfully" });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllPatients()
    {
        var patients = await _patientService.GetAllPatients();
        return Ok(patients);
    }
}
