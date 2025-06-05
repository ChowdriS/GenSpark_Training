using System;
using System.Security.Claims;
using AppointmentApi.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Policies;

public class DoctorExperienceHandler : AuthorizationHandler<DoctorExperienceRequirement>
{
    private readonly ClinicContext _context;
    public DoctorExperienceHandler(ClinicContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DoctorExperienceRequirement requirement)
    {
       var username = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // System.Console.WriteLine(username);
        if (string.IsNullOrEmpty(username))
        {
            return;
        }
        var user = await _context.Users.Include(u => u.Doctor)
                                       .FirstOrDefaultAsync(u => u.Username == username);
        // System.Console.WriteLine(user.Doctor.YearsOfExperience);
        if (user != null && user.Role == "Doctor" && user.Doctor != null && user.Doctor.YearsOfExperience > requirement.MinimumYears)
        {
            context.Succeed(requirement);
        }
    }
}
