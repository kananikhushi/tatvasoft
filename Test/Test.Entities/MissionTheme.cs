using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.Threading.Tasks;

namespace Test.Entities
{
    public class MissionTheme
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ThemeName { get; set; }
        public string Status { get; set; }
    }
}
