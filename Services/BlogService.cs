using GetStartedApp.Context;
using GetStartedApp.Context.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppEFContext appEFContext;
        public BlogService(AppEFContext appEFContext) {
            this.appEFContext = appEFContext;
        }
        public async Task<Blog> InsertAsync(Blog blog)
        {
            var entity = this.appEFContext.Add(blog);
            await this.appEFContext.SaveChangesAsync();
            return blog;
        }

        public async Task<List<Blog>> GetAsync()
        {
            var list = await this.appEFContext.Blogs.AsQueryable().ToListAsync();
            return list;
        }
    }
}
