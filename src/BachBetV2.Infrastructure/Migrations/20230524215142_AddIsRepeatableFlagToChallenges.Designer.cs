﻿// <auto-generated />
using System;
using BachBetV2.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    [DbContext(typeof(BachBetContext))]
    [Migration("20230524215142_AddIsRepeatableFlagToChallenges")]
    partial class AddIsRepeatableFlagToChallenges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.BetEntity", b =>
                {
                    b.Property<int>("BetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BetId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Odds")
                        .HasColumnType("numeric");

                    b.Property<int?>("Result")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("BetId");

                    b.ToTable("Bets", (string)null);
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.ChallengeClaimEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChallengeId")
                        .HasColumnType("integer");

                    b.Property<int>("ClaimaintId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("WitnessId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("ClaimaintId");

                    b.HasIndex("WitnessId");

                    b.ToTable("ChallengeClaims", (string)null);
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.ChallengeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRepeatable")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Reward")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Challenges");
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.LedgerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int?>("BetId")
                        .HasColumnType("integer");

                    b.Property<int?>("ChallengeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.Property<string>("TransferMessage")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Ledger", (string)null);
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.TagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("TagDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Token")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BetEntityTagEntity", b =>
                {
                    b.Property<int>("BetsBetId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("BetsBetId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("BetTags", (string)null);
                });

            modelBuilder.Entity("BetEntityUserEntity", b =>
                {
                    b.Property<int>("BetsBetId")
                        .HasColumnType("integer");

                    b.Property<int>("TakersId")
                        .HasColumnType("integer");

                    b.HasKey("BetsBetId", "TakersId");

                    b.HasIndex("TakersId");

                    b.ToTable("TakenBets", (string)null);
                });

            modelBuilder.Entity("ChallengeEntityTagEntity", b =>
                {
                    b.Property<int>("ChallengesId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("ChallengesId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("ChallengeTags", (string)null);
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.ChallengeClaimEntity", b =>
                {
                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.ChallengeEntity", "Challenge")
                        .WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.UserEntity", "Claimaint")
                        .WithMany("ChallengeClaims")
                        .HasForeignKey("ClaimaintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.UserEntity", "Witness")
                        .WithMany("ChallengeWitnesses")
                        .HasForeignKey("WitnessId");

                    b.Navigation("Challenge");

                    b.Navigation("Claimaint");

                    b.Navigation("Witness");
                });

            modelBuilder.Entity("BetEntityTagEntity", b =>
                {
                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.BetEntity", null)
                        .WithMany()
                        .HasForeignKey("BetsBetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BetEntityUserEntity", b =>
                {
                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.BetEntity", null)
                        .WithMany()
                        .HasForeignKey("BetsBetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("TakersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChallengeEntityTagEntity", b =>
                {
                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.ChallengeEntity", null)
                        .WithMany()
                        .HasForeignKey("ChallengesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BachBetV2.Infrastructure.Database.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BachBetV2.Infrastructure.Database.Entities.UserEntity", b =>
                {
                    b.Navigation("ChallengeClaims");

                    b.Navigation("ChallengeWitnesses");
                });
#pragma warning restore 612, 618
        }
    }
}
