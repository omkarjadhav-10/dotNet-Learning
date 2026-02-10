using System.ComponentModel.DataAnnotations;

namespace DotNET.Dtos;

public sealed class NotDefaultDateOnlyAttribute : ValidationAttribute
{
    public NotDefaultDateOnlyAttribute()
    {
        ErrorMessage = "Release date is required.";
    }

    public override bool IsValid(object? value)
    {
        if (value is DateOnly dateOnly)
        {
            return dateOnly != default;
        }

        return false;
    }
}
