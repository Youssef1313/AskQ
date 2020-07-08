using System;
using System.Collections.Generic;
using System.Text;
using AskQ.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AskQ.Infrastructure.Data.Configurations
{
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.ToTable(nameof(Reply));

            builder.HasKey(x => x.Id);
        }
    }
}
