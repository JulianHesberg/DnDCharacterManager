using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMicroservice.Application.Dtos
{
    public class CreateSkillDto
    {
      
        public int Cost { get; set; }
  
        public string Description { get; set; }
    }
}
