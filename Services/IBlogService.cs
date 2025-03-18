using GetStartedApp.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Services
{
    public interface IBlogService
    {
        Task<Blog> InsertAsync(Blog blog);
        Task<List<Blog>> GetAsync();
    }
}
