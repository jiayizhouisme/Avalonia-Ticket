using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Context.Models
{
    public class Blog
    {
        [Key]
        public int id { get; set; }
    }
}
