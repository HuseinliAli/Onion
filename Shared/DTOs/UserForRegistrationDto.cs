using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record UserForRegistrationDto(
        string FirstName, 
        string LastName,
        [Required] string UserName,
        [Required] string Password,
        string Email,
        string PhoneNumber,
        ICollection<string> Roles);
}
