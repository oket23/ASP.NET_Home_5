using Home_5.BLL.Enums;

namespace Home_5.API.Validations;

public static class SubscriptionValidator
{
    public static void ValidateSubscription(string title, decimal price, DateTime startDate, DateTime endDate, SubscriptionsEnum type)
    {
        ValidateTitle(title);
        ValidatePrice(price);
        ValidateDates(startDate, endDate);
        ValidateType(type);
    }

    public static void IsValidId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0"); 
        }
    }
    
    public static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 3)
        {
            throw new ArgumentException("Title must be at least 3 characters long.");
        }
    }
    
    public static void ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be less than zero.");
        }
    }
    
    public static void ValidateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate > DateTime.Now) 
        {
            throw new ArgumentException("Start date cannot be in the future.");
        }
        
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be later than the start date.");
        }
    }

    public static void ValidateType(SubscriptionsEnum type)
    {
        if (!Enum.IsDefined(typeof(SubscriptionsEnum), type))
        {
            throw new ArgumentException("Invalid subscription type.");
        }
    }
}