﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Prism_EndPoint.Models;

public partial class PrismDbContext : DbContext
{
    public PrismDbContext()
    {
    }

    public PrismDbContext(DbContextOptions<PrismDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DivisionProcess> DivisionProcesses { get; set; }

    public virtual DbSet<FrequencyAudit> FrequencyAudits { get; set; }

    public virtual DbSet<FrequencyMonth> FrequencyMonths { get; set; }

    public virtual DbSet<PrismCredential> PrismCredentials { get; set; }

    public virtual DbSet<QmsPlan> QmsPlans { get; set; }

    public virtual DbSet<QmsPlanAudit> QmsPlanAudits { get; set; }

    public virtual DbSet<QmsProgram> QmsPrograms { get; set; }

    public virtual DbSet<Qmsprocess> Qmsprocesses { get; set; }

    public virtual DbSet<QmssubProcess> QmssubProcesses { get; set; }

    public virtual DbSet<Qmsteam> Qmsteams { get; set; }

    public virtual DbSet<QmsteamMember> QmsteamMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=IMTS-1\\SQLEXPRESS;Database=prism_db;User Id=sa;Password=Nfrdi@2024!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DivisionProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Division__3214EC070BC6C43E");

            entity.ToTable("DivisionProcess");

            entity.Property(e => e.DivisionId)
                .HasMaxLength(5)
                .HasColumnName("Division_id");
            entity.Property(e => e.ProcessOwnerId).HasMaxLength(50);

            entity.HasOne(d => d.Process).WithMany(p => p.DivisionProcesses)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__DivisionP__Proce__0DAF0CB0");
        });

        modelBuilder.Entity<FrequencyAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Frequenc__3214EC0725869641");

            entity.ToTable("FrequencyAudit");

            entity.HasOne(d => d.AuditTeamNavigation).WithMany(p => p.FrequencyAudits)
                .HasForeignKey(d => d.AuditTeam)
                .HasConstraintName("FK__Frequency__Audit__276EDEB3");

            entity.HasOne(d => d.Program).WithMany(p => p.FrequencyAudits)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__Frequency__Progr__286302EC");
        });

        modelBuilder.Entity<FrequencyMonth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Frequenc__3214EC07398D8EEE");

            entity.ToTable("FrequencyMonth");

            entity.HasOne(d => d.Frequency).WithMany(p => p.FrequencyMonths)
                .HasForeignKey(d => d.FrequencyId)
                .HasConstraintName("FK__Frequency__Frequ__3B75D760");
        });

        modelBuilder.Entity<PrismCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrismCre__3214EC074222D4EF");

            entity.Property(e => e.AuditHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Qmsleader)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("QMSLEADER");
            entity.Property(e => e.ViceAuditHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ViceQmsleader)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ViceQMSLEADER");
        });

        modelBuilder.Entity<QmsPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QmsPlan__3214EC074AB81AF0");

            entity.ToTable("QmsPlan");

            entity.Property(e => e.Approve).HasMaxLength(50);
            entity.Property(e => e.AuditMemo).HasMaxLength(255);
            entity.Property(e => e.AuditObj).HasMaxLength(255);
            entity.Property(e => e.AuditScope).HasMaxLength(255);
            entity.Property(e => e.DocumentNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Frequency).WithMany(p => p.QmsPlans)
                .HasForeignKey(d => d.FrequencyId)
                .HasConstraintName("FK_QmsPlan_FrequencyId");

            entity.HasOne(d => d.Process).WithMany(p => p.QmsPlans)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_QmsPlan_ProcessId");

            entity.HasOne(d => d.Program).WithMany(p => p.QmsPlans)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QmsPlan__Program__4CA06362");
        });

        modelBuilder.Entity<QmsPlanAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QmsPlanA__3214EC075535A963");

            entity.ToTable("QmsPlanAudit");

            entity.Property(e => e.AuditCriteria).HasMaxLength(255);
            entity.Property(e => e.ProcessOwner).HasMaxLength(255);
            entity.Property(e => e.TeamId).HasColumnName("teamId");

            entity.HasOne(d => d.Plan).WithMany(p => p.QmsPlanAudits)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QmsPlanAu__PlanI__571DF1D5");

            entity.HasOne(d => d.SubProcess).WithMany(p => p.QmsPlanAudits)
                .HasForeignKey(d => d.SubProcessId)
                .HasConstraintName("FK__QmsPlanAu__SubPr__5812160E");

            entity.HasOne(d => d.Team).WithMany(p => p.QmsPlanAudits)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__QmsPlanAu__teamI__59063A47");
        });

        modelBuilder.Entity<QmsProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QmsProgr__3214EC0703317E3D");

            entity.ToTable("QmsProgram");

            entity.Property(e => e.ApprovedAuditHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedQmslead)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ApprovedQMSLEAD");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.DocumentNum).HasMaxLength(50);
            entity.Property(e => e.ProgramSecVi).HasColumnName("ProgramSecVI");
            entity.Property(e => e.ProgramSecVii).HasColumnName("ProgramSecVII");
            entity.Property(e => e.QmsauditLead).HasColumnName("QMSauditLead");
            entity.Property(e => e.Qmsleader).HasColumnName("QMSleader");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Qmsprocess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QMSproce__3214EC0707F6335A");

            entity.ToTable("QMSprocess");
        });

        modelBuilder.Entity<QmssubProcess>(entity =>
        {
            entity.HasKey(e => e.SubProcessId).HasName("PK__QMSsubPr__F054A8AC45F365D3");

            entity.ToTable("QMSsubProcess");

            entity.Property(e => e.SubProcessName).HasMaxLength(255);

            entity.HasOne(d => d.Process).WithMany(p => p.QmssubProcesses)
                .HasForeignKey(d => d.ProcessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QMSsubProcess_QMSprocess");
        });

        modelBuilder.Entity<Qmsteam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QMSTeam__3214EC071CF15040");

            entity.ToTable("QMSTeam");

            entity.Property(e => e.TeamLeader).HasMaxLength(5);
        });

        modelBuilder.Entity<QmsteamMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QMSTeamM__3214EC0720C1E124");

            entity.ToTable("QMSTeamMember");

            entity.Property(e => e.Member).HasMaxLength(5);

            entity.HasOne(d => d.Team).WithMany(p => p.QmsteamMembers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__QMSTeamMe__TeamI__22AA2996");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
