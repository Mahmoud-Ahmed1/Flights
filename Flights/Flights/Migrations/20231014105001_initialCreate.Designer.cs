﻿// <auto-generated />
using System;
using Flights.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flights.Migrations
{
    [DbContext(typeof(Entities))]
    [Migration("20231014105001_initialCreate")]
    partial class initialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Flights.Domain.Entities.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Airline")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReaminingNumberOfSeats")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Passenger", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Flight", b =>
                {
                    b.OwnsOne("Flights.Domain.Entities.TimePlace", "Arrival", b1 =>
                        {
                            b1.Property<Guid>("FlightId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Place")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("Time")
                                .HasColumnType("datetime2");

                            b1.HasKey("FlightId");

                            b1.ToTable("Flights");

                            b1.WithOwner()
                                .HasForeignKey("FlightId");
                        });

                    b.OwnsOne("Flights.Domain.Entities.TimePlace", "Departure", b1 =>
                        {
                            b1.Property<Guid>("FlightId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Place")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("Time")
                                .HasColumnType("datetime2");

                            b1.HasKey("FlightId");

                            b1.ToTable("Flights");

                            b1.WithOwner()
                                .HasForeignKey("FlightId");
                        });

                    b.Navigation("Arrival")
                        .IsRequired();

                    b.Navigation("Departure")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}