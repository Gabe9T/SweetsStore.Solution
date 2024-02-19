using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace SweetStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Treat> Treats { get; set; }
    }
}