using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
	public partial class LibraryDBContext : DbContext
	{
		public LibraryDBContext()
		{ }
		public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
		{

		}
		public virtual DbSet<Author> Authors { get; set; }

		public virtual DbSet<Book> Books { get; set; }

		public virtual DbSet<Category> Categories { get; set; }

		public virtual DbSet<BookTransaction> Transactions { get; set; }

		public virtual DbSet<Reservation> Reservations { get; set; }

		public virtual DbSet<Role> Roles { get; set; }

		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<UserLogin> UserLogins { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Author>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC0797E07FA2");

				entity.Property(e => e.AuthorBirthDate)
					.HasDefaultValueSql("(getdate())")
					.HasColumnType("datetime");
				entity.Property(e => e.AuthorName).HasMaxLength(50);
				entity.Property(e => e.AuthorSurname).HasMaxLength(50);
			});


			modelBuilder.Entity<Book>(entity =>
			{
				entity.HasKey(e => e.BookId).HasName("PK__Books__3214EC077261F067");

				entity.Property(e => e.BookPublishedYear).HasDefaultValueSql("(getdate())");
				entity.Property(e => e.BookTitle).HasMaxLength(100);
				entity.HasOne(d => d.Author).WithMany(p => p.Books)
					.HasForeignKey(d => d.AuthorId)
					.HasConstraintName("FK__Books__AuthorId__5165187F");

				entity.HasOne(d => d.Category).WithMany(p => p.Books)
					.HasForeignKey(d => d.CategoryId)
					.HasConstraintName("FK__Books__CategoryI__52593CB8");

			});

			modelBuilder.Entity<Category>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07443AE0CF");

				entity.Property(e => e.CategoryName).HasMaxLength(50);
				entity.HasData(
					new Category { Id = 1, CategoryName = "Fiction" },
					new Category { Id = 2, CategoryName = "Non-Fiction" },
					new Category { Id = 3, CategoryName = "Science" },
					new Category { Id = 4, CategoryName = "History" },
					new Category { Id = 5, CategoryName = "Biography" },
					new Category { Id = 6, CategoryName = "Fantasy" },
					new Category { Id = 7, CategoryName = "Romance" },
					new Category { Id = 8, CategoryName = "Mystery" },
					new Category { Id = 9, CategoryName = "Self-Help" },
					new Category { Id = 10, CategoryName = "Children's Books" },
					new Category { Id = 11, CategoryName = "Young Adult" },
					new Category { Id = 12, CategoryName = "Adventure" },
					new Category { Id = 13, CategoryName = "Horror" },
					new Category { Id = 14, CategoryName = "Philosophy" },
					new Category { Id = 15, CategoryName = "Science Fiction" }
				);
			});

			modelBuilder.Entity<BookTransaction>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Transactions__3214EC07486A713F");

				entity.HasOne(d => d.Book).WithMany(p => p.Transactions)
					.HasForeignKey(d => d.BookId)
					.HasConstraintName("FK__Transactions__BookId__66603565");

				entity.HasOne(d => d.User).WithMany(p => p.Transactions)
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("FK__Transactions__UserId__6754599E");
			});
			modelBuilder.Entity<Reservation>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC079D639DAD");

				entity.Property(e => e.ExpirationDate).HasDefaultValueSql("(getdate())");
				entity.Property(e => e.ReservationDate).HasDefaultValueSql("(getdate())");

				entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
					.HasForeignKey(d => d.BookId)
					.HasConstraintName("FK__Reservati__BookI__628FA481");

				entity.HasOne(d => d.User).WithMany(p => p.Reservations)
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("FK__Reservati__UserI__6383C8BA");
			});


			modelBuilder.Entity<Role>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0794AD8251");

				entity.Property(e => e.RoleName).HasMaxLength(30);
				entity.HasData(
					new Role { Id = 1, RoleName = "Admin" },
					new Role { Id = 2, RoleName = "Staff" },
					new Role { Id = 3, RoleName = "User" }
				);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07A6E2EFF5");

				entity.Property(e => e.Firstname).HasMaxLength(50);
				entity.Property(e => e.Lastname).HasMaxLength(50);
				entity.Property(e => e.Password).HasMaxLength(20);
				entity.Property(e => e.Username).HasMaxLength(50);

				entity.HasOne(d => d.Role).WithMany(p => p.Users)
					.HasForeignKey(d => d.RoleId)
					.HasConstraintName("FK__Users__RoleId__5812160E");
			});
			modelBuilder.Entity<UserLogin>(entity =>
			{
				entity.HasKey(e => e.Id).HasName("[PK__UserLogi__3214EC070064523E]");
				entity.Property(e => e.UserLoginDate).HasDefaultValueSql("(getdate())");

				entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
				.OnDelete(DeleteBehavior.SetNull);

			});
			OnModelCreatingPartial(modelBuilder);
		}
		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
