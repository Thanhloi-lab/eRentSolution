using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserAvatarUpdateRequest
    {
       public Guid Id { get; set; }
       public IFormFile AvatarFile { get; set; }
    }
}
