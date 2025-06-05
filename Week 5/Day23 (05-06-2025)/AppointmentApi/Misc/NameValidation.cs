using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Misc;

public class NameValidation : ValidationAttribute
{
    public override bool IsValid(Object? name)
    {
        string str_name = name?.ToString() ?? "";

        if (string.IsNullOrEmpty(str_name)) return false;

        foreach (char ch in str_name)
        {
            if (!char.IsLetter(ch) || !char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch)) return false;
        }
        return true;
    }
}
