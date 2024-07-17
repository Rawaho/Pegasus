using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pegasus.Configuration;

namespace Pegasus.Database.Model
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Dungeon> Dungeon { get; set; }
        public virtual DbSet<DungeonTile> DungeonTile { get; set; }
        public virtual DbSet<Friend> Friend { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql($"server={ConfigManager.Config.MySql.Host};port={ConfigManager.Config.MySql.Port};user={ConfigManager.Config.MySql.Username}" +
                    $";password={ConfigManager.Config.MySql.Password};database={ConfigManager.Config.MySql.Database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateIp)
                    .IsRequired()
                    .HasColumnName("createIp")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("createTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.LastIp)
                    .IsRequired()
                    .HasColumnName("lastIp")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastTime)
                    .HasColumnName("lastTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Privileges)
                    .HasColumnName("privileges")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Dungeon>(entity =>
            {
                entity.HasKey(e => e.LandBlockId)
                    .HasName("PRIMARY");

                entity.ToTable("dungeon");

                entity.Property(e => e.LandBlockId)
                    .HasColumnName("landBlockId")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<DungeonTile>(entity =>
            {
                entity.ToTable("dungeon_tile");

                entity.HasIndex(e => e.LandBlockId)
                    .HasName("__FK_dungeon_tile_landBlockId__dungeon_landBlockId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LandBlockId)
                    .HasColumnName("landBlockId")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TileId)
                    .HasColumnName("tileId")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.X)
                    .HasColumnName("x")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Y)
                    .HasColumnName("y")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Z)
                    .HasColumnName("z")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.LandBlock)
                    .WithMany(p => p.DungeonTile)
                    .HasForeignKey(d => d.LandBlockId)
                    .HasConstraintName("__FK_dungeon_tile_landBlockId__dungeon_landBlockId");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friend");

                entity.HasIndex(e => e.Friend1)
                    .HasName("__FK_friend_friend__account_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AddTime)
                    .HasColumnName("addTime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Friend1)
                    .HasColumnName("friend")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Friend1Navigation)
                    .WithMany(p => p.FriendFriend1Navigation)
                    .HasForeignKey(d => d.Friend1)
                    .HasConstraintName("__FK_friend_friend__account_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.FriendIdNavigation)
                    .HasForeignKey<Friend>(d => d.Id)
                    .HasConstraintName("__FK_friend_id__account_id");
            });
        }
    }
}
