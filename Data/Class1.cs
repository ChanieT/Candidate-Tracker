using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Data
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; } //0=pending; 1=confirmed; 2=declined.
    }

    public class CTContextFactory : IDesignTimeDbContextFactory<CTContext>
    {
        public CTContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}5.6.19"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new CTContext(config.GetConnectionString("ConStr"));
        }
    }

    public class CTContext : DbContext
    {
        private string _conn;
        public CTContext(string conn)
        {
            _conn = conn;
        }

        public DbSet<Candidate> Candidates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conn);
        }
    }

    public class CTRepository
    {
        private string _conn;
        public CTRepository(string conn)
        {
            _conn = conn;
        }

        public void AddCandidate(Candidate c)
        {
            using (var context = new CTContext(_conn))
            {
                var candidate = new Candidate
                {
                    Name = c.Name,
                    Email = c.Email,
                    Number = c.Number,
                    Notes = c.Notes,
                    Status = 0
                };
                context.Candidates.Add(candidate);
                context.SaveChanges();
            }
        }
        public IEnumerable<Candidate> GetPendingCandidates()
        {
            using (var context = new CTContext(_conn))
            {
                return context.Candidates.Where(c => c.Status == 0).ToList();
            }
        }

        public Candidate GetCandidateById(int id)
        {
            using (var context = new CTContext(_conn))
            {
                return context.Candidates.FirstOrDefault(c => c.Id == id);
            }
        }

        public IEnumerable<Candidate> GetConfirmedCandidates()
        {
            using (var context = new CTContext(_conn))
            {
                return context.Candidates.Where(c => c.Status == 1).ToList();
            }
        }

        public IEnumerable<Candidate> GetDeclinedCandidates()
        {
            using (var context = new CTContext(_conn))
            {
                return context.Candidates.Where(c => c.Status == 2).ToList();
            }
        }

        public int GetPendingAmount()
        {
            using (var context = new CTContext(_conn))
            {
                return GetPendingCandidates().Count();
            }
        }

        public int GetConfirmedAmount()
        {
            using (var context = new CTContext(_conn))
            {
                return GetConfirmedCandidates().Count();
            }
        }

        public int GetDeclinedAmount()
        {
            using (var context = new CTContext(_conn))
            {
                return GetDeclinedCandidates().Count();
            }
        }

        public void ConfirmCandidate(int id)
        {
            using (var context = new CTContext(_conn))
            {
                var candidate= context.Candidates.FirstOrDefault(c => c.Id == id);
                candidate.Status = 1;
                context.Candidates.Update(candidate);
                context.SaveChanges();
            }
        }

        public void DeclineCandidate(int id)
        {
            using (var context = new CTContext(_conn))
            {
                var candidate = context.Candidates.FirstOrDefault(c => c.Id == id);
                candidate.Status = 2;
                context.Candidates.Update(candidate);
                context.SaveChanges();
            }
        }
    }
}
