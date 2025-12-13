using System.Text.RegularExpressions;
using Home_5.BLL.Enums;

namespace Home_5.API.Validations;

public static class UserValidator
{
    public static void ValidateUser(string firstName, string lastName, string email, DateTime birthDate)
    {
        ValidFirstName(firstName);
        ValidLastName(lastName);
        ValidEmail(email);
        ValidateBirthDate(birthDate);
    }
    
    public static void IsValidId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0"); 
        }
    }
    
    public static void ValidFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 3)
        {
            throw new ArgumentException("FirstName must be at least 3 characters long.");
        }
    }
    
    public static void ValidLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3)
        {
            throw new ArgumentException("LastName must be at least 3 characters long.");
        }
    }
    
    public static void ValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }
        
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        
        if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
        {
            throw new ArgumentException($"Invalid email format: '{email}'");
        }
    }
    
    public static void ValidateBirthDate(DateTime birthDate)
    {
        var today = DateTime.Today;
        
        if (birthDate.Date > today)
        {
            throw new ArgumentException("Birth date cannot be in the future.");
        }
        
        if (birthDate.Date > today.AddYears(-3))
        {
            throw new ArgumentException("User must be at least 3 years old.");
        }

        if (birthDate.Date < today.AddYears(-90))
        {
            throw new ArgumentException("User must be younger than 90 years old.");
        }
    }
}