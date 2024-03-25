﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TchotchomereCore.Infrastructure.Sql.DataContexts;

#nullable disable

namespace TchotchomereCore.Infrastructure.Sql.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240325020504_AddRecordsInGenreTable")]
    partial class AddRecordsInGenreTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.DataDownload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("EpisodeTV")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("FilmId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Magnet")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("SeasonTV")
                        .HasColumnType("int");

                    b.Property<Guid?>("SerieId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("SerieId");

                    b.ToTable("DataDownload");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.ExtractedURL", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Html")
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ProcessedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("Trace")
                        .HasColumnType("char(36)");

                    b.Property<string>("URLHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("BaseUrl");

                    b.HasIndex("ProcessedDateTime");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("ExtractedURL");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Film", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("OriginalTitle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Film");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("TheMovieDBCode")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Genre");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9135f939-92dc-4e6d-a78d-3965db200c9c"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(230),
                            Name = "Ação e Aventura",
                            TheMovieDBCode = 10759
                        },
                        new
                        {
                            Id = new Guid("9f57ca25-e44b-443a-853d-20fb1269878e"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(752),
                            Name = "Kids",
                            TheMovieDBCode = 10762
                        },
                        new
                        {
                            Id = new Guid("2bcdafad-1073-4411-b4c1-ebd9b8798e12"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(754),
                            Name = "Ação",
                            TheMovieDBCode = 28
                        },
                        new
                        {
                            Id = new Guid("e93fb403-a634-431e-96d2-ed18d4d539ec"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(756),
                            Name = "Aventura",
                            TheMovieDBCode = 12
                        },
                        new
                        {
                            Id = new Guid("8f745a2c-ed85-4386-914a-01f5868d1ba6"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(757),
                            Name = "Animação",
                            TheMovieDBCode = 16
                        },
                        new
                        {
                            Id = new Guid("8214caf5-27a5-49b8-af67-d7fbc3e47ce2"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(759),
                            Name = "Comédia",
                            TheMovieDBCode = 35
                        },
                        new
                        {
                            Id = new Guid("6a722a42-01b4-4508-b83b-bda626da9546"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(760),
                            Name = "Crime",
                            TheMovieDBCode = 80
                        },
                        new
                        {
                            Id = new Guid("bd35c568-46b0-4578-8549-52342794910d"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(762),
                            Name = "Documentário",
                            TheMovieDBCode = 99
                        },
                        new
                        {
                            Id = new Guid("d8725557-4aa5-40f3-a0be-8c41942d30f7"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(763),
                            Name = "Drama",
                            TheMovieDBCode = 18
                        },
                        new
                        {
                            Id = new Guid("1d99209a-e8dc-479a-9b7e-4ed3ad7a7b01"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(765),
                            Name = "Família",
                            TheMovieDBCode = 10751
                        },
                        new
                        {
                            Id = new Guid("7e00b4c3-8446-43f1-9bb9-81589843e219"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(767),
                            Name = "Fantasia",
                            TheMovieDBCode = 14
                        },
                        new
                        {
                            Id = new Guid("dacb0dce-28fe-4290-8de7-1f9b587a2914"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(804),
                            Name = "História",
                            TheMovieDBCode = 36
                        },
                        new
                        {
                            Id = new Guid("b122bc26-1811-4ef7-b287-454f86aa3903"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(806),
                            Name = "Terror",
                            TheMovieDBCode = 27
                        },
                        new
                        {
                            Id = new Guid("e1399fd5-ae70-4a33-82e0-df55163c7df8"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(807),
                            Name = "Música",
                            TheMovieDBCode = 10402
                        },
                        new
                        {
                            Id = new Guid("a220afc6-a007-4fc5-8c26-0e406c088ddd"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(809),
                            Name = "Mistério",
                            TheMovieDBCode = 9648
                        },
                        new
                        {
                            Id = new Guid("539c4ee6-ef15-416b-b605-6fb7dda07a6a"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(810),
                            Name = "Romance",
                            TheMovieDBCode = 10749
                        },
                        new
                        {
                            Id = new Guid("8405cc2a-a25f-4499-8e0a-e8231cb0d162"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(812),
                            Name = "Ficção Científica",
                            TheMovieDBCode = 878
                        },
                        new
                        {
                            Id = new Guid("0f90f494-f443-4f61-8030-9e8926a8b097"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(813),
                            Name = "Cinema TV",
                            TheMovieDBCode = 10770
                        },
                        new
                        {
                            Id = new Guid("6b3c66f9-3130-461e-8d01-ebca70bf8bb3"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(815),
                            Name = "Thriller",
                            TheMovieDBCode = 53
                        },
                        new
                        {
                            Id = new Guid("587e5f54-606c-4b96-ba75-0f2179b1b2ec"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(816),
                            Name = "Guerra",
                            TheMovieDBCode = 10752
                        },
                        new
                        {
                            Id = new Guid("5b10e095-dcdd-400d-8463-a2b60a15a583"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(818),
                            Name = "Faoreste",
                            TheMovieDBCode = 37
                        },
                        new
                        {
                            Id = new Guid("6986c51a-4a7d-4ad0-aea6-2c782eb69fa0"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(819),
                            Name = "News",
                            TheMovieDBCode = 10763
                        },
                        new
                        {
                            Id = new Guid("7bd06510-f1d6-449e-bcc9-e83a6543e10e"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(821),
                            Name = "Reality",
                            TheMovieDBCode = 10764
                        },
                        new
                        {
                            Id = new Guid("b5ebcd0a-5ac5-4b9c-ac04-35f6fc43ca89"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(822),
                            Name = "Ficção Científica e Fantasia",
                            TheMovieDBCode = 10765
                        },
                        new
                        {
                            Id = new Guid("fef622cd-fe91-4758-b8bd-e2f65bb7a955"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(824),
                            Name = "Soap",
                            TheMovieDBCode = 10766
                        },
                        new
                        {
                            Id = new Guid("e7b7dcf4-b783-447e-af7d-3ec1af27e0b1"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(825),
                            Name = "Talk",
                            TheMovieDBCode = 10767
                        },
                        new
                        {
                            Id = new Guid("b25a7dd5-7c43-4ace-8b74-cda8dc0161d1"),
                            CreatedAt = new DateTime(2024, 3, 25, 2, 5, 4, 5, DateTimeKind.Utc).AddTicks(827),
                            Name = "Guerra e Política",
                            TheMovieDBCode = 10768
                        });
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.InitialURL", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ProcessedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("InitialURL");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Serie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("OriginalTitle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Serie");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Subtitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("DownloadText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("FilmId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Lang")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("SerieId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("SerieId");

                    b.ToTable("Subtitle");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.DataDownload", b =>
                {
                    b.HasOne("TchotchomereCore.Domain.Entities.Film", null)
                        .WithMany("Downloads")
                        .HasForeignKey("FilmId");

                    b.HasOne("TchotchomereCore.Domain.Entities.Serie", null)
                        .WithMany("Downloads")
                        .HasForeignKey("SerieId");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Film", b =>
                {
                    b.OwnsOne("TchotchomereCore.Domain.ValueObjects.ImdbData", "ImdbData", b1 =>
                        {
                            b1.Property<Guid>("FilmId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("BackdropPicture")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("BackdropPicture");

                            b1.Property<string>("Date")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("Date");

                            b1.Property<string>("IDIMDb")
                                .HasColumnType("longtext")
                                .HasColumnName("IDIMDb");

                            b1.Property<int?>("IDTheMovieDB")
                                .HasColumnType("int")
                                .HasColumnName("IDTheMovieDB");

                            b1.Property<string>("PosterPicture")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("PosterPicture");

                            b1.HasKey("FilmId");

                            b1.ToTable("Film");

                            b1.WithOwner()
                                .HasForeignKey("FilmId");
                        });

                    b.Navigation("ImdbData")
                        .IsRequired();
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Serie", b =>
                {
                    b.OwnsOne("TchotchomereCore.Domain.ValueObjects.ImdbData", "ImdbData", b1 =>
                        {
                            b1.Property<Guid>("SerieId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("BackdropPicture")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("BackdropPicture");

                            b1.Property<string>("Date")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("Date");

                            b1.Property<string>("IDIMDb")
                                .HasColumnType("longtext")
                                .HasColumnName("IDIMDb");

                            b1.Property<int?>("IDTheMovieDB")
                                .HasColumnType("int")
                                .HasColumnName("IDTheMovieDB");

                            b1.Property<string>("PosterPicture")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("PosterPicture");

                            b1.HasKey("SerieId");

                            b1.ToTable("Serie");

                            b1.WithOwner()
                                .HasForeignKey("SerieId");
                        });

                    b.Navigation("ImdbData")
                        .IsRequired();
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Subtitle", b =>
                {
                    b.HasOne("TchotchomereCore.Domain.Entities.Film", null)
                        .WithMany("Subtitles")
                        .HasForeignKey("FilmId");

                    b.HasOne("TchotchomereCore.Domain.Entities.Serie", null)
                        .WithMany("Subtitles")
                        .HasForeignKey("SerieId");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Film", b =>
                {
                    b.Navigation("Downloads");

                    b.Navigation("Subtitles");
                });

            modelBuilder.Entity("TchotchomereCore.Domain.Entities.Serie", b =>
                {
                    b.Navigation("Downloads");

                    b.Navigation("Subtitles");
                });
#pragma warning restore 612, 618
        }
    }
}