using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace MovieTimeLibraryCore.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Watch> Watches { get; set; }
        public DbSet<DownloadData> DownloadData { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Nome_BancoDados;Trusted_Connection=True;");
        }
    }
}
