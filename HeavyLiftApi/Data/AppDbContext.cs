using System;
using System.Collections.Generic;
using System.Text.Json;
using HeavyLiftApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HeavyLiftApi.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<customexercise> customexercises { get; set; }

    public virtual DbSet<customtrainingplan> customtrainingplans { get; set; }

    public virtual DbSet<friend> friends { get; set; }

    public virtual DbSet<traininghistory> traininghistories { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<customexercise>(entity =>
        {
            entity.HasKey(e => e.id).HasName("customexercise_pkey");

            entity.ToTable("customexercise");

            entity.Property(e => e.description).HasMaxLength(150);
            entity.Property(e => e.musclegroups).HasMaxLength(300);
            entity.Property(e => e.name).HasMaxLength(30);
            entity.Property(e => e.type).HasMaxLength(30);

            entity.HasOne(d => d.users).WithMany(p => p.customexercises)
                .HasForeignKey(d => d.users_id)
                .HasConstraintName("customexercise_users_fk");
        });

        modelBuilder.Entity<customtrainingplan>(entity =>
        {
            entity.HasKey(e => e.id).HasName("customtrainingplan_pkey");

            entity.ToTable("customtrainingplan");

            entity.Property(e => e.plan).HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    v => JsonSerializer.Deserialize<List<ExerciseEntry>>(v, new JsonSerializerOptions()) ?? new List<ExerciseEntry>()
                );
            entity.Property(e => e.name).HasMaxLength(30);

            entity.HasOne(d => d.users).WithMany(p => p.customtrainingplans)
                .HasForeignKey(d => d.users_id)
                .HasConstraintName("customtrainingplan_users_fk");
        });

        modelBuilder.Entity<friend>(entity =>
        {
            entity.HasKey(e => e.id).HasName("friends_pkey");

            entity.Property(e => e.status).HasMaxLength(10);

            entity.HasOne(d => d.friendNavigation).WithMany(p => p.friendfriendNavigations)
                .HasForeignKey(d => d.friendid)
                .HasConstraintName("friends_friendid_fk");

            entity.HasOne(d => d.user).WithMany(p => p.friendusers)
                .HasForeignKey(d => d.userid)
                .HasConstraintName("friends_userid_fk");
        });

        modelBuilder.Entity<traininghistory>(entity =>
        {
            entity.HasKey(e => e.id).HasName("traininghistory_pkey");

            entity.ToTable("traininghistory");

            entity.Property(e => e.plan).HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    v => JsonSerializer.Deserialize<List<ExerciseEntry>>(v, new JsonSerializerOptions()) ?? new List<ExerciseEntry>()
                );

            entity.Property(e => e.name).HasMaxLength(30);

            entity.HasOne(d => d.users).WithMany(p => p.traininghistories)
                .HasForeignKey(d => d.users_id)
                .HasConstraintName("traininghistory_users_fk");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.Property(e => e.createdat).HasColumnType("timestamp without time zone");
            entity.Property(e => e.email).HasMaxLength(255);
            entity.Property(e => e.nickname).HasMaxLength(50);
            entity.Property(e => e.password).HasMaxLength(100);
            entity.Property(e => e.status).HasMaxLength(1);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
