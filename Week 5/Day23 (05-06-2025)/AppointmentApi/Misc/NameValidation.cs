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


// Notify is a document-sharing API designed for internal team collaboration in an organization. 
// The primary goals are to allow secure file uploads, restrict access to only authorized personnel,
// and notify all connected users in real-timewhenever a new document is added.


// The HR department of a company wants to securely share onboarding documents with internal staff.
// Only HR admins are allowed to upload documents. Once uploaded, all logged-in users should instantly be notified so 
// they can access the new document.
// The IT team is tasked with building a backend API that supports:
// * Role-based authentication using JWT
// * Secure file upload and storage
// * Real-time client notifications using SignalR 