using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_1_Cookie_Authenticaion.Models
{
    public class seedData
    {
  
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
                {
                    if (context.users.Any())
                    {
                        return;
                    }

                    context.users.AddRange(
                       new User { Id = 1, Name = "Murat", SurName = "Karakuş", Email = "mkarakus2013@gmail.com", Password = "123456", UserName = "Mk" },
                       new User { Id = 2, Name = "Cafer", SurName = "Karakuş", Email = "caferkarakus@yandex.com.tr", Password = "123456", UserName = "Ck" },
                       new User { Id = 3, Name = "Gökdeniz", SurName = "Karakuş", Email = "2010karakus@gmail.com", Password = "123456", UserName = "Gk" });

                    context.SaveChanges();
                }
            }
        }
    }
