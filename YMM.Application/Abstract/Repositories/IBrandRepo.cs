using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Data.Entities;

namespace YMM.Application.Abstract.Repositories
{
    public interface IBrandRepo
    {
        Task<Brand> AddAsync(Brand brand);
    }
}
