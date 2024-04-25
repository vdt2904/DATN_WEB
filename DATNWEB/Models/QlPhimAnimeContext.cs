using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DATNWEB.Models
{
    public partial class QlPhimAnimeContext : DbContext
    {
        public QlPhimAnimeContext()
        {
        }

        public QlPhimAnimeContext(DbContextOptions<QlPhimAnimeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Anime> Animes { get; set; } = null!;
        public virtual DbSet<CodeRegister> CodeRegisters { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Director> Directors { get; set; } = null!;
        public virtual DbSet<Episode> Episodes { get; set; } = null!;
        public virtual DbSet<FilmGenre> FilmGenres { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<PasswordResetRequest> PasswordResetRequests { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Season> Seasons { get; set; } = null!;
        public virtual DbSet<ServicePackage> ServicePackages { get; set; } = null!;
        public virtual DbSet<ServiceUsage> ServiceUsages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
        public virtual DbSet<View> Views { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=VDT\\SQLEXPRESS;Initial Catalog=QlPhimAnime;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mail).HasMaxLength(100);

                entity.Property(e => e.PassWord).HasMaxLength(500);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Anime>(entity =>
            {
                entity.ToTable("Anime");

                entity.Property(e => e.AnimeId)
                    .HasMaxLength(10)
                    .HasColumnName("Anime_ID");

                entity.Property(e => e.AnimeName)
                    .HasMaxLength(500)
                    .HasColumnName("Anime_Name");

                entity.Property(e => e.BackgroundImageUrl)
                    .HasMaxLength(500)
                    .HasColumnName("BackgroundImage_URL");

                entity.Property(e => e.BroadcastSchedule)
                    .HasColumnType("datetime")
                    .HasColumnName("Broadcast_schedule");

                entity.Property(e => e.DirectorId)
                    .HasMaxLength(10)
                    .HasColumnName("Director_ID");

                entity.Property(e => e.ImageHUrl)
                    .HasMaxLength(500)
                    .HasColumnName("Image_h_URL");

                entity.Property(e => e.ImageVUrl)
                    .HasMaxLength(500)
                    .HasColumnName("Image_v_URL");

                entity.Property(e => e.SeasonId)
                    .HasMaxLength(10)
                    .HasColumnName("Season_ID");

                entity.Property(e => e.TotalEpisode).HasColumnName("Total_episode");

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.Animes)
                    .HasForeignKey(d => d.DirectorId)
                    .HasConstraintName("FK_Anime_Director");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.Animes)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("FK_Anime_Season");
            });

            modelBuilder.Entity<CodeRegister>(entity =>
            {
                entity.ToTable("CodeRegister");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Token).HasMaxLength(10);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment1)
                    .HasMaxLength(500)
                    .HasColumnName("Comment");

                entity.Property(e => e.CommentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Comment_date");

                entity.Property(e => e.EpisodeId)
                    .HasMaxLength(10)
                    .HasColumnName("Episode_ID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.EpisodeId)
                    .HasConstraintName("FK_Comment_Episode");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("Director");

                entity.Property(e => e.DirectorId)
                    .HasMaxLength(10)
                    .HasColumnName("Director_ID");

                entity.Property(e => e.DirectorName)
                    .HasMaxLength(500)
                    .HasColumnName("Director_Name");
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.ToTable("Episode");

                entity.Property(e => e.EpisodeId)
                    .HasMaxLength(10)
                    .HasColumnName("Episode_ID");

                entity.Property(e => e.AnimeId)
                    .HasMaxLength(10)
                    .HasColumnName("Anime_ID");

                entity.Property(e => e.PostingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Posting_date");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(500)
                    .HasColumnName("Video_URL");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(d => d.AnimeId)
                    .HasConstraintName("FK_Episode_Anime");
            });

            modelBuilder.Entity<FilmGenre>(entity =>
            {
                entity.ToTable("FilmGenre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnimeId)
                    .HasMaxLength(10)
                    .HasColumnName("Anime_ID");

                entity.Property(e => e.GenreId)
                    .HasMaxLength(10)
                    .HasColumnName("Genre_ID");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.FilmGenres)
                    .HasForeignKey(d => d.AnimeId)
                    .HasConstraintName("FK_FilmGenre_Anime");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.FilmGenres)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_FilmGenre_Genre");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreId)
                    .HasMaxLength(10)
                    .HasColumnName("Genre_ID");

                entity.Property(e => e.GenreName)
                    .HasMaxLength(500)
                    .HasColumnName("Genre_Name");
            });

            modelBuilder.Entity<PasswordResetRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("PasswordResetRequest");

                entity.Property(e => e.RequestId).HasColumnName("Request_ID");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiration_Date");

                entity.Property(e => e.IsUsed).HasColumnName("Is_Used");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Request_Date");

                entity.Property(e => e.Token).HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordResetRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PasswordResetRequest_User");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnimeId)
                    .HasMaxLength(10)
                    .HasColumnName("Anime_ID");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .HasColumnName("Content_");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.AnimeId)
                    .HasConstraintName("FK_Review_Anime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Review_User");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.Property(e => e.SeasonId)
                    .HasMaxLength(10)
                    .HasColumnName("Season_ID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500)
                    .HasColumnName("Image_URL");

                entity.Property(e => e.PostingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Posting_date");

                entity.Property(e => e.SeasonName)
                    .HasMaxLength(500)
                    .HasColumnName("Season_Name");
            });

            modelBuilder.Entity<ServicePackage>(entity =>
            {
                entity.HasKey(e => e.PackageId);

                entity.ToTable("ServicePackage");

                entity.Property(e => e.PackageId)
                    .HasMaxLength(10)
                    .HasColumnName("Package_ID");

                entity.Property(e => e.PackageName)
                    .HasMaxLength(50)
                    .HasColumnName("Package_Name");

                entity.Property(e => e.ValidityPeriod).HasColumnName("Validity_Period");
            });

            modelBuilder.Entity<ServiceUsage>(entity =>
            {
                entity.ToTable("ServiceUsage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PackageId)
                    .HasMaxLength(10)
                    .HasColumnName("Package_ID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.ServiceUsages)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_ServiceUsage_ServicePackage");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.UserType).HasColumnName("User_Type");

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.ToTable("UserSubscription");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiration_Date");

                entity.Property(e => e.PackageId)
                    .HasMaxLength(10)
                    .HasColumnName("Package_ID");

                entity.Property(e => e.SubscriptionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Subscription_Date");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_UserSubscription_ServicePackage");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserSubscription_User");
            });

            modelBuilder.Entity<View>(entity =>
            {
                entity.ToTable("View");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EpisodeId)
                    .HasMaxLength(10)
                    .HasColumnName("Episode_ID");

                entity.Property(e => e.IsView).HasColumnName("Is_View");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("User_ID");

                entity.Property(e => e.ViewDate)
                    .HasColumnType("datetime")
                    .HasColumnName("View_date");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.EpisodeId)
                    .HasConstraintName("FK_View_Episode");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Views)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_View_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
