﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;
using OpenAuth.Domain;

namespace OpenAuth.Repository.Models.Mapping
{
    public partial class GoodsApplyMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<GoodsApply>
    {
        public GoodsApplyMap()
        {
            // table
            ToTable("GoodsApply", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .IsRequired();
            Property(t => t.Sort)
                .HasColumnName("Sort")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.Number)
                .HasColumnName("Number")
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(256)
                .IsOptional();
            Property(t => t.Comment)
                .HasColumnName("Comment")
                .IsRequired();
            Property(t => t.State)
                .HasColumnName("State")
                .HasMaxLength(1024)
                .IsRequired();
            Property(t => t.StateName)
                .HasColumnName("StateName")
                .HasMaxLength(1024)
                .IsOptional();
            Property(t => t.UserId)
                .HasColumnName("UserId")
                .IsRequired();
            Property(t => t.ControllerUserId)
                .HasColumnName("ControllerUserId")
                .IsOptional();

            // Relationships
        }
    }
}