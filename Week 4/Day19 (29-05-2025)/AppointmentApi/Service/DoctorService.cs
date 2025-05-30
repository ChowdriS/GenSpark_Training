using System;
using AppointmentApi.Interface;
using AppointmentApi.Misc;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Service;

public class DoctorService : IDoctorService
{
    DoctorMapper doctorMapper;
    SpecialityMapper specialityMapper;
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    private readonly IOtherContextFunctionities _otherContextFunctionities;
    public DoctorService(IRepository<int, Doctor> doctorRepository,
                        IRepository<int, Speciality> specialityRepository,
                        IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
                        IOtherContextFunctionities otherContextFunctionities)
    {
        doctorMapper = new DoctorMapper();
        specialityMapper = new();
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;
        _otherContextFunctionities = otherContextFunctionities;
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

    // Session way of using mapper

    // public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctor)
    // {
    //     try
    //     {
    //         var newDoctor = doctorMapper.MapDoctorAddRequestDoctor(doctor);
    //         newDoctor = await _doctorRepository.Add(newDoctor);
    //         if (newDoctor == null)
    //             throw new Exception("Could not add doctor");
    //         if (doctor.Specialities.Count() > 0)
    //         {
    //             int[] specialities = await MapAndAddSpeciality(doctor);
    //             for (int i = 0; i < specialities.Length; i++)
    //             {
    //                 var doctorSpeciality = specialityMapper.MapDoctorSpecility(newDoctor.Id, specialities[i]);
    //                 doctorSpeciality = await _doctorSpecialityRepository.Add(doctorSpeciality);
    //             }
    //         }
    //         return newDoctor;
    //     }
    //     catch (Exception e)
    //     {
    //         throw new Exception(e.Message);
    //     }

    // }

    // private async Task<int[]> MapAndAddSpeciality(DoctorAddRequestDto doctor)
    // {
    //     int[] specialityIds = new int[doctor.Specialities.Count()];
    //     IEnumerable<Speciality> existingSpecialities = null;
    //     try
    //     {
    //         existingSpecialities = await _specialityRepository.GetAll();
    //     }
    //     catch (Exception e)
    //     {

    //     }
    //     int count = 0;
    //     foreach (var item in doctor.Specialities)
    //     {
    //         Speciality speciality = null;
    //         if (existingSpecialities != null)
    //             speciality = existingSpecialities.FirstOrDefault(s => s.Name.ToLower() == item.Name.ToLower());
    //         if (speciality == null)
    //         {
    //             speciality = specialityMapper.MapSpecialityAddRequestDoctor(item);
    //             speciality = await _specialityRepository.Add(speciality);
    //         }
    //         specialityIds[count] = speciality.Id;
    //         count++;
    //     }
    //     return specialityIds;
    // }
    public async Task<Doctor> GetDoctorByName(string name)
    {
        var allDoctors = await _doctorRepository.GetAll();
        var matchedDoctors = allDoctors.FirstOrDefault(x => x.Name == name);
        if (matchedDoctors != null)
            return matchedDoctors;
        else
            throw new Exception("No doctor found with the given name");
    }

    // public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
    // {
    //     var all_speciality = await _specialityRepository.GetAll();
    //     var matchedSpeciality = all_speciality.FirstOrDefault(spec => spec.Name == speciality);

    //     if (matchedSpeciality != null)
    //     {
    //         var allDoctorsSpecialities = await _doctorSpecialityRepository.GetAll();
    //         var matchedDoctorsId = allDoctorsSpecialities.Where(ds => matchedSpeciality.Id == ds.SpecialityId)
    //                                                     .Select(ds => ds.DoctorId)
    //                                                     .ToList();

    //         var allDoctors = await _doctorRepository.GetAll();
    //         var matchedDoctors = allDoctors.Where(doc => matchedDoctorsId.Contains(doc.Id)).ToList();
    //         return matchedDoctors;
    //     }
    //     else
    //     {
    //         throw new Exception("No Speciality with the given name");
    //     }
    // }
    
    
    public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
    {
        // session way of finding doctor with specific spec using sp
        System.Console.WriteLine("Session method");
        var result = await _otherContextFunctionities.GetDoctorsBySpeciality(speciality);
        return result;
    }
}