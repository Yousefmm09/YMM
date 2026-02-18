using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Infrastructure.Context.Config
{
    public record CategoryResponse(string name ,string desc ,string slug ,bool IsActive);
    
}
