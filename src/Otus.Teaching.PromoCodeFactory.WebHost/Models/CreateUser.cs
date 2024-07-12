using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class CreateUser
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<RoleModel> Roles { get; set; }
    }
}