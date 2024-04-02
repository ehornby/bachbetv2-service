using BachBetV2.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BachBetV2.Infrastructure.Database.Contexts
{
    public class BachBetContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BetEntity> Bets { get; set; }
        public DbSet<LedgerEntity> Ledger { get; set; }
        public DbSet<ChallengeEntity> Challenges { get; set; }
        public DbSet<ChallengeClaimEntity> ChallengeClaims { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<PushSubscriptionEntity> PushSubscriptions { get; set; }

        public BachBetContext(DbContextOptions<BachBetContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Bets)
                .WithMany(b => b.Takers)
                .UsingEntity(e => e.ToTable("TakenBets"));

            modelBuilder.Entity<ChallengeClaimEntity>()
                .HasOne(cc => cc.Claimaint)
                .WithMany(u => u.ChallengeClaims);

            modelBuilder.Entity<ChallengeClaimEntity>()
                .HasOne(cc => cc.Witness)
                .WithMany(u => u.ChallengeWitnesses);

            modelBuilder.Entity<BetEntity>()
                .HasMany(b => b.Tags)
                .WithMany(t => t.Bets)
                .UsingEntity(e => e.ToTable("BetTags"));

            modelBuilder.Entity<BetEntity>().ToTable("Bets")
                .Property(p => p.Status)
                .HasConversion<int>();

            modelBuilder.Entity<BetEntity>().ToTable("Bets")
                .Property(p => p.Result)
                .HasConversion<int>();

            modelBuilder.Entity<LedgerEntity>().ToTable("Ledger")
                .Property(p => p.TransactionType)
                .HasConversion<int>();

            modelBuilder.Entity<ChallengeClaimEntity>().ToTable("ChallengeClaims")
                .Property(p => p.Status)
                .HasConversion<int>();

            modelBuilder.Entity<TagEntity>()
                .HasMany(t => t.Challenges)
                .WithMany(c => c.Tags)
                .UsingEntity(e => e.ToTable("ChallengeTags"));

            modelBuilder.Entity<PushSubscriptionEntity>()
                .HasOne(ps => ps.User)
                .WithOne(u => u.PushSubscription);

            base.OnModelCreating(modelBuilder);
        }
    }
}
