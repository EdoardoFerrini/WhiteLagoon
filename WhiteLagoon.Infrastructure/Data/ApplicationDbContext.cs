﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Beach Villa",
                    Price = 1000000,
                    SquareFoot = 1000,
                    Description = "This is a villa placed near the sea.",
                    ImageUrl = "",
                    Occupancy = 5,
                    CreatedTime = DateTime.Now,
                },
                new Villa
                {
                    Id = 2,
                    Name = "City Villa",
                    Price = 1500000,
                    SquareFoot = 1200,
                    Description = "This is a villa close to Colosseo.",
                    ImageUrl = "",
                    Occupancy = 5,
                    CreatedTime = DateTime.Now
                }
                );
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 100,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 102,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 200,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 201,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 202,
                    VillaId = 2,
                }
                );
        }
    }
}