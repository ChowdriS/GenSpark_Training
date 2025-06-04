using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppointmentApi.Policies;


public class DoctorExperienceRequirement : IAuthorizationRequirement
{
    public float MinimumYears { get; }
    public DoctorExperienceRequirement(float minimumYears)
    {
        MinimumYears = minimumYears;
    }
}