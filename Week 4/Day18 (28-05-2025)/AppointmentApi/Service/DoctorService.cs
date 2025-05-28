using System;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Service;

public class DoctorService : IDoctorService
{
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    public DoctorService(IRepository<int, Doctor> doctorRepository,
                        IRepository<int, Speciality> specialityRepository,
                        IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
    {
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorAddRequestDto)
    {
        var doctor = new Doctor
        {
            Name = doctorAddRequestDto.Name,
            Status = "created",
            YearsOfExperience = doctorAddRequestDto.YearsOfExperience
        };
        var addedDoctor = await _doctorRepository.Add(doctor);

        // System.Console.WriteLine(addedDoctor.Name);

        if (doctorAddRequestDto.Specialities != null)
        {
            foreach (var iter_speciality in doctorAddRequestDto.Specialities)
            {
                Speciality speciality;
                try
                {
                    var all_speciality = await _specialityRepository.GetAll();
                    var exisiting_speciality = all_speciality.FirstOrDefault(x => x.Name == iter_speciality.Name);
                    if (exisiting_speciality == null)
                    {
                        speciality = new Speciality
                        {
                            Name = iter_speciality.Name,
                            Status = "created"
                        };
                        await _specialityRepository.Add(speciality);
                    }
                    else
                    {
                        speciality = exisiting_speciality;
                    }
                }
                catch (Exception)
                {
                    speciality = new Speciality
                    {
                        Name = iter_speciality.Name,
                        Status = "created"
                    };
                    await _specialityRepository.Add(speciality);
                }
                var doctor_speciality = new DoctorSpeciality
                {
                    DoctorId = addedDoctor.Id,
                    SpecialityId = speciality.Id
                };
                var addedDoctorSpeciality = await _doctorSpecialityRepository.Add(doctor_speciality);
            }
        }
        return addedDoctor;
    }

    public async Task<Doctor> GetDoctorByName(string name)
    {
        var allDoctors = await _doctorRepository.GetAll();
        var matchedDoctors = allDoctors.FirstOrDefault(x => x.Name == name);
        if (matchedDoctors != null)
            return matchedDoctors;
        else
            throw new Exception("No doctor found with the given name");
    }

    public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
    {
        var all_speciality = await _specialityRepository.GetAll();
        var matchedSpeciality = all_speciality.FirstOrDefault(spec => spec.Name == speciality);

        if (matchedSpeciality != null)
        {
            var allDoctorsSpecialities = await _doctorSpecialityRepository.GetAll();
            var matchedDoctorsId = allDoctorsSpecialities.Where(ds => matchedSpeciality.Id == ds.SpecialityId)
                                                        .Select(ds => ds.DoctorId)
                                                        .ToList();

            var allDoctors = await _doctorRepository.GetAll();
            var matchedDoctors = allDoctors.Where(doc => matchedDoctorsId.Contains(doc.Id)).ToList();
            return matchedDoctors;
        }
        else
        {
            throw new Exception("No Speciality with the given name");
        }
    }
}