using ChatStudents_Kazakov.Classes.Common;
using ChatStudents_Kazakov.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatStudents_Kazakov.Classes
{
    public class MessagesContext
    {
        public DbSet<Messages> messages {  get; set; }
        public MessagesContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(Config.config);
    }
}
