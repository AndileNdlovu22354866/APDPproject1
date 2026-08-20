using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Static_and_Login.Models
{
    public class MinimumAgeAttribute :ValidationAttribute
    {    
            private readonly int _minAge;
            public MinimumAgeAttribute(int minAge)
            {
                _minAge = minAge;
                ErrorMessage = $"You must be at least {minAge} years old to register.";
            }

            public override bool IsValid(object value)
            {
                if (!(value is DateTime dob)) return false;
                var age = DateTime.Today.Year - dob.Year;
                if (dob.Date > DateTime.Today.AddYears(-age)) age--;
                return age >= _minAge;
            }
        
    }

}
