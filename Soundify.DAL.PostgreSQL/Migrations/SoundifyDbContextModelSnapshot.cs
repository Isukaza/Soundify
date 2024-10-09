﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Soundify.DAL.PostgreSQL;

#nullable disable

namespace Soundify.DAL.PostgreSQL.Migrations
{
    [DbContext(typeof(SoundifyDbContext))]
    partial class SoundifyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uuid");

                    b.Property<string>("CoverFilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageFilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId")
                        .IsUnique();

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.ArtistSocialMedia", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Platform")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("ArtistSocialMedias");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.PlayList", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.PlayListTrack", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TrackId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("TrackId");

                    b.ToTable("PlaylistTracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.SingleTrack", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uuid");

                    b.Property<string>("CoverFilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TrackId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("TrackId")
                        .IsUnique();

                    b.ToTable("SingleTracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Track", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AlbumId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RatingCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<double>("TotalRating")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("GenreId");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.TrackRating", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<Guid>("TrackId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.ToTable("TrackRatings");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.UserFavorite", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TrackId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.ToTable("UserFavorites");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Album", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.ArtistSocialMedia", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Artist", "Artist")
                        .WithMany("SocialMediaLinks")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.PlayListTrack", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.PlayList", "PlayList")
                        .WithMany("PlaylistTracks")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Track", "Track")
                        .WithMany("PlaylistTracks")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("PlayList");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.SingleTrack", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Artist", "Artist")
                        .WithMany("Singles")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Track", "Track")
                        .WithOne("Single")
                        .HasForeignKey("Soundify.DAL.PostgreSQL.Models.db.SingleTrack", "TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Track", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Album", "Album")
                        .WithMany("Tracks")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Genre", "Genre")
                        .WithMany("Tracks")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.TrackRating", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Track", "Track")
                        .WithMany("Ratings")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.UserFavorite", b =>
                {
                    b.HasOne("Soundify.DAL.PostgreSQL.Models.db.Track", "Track")
                        .WithMany("UserFavorites")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Album", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Artist", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Singles");

                    b.Navigation("SocialMediaLinks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Genre", b =>
                {
                    b.Navigation("Tracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.PlayList", b =>
                {
                    b.Navigation("PlaylistTracks");
                });

            modelBuilder.Entity("Soundify.DAL.PostgreSQL.Models.db.Track", b =>
                {
                    b.Navigation("PlaylistTracks");

                    b.Navigation("Ratings");

                    b.Navigation("Single");

                    b.Navigation("UserFavorites");
                });
#pragma warning restore 612, 618
        }
    }
}
