﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RPS_Final_Version.Models;

#nullable disable

namespace RPS_Final_Version.Migrations
{
    [DbContext(typeof(rock_paper_scissorsContext))]
    [Migration("20220207234906_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RPS_Final_Version.Models.Choice", b =>
                {
                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("DESCRIPTION");

                    b.HasKey("Description")
                        .HasName("PK__CHOICE__4193D92F491B2636");

                    b.ToTable("CHOICE", (string)null);
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Game", b =>
                {
                    b.Property<int>("Gameid")
                        .HasColumnType("int")
                        .HasColumnName("GAMEID");

                    b.Property<DateTime?>("Datetimeended")
                        .HasColumnType("datetime")
                        .HasColumnName("DATETIMEENDED");

                    b.Property<DateTime>("Datetimestarted")
                        .HasColumnType("datetime")
                        .HasColumnName("DATETIMESTARTED");

                    b.Property<string>("GameWinner")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("GAME_WINNER");

                    b.Property<string>("Gamecode")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("GAMECODE");

                    b.Property<string>("PlayerOne")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("PLAYER_ONE");

                    b.Property<string>("PlayerTwo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("PLAYER_TWO");

                    b.Property<int>("Roundlimit")
                        .HasColumnType("int")
                        .HasColumnName("ROUNDLIMIT");

                    b.HasKey("Gameid");

                    b.HasIndex("PlayerOne");

                    b.HasIndex("PlayerTwo");

                    b.ToTable("GAME", (string)null);
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Player", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("USERNAME");

                    b.HasKey("Username")
                        .HasName("PK__PLAYER__B15BE12FD01F52A9");

                    b.ToTable("PLAYER", (string)null);
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Round", b =>
                {
                    b.Property<int>("Roundnumber")
                        .HasColumnType("int")
                        .HasColumnName("ROUNDNUMBER");

                    b.Property<int>("Gameid")
                        .HasColumnType("int")
                        .HasColumnName("GAMEID");

                    b.Property<string>("PlayerOneChoice")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("PLAYER_ONE_CHOICE");

                    b.Property<string>("PlayerTwoChoice")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("PLAYER_TWO_CHOICE");

                    b.Property<string>("Winner")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)")
                        .HasColumnName("WINNER");

                    b.HasKey("Roundnumber", "Gameid", "PlayerOneChoice", "PlayerTwoChoice")
                        .HasName("PK__ROUND__C3DB5F0D48FFB294");

                    b.HasIndex("Gameid");

                    b.HasIndex("PlayerOneChoice");

                    b.HasIndex("PlayerTwoChoice");

                    b.ToTable("ROUND", (string)null);
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Game", b =>
                {
                    b.HasOne("RPS_Final_Version.Models.Player", "PlayerOneNavigation")
                        .WithMany("GamePlayerOneNavigations")
                        .HasForeignKey("PlayerOne")
                        .IsRequired()
                        .HasConstraintName("FK__GAME__PLAYER_ONE__503BEA1C");

                    b.HasOne("RPS_Final_Version.Models.Player", "PlayerTwoNavigation")
                        .WithMany("GamePlayerTwoNavigations")
                        .HasForeignKey("PlayerTwo")
                        .IsRequired()
                        .HasConstraintName("FK__GAME__PLAYER_TWO__51300E55");

                    b.Navigation("PlayerOneNavigation");

                    b.Navigation("PlayerTwoNavigation");
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Round", b =>
                {
                    b.HasOne("RPS_Final_Version.Models.Game", "Game")
                        .WithMany("Rounds")
                        .HasForeignKey("Gameid")
                        .IsRequired()
                        .HasConstraintName("FK__ROUND__GAMEID__59C55456");

                    b.HasOne("RPS_Final_Version.Models.Choice", "PlayerOneChoiceNavigation")
                        .WithMany("RoundPlayerOneChoiceNavigations")
                        .HasForeignKey("PlayerOneChoice")
                        .IsRequired()
                        .HasConstraintName("FK__ROUND__PLAYER_ON__5AB9788F");

                    b.HasOne("RPS_Final_Version.Models.Choice", "PlayerTwoChoiceNavigation")
                        .WithMany("RoundPlayerTwoChoiceNavigations")
                        .HasForeignKey("PlayerTwoChoice")
                        .IsRequired()
                        .HasConstraintName("FK__ROUND__PLAYER_TW__5BAD9CC8");

                    b.Navigation("Game");

                    b.Navigation("PlayerOneChoiceNavigation");

                    b.Navigation("PlayerTwoChoiceNavigation");
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Choice", b =>
                {
                    b.Navigation("RoundPlayerOneChoiceNavigations");

                    b.Navigation("RoundPlayerTwoChoiceNavigations");
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Game", b =>
                {
                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("RPS_Final_Version.Models.Player", b =>
                {
                    b.Navigation("GamePlayerOneNavigations");

                    b.Navigation("GamePlayerTwoNavigations");
                });
#pragma warning restore 612, 618
        }
    }
}