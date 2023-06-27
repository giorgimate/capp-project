using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Models.Domain
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MaxLength(11)]
        public string PersonalNumber { get; set; }
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
