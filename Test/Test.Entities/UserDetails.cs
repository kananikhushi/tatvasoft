﻿using Microsoft.AspNetCore.Http; // ✅ correct
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Test.Entities
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string UserType { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile  ProfileImage
        { get; set; }


    }
}
