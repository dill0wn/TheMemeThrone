﻿// <auto-generated />
using System;
using MemeThroneBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TheMemeThrone.Migrations
{
    [DbContext(typeof(MemingContext))]
    partial class MemingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("MemeThroneBot.CaptionCard", b =>
                {
                    b.Property<int>("CaptionCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("CaptionCardId");

                    b.ToTable("CaptionCards");
                });

            modelBuilder.Entity("MemeThroneBot.GameState", b =>
                {
                    b.Property<int>("GameStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Channel")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Guild")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("GameStateId");

                    b.HasIndex("Guild")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("MemeThroneBot.MemeCard", b =>
                {
                    b.Property<int>("MemeCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("MemeCardId");

                    b.ToTable("MemeCards");
                });

            modelBuilder.Entity("MemeThroneBot.PlayerState", b =>
                {
                    b.Property<int>("PlayerStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("User")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerStateId");

                    b.HasIndex("GameStateId");

                    b.ToTable("PlayerState");
                });

            modelBuilder.Entity("MemeThroneBot.PlayerState", b =>
                {
                    b.HasOne("MemeThroneBot.GameState", null)
                        .WithMany("Players")
                        .HasForeignKey("GameStateId");
                });

            modelBuilder.Entity("MemeThroneBot.GameState", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
