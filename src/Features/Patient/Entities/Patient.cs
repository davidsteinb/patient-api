namespace PatientApi.Features.Patient.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
}

