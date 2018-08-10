using Microsoft.EntityFrameworkCore;
using CallGate.Models;
using EntityFrameworkCore.Triggers;

namespace CallGate.Data
{
    public class DatabaseContext : DbContextWithTriggers
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnUserModelCreating(modelBuilder);
            
            OnGroupModelCreating(modelBuilder);   
            OnGroupUserModelCreating(modelBuilder);  
            
            OnChatModelCreating(modelBuilder);
            OnChatUserModelCreating(modelBuilder);
            
            OnChannelModelCreating(modelBuilder);
            OnChannelUserModelCreating(modelBuilder);
        }

        protected void OnUserModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .HasColumnType("varchar(255)")
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasColumnType("varchar(255)")
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.IsActive)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.ConfirmationPhrase)
                .HasColumnType("varchar(255)")
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(user => user.ConfirmationPhrase)
                .IsUnique();
        }
        
        protected void OnGroupModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .Property(group => group.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();
            
            modelBuilder.Entity<Group>()
                .Property(group => group.Description)
                .HasColumnType("varchar(768)")
                .IsRequired();
            
            modelBuilder.Entity<Group>()
                .Property(group => group.CreatedAt)
                .IsRequired();
            
            modelBuilder.Entity<Group>()
                .Property(group => group.UpdatedAt)
                .IsRequired();
        }

        protected void OnGroupUserModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.UserId, gu.GroupId });

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(gu => gu.UserId);

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupUsers)
                .HasForeignKey(gu => gu.GroupId);
            
            modelBuilder.Entity<GroupUser>()
                .Property(gu => gu.Role)
                .IsRequired();
        }
        
        protected void OnChatModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Group);
            
            modelBuilder.Entity<Chat>()
                .Property(chat => chat.CreatedAt)
                .IsRequired();
        }
        
        protected void OnChatUserModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatUser>()
                .HasKey(cu => new { cu.UserId, cu.ChatId });

            modelBuilder.Entity<ChatUser>()
                .HasOne(cu => cu.User)
                .WithMany(c => c.ChatUsers)
                .HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<ChatUser>()
                .HasOne(cu => cu.Chat)
                .WithMany(c => c.ChatUsers)
                .HasForeignKey(cu => cu.ChatId);
        }
        
        protected void OnChannelModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasOne(c => c.Group);
            
            modelBuilder.Entity<Channel>()
                .Property(channel => channel.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();
            
            modelBuilder.Entity<Channel>()
                .Property(channel => channel.CreatedAt)
                .IsRequired();
        }
        
        protected void OnChannelUserModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelUser>()
                .HasKey(cu => new { cu.UserId, cu.ChannelId });

            modelBuilder.Entity<ChannelUser>()
                .HasOne(cu => cu.User)
                .WithMany(c => c.ChannelUsers)
                .HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<ChannelUser>()
                .HasOne(cu => cu.Channel)
                .WithMany(c => c.ChannelUsers)
                .HasForeignKey(cu => cu.ChannelId);
        }
    }
}
