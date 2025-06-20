﻿using Microsoft.EntityFrameworkCore;
using System;

namespace Hospital
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = true;
        }
    }
}
