using Microsoft.Data.SqlClient;
using PatientApi.Features.Patient.DTOs;
using PatientApi.Features.Patient.Interfaces;
using PatientEntity = PatientApi.Features.Patient.Entities.Patient;

namespace PatientApi.Features.Patient.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly string _connectionString;

    public PatientRepository()
    {
        _connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION") ?? throw new InvalidOperationException("Database connection string is missing!");
    }

    public async Task<PatientEntity?> GetById(Guid patientId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT Id, FirstName, LastName, Email FROM Patients WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", patientId);

        using var reader = await command.ExecuteReaderAsync();
        if (!reader.Read()) return null;

        return new PatientEntity
        {
            Id = reader.GetGuid(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3)
        };
    }

    public async Task<Guid> Create(PatientEntity patient)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var patientId = Guid.NewGuid();
        string query = "INSERT INTO Patients (Id, FirstName, LastName, Email) VALUES (@Id, @FirstName, @LastName, @Email)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", patientId);
        command.Parameters.AddWithValue("@FirstName", patient.FirstName);
        command.Parameters.AddWithValue("@LastName", patient.LastName);
        command.Parameters.AddWithValue("@Email", patient.Email);

        await command.ExecuteNonQueryAsync();
        return patientId;
    }

    public async Task<bool> Update(PatientEntity patient)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "UPDATE Patients SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", patient.Id);
        command.Parameters.AddWithValue("@FirstName", patient.FirstName);
        command.Parameters.AddWithValue("@LastName", patient.LastName);
        command.Parameters.AddWithValue("@Email", patient.Email);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> Delete(Guid patientId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "DELETE FROM Patients WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", patientId);

        int rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<List<PatientEntity>> GetList()
    {
        var patients = new List<PatientEntity>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT Id, FirstName, LastName, Email FROM Patients";
        using var command = new SqlCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            patients.Add(new PatientEntity
            {
                Id = reader.GetGuid(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3)
            });
        }

        return patients;
    }
}