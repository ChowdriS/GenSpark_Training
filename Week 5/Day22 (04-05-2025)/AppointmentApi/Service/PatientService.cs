using System;
using System.Diagnostics;
using AppointmentApi.Interface;
using AppointmentApi.Misc;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;
using AppointmentApi.Repository;
using AutoMapper;

namespace AppointmentApi.Service;

public class PatientService : IPatientService
{
    private readonly PatientMapper _patientMapper;
    private readonly IRepository<int, Patient> _patientRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IOtherContextFunctionities _otherContextFunctionities;
    private readonly IEncryptionService _encryptionService;
    private readonly IMapper _mapper;

    public PatientService(IRepository<int, Patient> patientRepository,
                            IRepository<string, User> userRepository,
                            IOtherContextFunctionities otherContextFunctionities,
                            IEncryptionService encryptionService,
                            IMapper mapper)
    {

        _patientMapper = new PatientMapper();
        _patientRepository = patientRepository;
        _userRepository = userRepository;
        _otherContextFunctionities = otherContextFunctionities;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }
    public async Task<Patient> CreatePatient(PatientAddRequestDTO Patient)
    {
        try
        {
            var user = _mapper.Map<PatientAddRequestDTO, User>(Patient);
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = Patient.Password
            });
            user.Password = encryptedData.EncryptedData;
            user.HashKey = encryptedData.HashKey;
            user.Role = "Patient";
            user = await _userRepository.Add(user);
            var newPatient = _patientMapper.MapPatientAddRequestPatient(Patient);
            newPatient = await _patientRepository.Add(newPatient);
            return newPatient;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
}
