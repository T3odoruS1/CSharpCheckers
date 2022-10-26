﻿// <auto-generated />
using System;
using DAL.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Db.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11");

            modelBuilder.Entity("Domain.CheckerGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameOptionsId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("GameOverAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("GameWonBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OptionsId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Player1Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("Player1Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Player2Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("Player2Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StarterAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameOptionsId");

                    b.ToTable("CheckerGames");
                });

            modelBuilder.Entity("Domain.CheckerGameOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<bool>("TakingIsMandatory")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WhiteStarts")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CheckerGameOptions");
                });

            modelBuilder.Entity("Domain.CheckerGameState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CheckerGameId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("NextMoveByBlack")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerializedGameBoard")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CheckerGameId");

                    b.ToTable("CheckerGameStates");
                });

            modelBuilder.Entity("Domain.CheckerGame", b =>
                {
                    b.HasOne("Domain.CheckerGameOptions", "GameOptions")
                        .WithMany("CheckerGames")
                        .HasForeignKey("GameOptionsId");

                    b.Navigation("GameOptions");
                });

            modelBuilder.Entity("Domain.CheckerGameState", b =>
                {
                    b.HasOne("Domain.CheckerGame", "CheckerGame")
                        .WithMany("CheckerGameStates")
                        .HasForeignKey("CheckerGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckerGame");
                });

            modelBuilder.Entity("Domain.CheckerGame", b =>
                {
                    b.Navigation("CheckerGameStates");
                });

            modelBuilder.Entity("Domain.CheckerGameOptions", b =>
                {
                    b.Navigation("CheckerGames");
                });
#pragma warning restore 612, 618
        }
    }
}
