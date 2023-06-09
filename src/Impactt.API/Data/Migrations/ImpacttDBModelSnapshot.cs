﻿// <auto-generated />
using System;
using Impactt.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Impactt.API.Data.Migrations
{
    [DbContext(typeof(ImpacttDB))]
    partial class ImpacttDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Impactt.API.Entities.BookedTime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<string>("Resident")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("resident");

                    b.Property<long>("RoomId")
                        .HasColumnType("bigint")
                        .HasColumnName("room_id");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.HasKey("Id")
                        .HasName("pk_booked_times");

                    b.HasIndex("EndTime")
                        .HasDatabaseName("ix_booked_times_end_time");

                    b.HasIndex("RoomId")
                        .HasDatabaseName("ix_booked_times_room_id");

                    b.HasIndex("StartTime")
                        .HasDatabaseName("ix_booked_times_start_time");

                    b.ToTable("booked_times", (string)null);
                });

            modelBuilder.Entity("Impactt.API.Entities.Room", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer")
                        .HasColumnName("capacity");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_rooms");

                    b.ToTable("rooms", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Capacity = 1,
                            Name = "mytaxi",
                            Type = "focus"
                        },
                        new
                        {
                            Id = 2L,
                            Capacity = 5,
                            Name = "workly",
                            Type = "team"
                        },
                        new
                        {
                            Id = 3L,
                            Capacity = 15,
                            Name = "express24",
                            Type = "conference"
                        },
                        new
                        {
                            Id = 4L,
                            Capacity = 4,
                            Name = "amazon",
                            Type = "focus"
                        },
                        new
                        {
                            Id = 5L,
                            Capacity = 10,
                            Name = "google",
                            Type = "team"
                        },
                        new
                        {
                            Id = 6L,
                            Capacity = 24,
                            Name = "meta",
                            Type = "conference"
                        },
                        new
                        {
                            Id = 7L,
                            Capacity = 2,
                            Name = "uber",
                            Type = "focus"
                        },
                        new
                        {
                            Id = 8L,
                            Capacity = 20,
                            Name = "twitter",
                            Type = "conference"
                        },
                        new
                        {
                            Id = 9L,
                            Capacity = 3,
                            Name = "apple",
                            Type = "focus"
                        },
                        new
                        {
                            Id = 10L,
                            Capacity = 6,
                            Name = "microsoft",
                            Type = "team"
                        },
                        new
                        {
                            Id = 11L,
                            Capacity = 18,
                            Name = "yandex",
                            Type = "conference"
                        },
                        new
                        {
                            Id = 12L,
                            Capacity = 2,
                            Name = "yahoo",
                            Type = "focus"
                        },
                        new
                        {
                            Id = 13L,
                            Capacity = 7,
                            Name = "oracle",
                            Type = "team"
                        },
                        new
                        {
                            Id = 14L,
                            Capacity = 16,
                            Name = "intel",
                            Type = "conference"
                        },
                        new
                        {
                            Id = 15L,
                            Capacity = 2,
                            Name = "ibm",
                            Type = "focus"
                        });
                });

            modelBuilder.Entity("Impactt.API.Entities.BookedTime", b =>
                {
                    b.HasOne("Impactt.API.Entities.Room", "Room")
                        .WithMany("BookedTimes")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_booked_times_rooms_room_id");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Impactt.API.Entities.Room", b =>
                {
                    b.Navigation("BookedTimes");
                });
#pragma warning restore 612, 618
        }
    }
}
