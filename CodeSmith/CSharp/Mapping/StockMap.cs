﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenAuth.Repository.Mapping
{
    public partial class StockMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<OpenAuth.Repository.Domain.Stock>
    {
        public StockMap()
        {
            // table
            ToTable("Stock", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.Name)
                .HasColumnName("Name")
                .HasMaxLength(500)
                .IsRequired();
            Property(t => t.Number)
                .HasColumnName("Number")
                .IsRequired();
            Property(t => t.Price)
                .HasColumnName("Price")
                .HasPrecision(10, 1)
                .IsRequired();
            Property(t => t.Status)
                .HasColumnName("Status")
                .IsRequired();
            Property(t => t.Viewable)
                .HasColumnName("Viewable")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.User)
                .HasColumnName("User")
                .HasMaxLength(50)
                .IsRequired();
            Property(t => t.Time)
                .HasColumnName("Time")
                .IsRequired();
            Property(t => t.OrgId)
                .HasColumnName("OrgId")
                .HasMaxLength(50)
                .IsOptional();

            // Relationships
        }
    }
}
