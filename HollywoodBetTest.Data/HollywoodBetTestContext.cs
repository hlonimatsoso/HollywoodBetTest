using HollywoodBetTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HollywoodBetTest.Data
{
    public class HollywoodBetTestContext : IdentityDbContext<HollywoodBetTestUser>
    {
        public HollywoodBetTestContext()
        { }

        public HollywoodBetTestContext(DbContextOptions<HollywoodBetTestContext> options)
           : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=desktop-6en00hd\\sql2019;Database=HollywoodBetTest;Trusted_Connection=True;");
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }
        public DbSet<EventDetailStatus> EventDetailStatuses { get; set; }

    }
}
