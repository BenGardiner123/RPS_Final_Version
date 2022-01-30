using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RPS_Final_Version.Models
{
    public partial class rock_paper_scissorsContext : DbContext
    {
        public rock_paper_scissorsContext()
        {
        }

        public rock_paper_scissorsContext(DbContextOptions<rock_paper_scissorsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Choice> Choices { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Round> Rounds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;Database=rock_paper_scissors;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Choice>(entity =>
            {
                entity.HasKey(e => e.Description)
                    .HasName("PK__CHOICE__4193D92F4897F363");

                entity.ToTable("CHOICE");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("DESCRIPTION");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("GAME");

                entity.Property(e => e.Gameid)
                    .ValueGeneratedNever()
                    .HasColumnName("GAMEID");

                entity.Property(e => e.Datetimeended)
                    .HasColumnType("datetime")
                    .HasColumnName("DATETIMEENDED");

                entity.Property(e => e.Datetimestarted)
                    .HasColumnType("datetime")
                    .HasColumnName("DATETIMESTARTED");

                entity.Property(e => e.Gamecode)
                    .HasMaxLength(255)
                    .HasColumnName("GAMECODE");

                entity.Property(e => e.GamerWinner)
                    .HasMaxLength(255)
                    .HasColumnName("GAMER_WINNER");

                entity.Property(e => e.PlayerOne)
                    .HasMaxLength(255)
                    .HasColumnName("PLAYER_ONE");

                entity.Property(e => e.PlayerTwo)
                    .HasMaxLength(255)
                    .HasColumnName("PLAYER_TWO");

                entity.Property(e => e.Roundlimit).HasColumnName("ROUNDLIMIT");

                entity.HasOne(d => d.PlayerOneNavigation)
                    .WithMany(p => p.GamePlayerOneNavigations)
                    .HasForeignKey(d => d.PlayerOne)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GAME__PLAYER_ONE__236943A5");

                entity.HasOne(d => d.PlayerTwoNavigation)
                    .WithMany(p => p.GamePlayerTwoNavigations)
                    .HasForeignKey(d => d.PlayerTwo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GAME__PLAYER_TWO__245D67DE");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__PLAYER__B15BE12F66EB9D4D");

                entity.ToTable("PLAYER");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("USERNAME");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.HasKey(e => new { e.Roundnumber, e.Gameid, e.PlayerOneChoice, e.PlayerTwoChoice })
                    .HasName("PK__ROUND__C3DB5F0DC7CF8D00");

                entity.ToTable("ROUND");

                entity.Property(e => e.Roundnumber).HasColumnName("ROUNDNUMBER");

                entity.Property(e => e.Gameid).HasColumnName("GAMEID");

                entity.Property(e => e.PlayerOneChoice)
                    .HasMaxLength(50)
                    .HasColumnName("PLAYER_ONE_CHOICE");

                entity.Property(e => e.PlayerTwoChoice)
                    .HasMaxLength(50)
                    .HasColumnName("PLAYER_TWO_CHOICE");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Rounds)
                    .HasForeignKey(d => d.Gameid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ROUND__GAMEID__2BFE89A6");

                entity.HasOne(d => d.PlayerOneChoiceNavigation)
                    .WithMany(p => p.RoundPlayerOneChoiceNavigations)
                    .HasForeignKey(d => d.PlayerOneChoice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ROUND__PLAYER_ON__2CF2ADDF");

                entity.HasOne(d => d.PlayerTwoChoiceNavigation)
                    .WithMany(p => p.RoundPlayerTwoChoiceNavigations)
                    .HasForeignKey(d => d.PlayerTwoChoice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ROUND__PLAYER_TW__2DE6D218");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
