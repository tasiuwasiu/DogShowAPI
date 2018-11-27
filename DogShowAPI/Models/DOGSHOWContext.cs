using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DogShowAPI.Models
{
    public partial class DogShowContext : DbContext
    {

        public DogShowContext(DbContextOptions<DogShowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AllowedBreedsContest> AllowedBreedsContest { get; set; }
        public virtual DbSet<AppSetting> AppSetting { get; set; }
        public virtual DbSet<BreedGroup> BreedGroup { get; set; }
        public virtual DbSet<BreedSection> BreedSection { get; set; }
        public virtual DbSet<Contest> Contest { get; set; }
        public virtual DbSet<ContestType> ContestType { get; set; }
        public virtual DbSet<Dog> Dog { get; set; }
        public virtual DbSet<DogBreed> DogBreed { get; set; }
        public virtual DbSet<DogClass> DogClass { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<Participation> Participation { get; set; }
        public virtual DbSet<Place> Place { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UsersPermission> UsersPermission { get; set; }
        public virtual DbSet<UsersSecurity> UsersSecurity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllowedBreedsContest>(entity =>
            {
                entity.HasKey(e => new { e.ContestTypeId, e.BreedTypeId });

                entity.ToTable("allowed_breeds_contest");

                entity.HasIndex(e => e.BreedTypeId)
                    .HasName("allowedbreedscontest_dogbreed_BREED_ID_fk");

                entity.Property(e => e.ContestTypeId)
                    .HasColumnName("CONTEST_TYPE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BreedTypeId)
                    .HasColumnName("BREED_TYPE_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.BreedType)
                    .WithMany(p => p.AllowedBreedsContest)
                    .HasForeignKey(d => d.BreedTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("allowedbreedscontest_dogbreed_BREED_ID_fk");

                entity.HasOne(d => d.ContestType)
                    .WithMany(p => p.AllowedBreedsContest)
                    .HasForeignKey(d => d.ContestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("allowedbreedscontest_contesttype_CONTEST_TYPE_ID_fk");
            });

            modelBuilder.Entity<AppSetting>(entity =>
            {
                entity.HasKey(e => e.SettingId);

                entity.ToTable("app_setting");

                entity.HasIndex(e => e.SettingId)
                    .HasName("app_settings_SETTING_ID_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.SettingName)
                    .HasName("app_settings_SETTING_NAME_uindex")
                    .IsUnique();

                entity.Property(e => e.SettingId)
                    .HasColumnName("SETTING_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SettingValue)
                    .HasColumnName("SETTING_VALUE")
                    .HasColumnType("json");

                entity.Property(e => e.SettingData)
                    .HasColumnName("SETTING_DATA")
                    .HasColumnType("blob");

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasColumnName("SETTING_NAME")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<BreedGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("breed_group");

                entity.HasIndex(e => e.GroupId)
                    .HasName("breed_groups_GROUP_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NamePolish)
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<BreedSection>(entity =>
            {
                entity.HasKey(e => e.SectionId);

                entity.ToTable("breed_section");

                entity.HasIndex(e => e.GroupNumber)
                    .HasName("breed_sections_breed_groups_GROUP_ID_fk");

                entity.HasIndex(e => e.SectionId)
                    .HasName("BREED_SECTIONS_SECTION_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.SectionId)
                    .HasColumnName("SECTION_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GroupNumber)
                    .HasColumnName("GROUP_NUMBER")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NamePolish)
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SectionNumber)
                    .HasColumnName("SECTION_NUMBER")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.GroupNumberNavigation)
                    .WithMany(p => p.BreedSection)
                    .HasForeignKey(d => d.GroupNumber)
                    .HasConstraintName("breed_sections_breed_groups_GROUP_ID_fk");
            });

            modelBuilder.Entity<Contest>(entity =>
            {
                entity.ToTable("contest");

                entity.HasIndex(e => e.ContestId)
                    .HasName("contests_CONTEST_ID_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.ContestTypeId)
                    .HasName("contests_contest_types_CONTEST_TYPE_ID_fk");

                entity.HasIndex(e => e.PlaceId)
                    .HasName("contests_places_PLACE_ID_fk");

                entity.Property(e => e.ContestId)
                    .HasColumnName("CONTEST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContestTypeId)
                    .HasColumnName("CONTEST_TYPE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.PlaceId)
                    .HasColumnName("PLACE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ContestType)
                    .WithMany(p => p.Contest)
                    .HasForeignKey(d => d.ContestTypeId)
                    .HasConstraintName("contests_contest_types_CONTEST_TYPE_ID_fk");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Contest)
                    .HasForeignKey(d => d.PlaceId)
                    .HasConstraintName("contests_places_PLACE_ID_fk");
            });

            modelBuilder.Entity<ContestType>(entity =>
            {
                entity.ToTable("contest_type");

                entity.HasIndex(e => e.ContestTypeId)
                    .HasName("contest_types_CONTEST_TYPE_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.ContestTypeId)
                    .HasColumnName("CONTEST_TYPE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Enterable)
                    .HasColumnName("ENTERABLE")
                    .HasColumnType("bit")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NamePolish)
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Dog>(entity =>
            {
                entity.ToTable("dog");

                entity.HasIndex(e => e.BreedId)
                    .HasName("dog_dogbreed_BREED_ID_fk");

                entity.HasIndex(e => e.ChipNumber)
                    .HasName("dogs_CHIP_NUMBER_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.ClassId)
                    .HasName("dog_dogclass_CLASS_ID_fk");

                entity.HasIndex(e => e.DogId)
                    .HasName("dogs_DOG_ID_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.OwnerId)
                    .HasName("dogs_users_USER_ID_fk");

                entity.Property(e => e.DogId)
                    .HasColumnName("DOG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Birthday)
                    .HasColumnName("BIRTHDAY")
                    .HasColumnType("date");

                entity.Property(e => e.BreedId)
                    .HasColumnName("BREED_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BreederAddress)
                    .HasColumnName("BREEDER_ADDRESS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.BreederName)
                    .HasColumnName("BREEDER_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ChipNumber)
                    .IsRequired()
                    .HasColumnName("CHIP_NUMBER")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ClassId)
                    .HasColumnName("CLASS_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FatherName)
                    .HasColumnName("FATHER_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.LineageNumber)
                    .HasColumnName("LINEAGE_NUMBER")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.MotherName)
                    .HasColumnName("MOTHER_NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RegistrationNumber)
                    .HasColumnName("REGISTRATION_NUMBER")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasColumnName("SEX")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Titles)
                    .HasColumnName("TITLES")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.Breed)
                    .WithMany(p => p.Dog)
                    .HasForeignKey(d => d.BreedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dog_dogbreed_BREED_ID_fk");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Dog)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("dog_dogclass_CLASS_ID_fk");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Dog)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dogs_users_USER_ID_fk");
            });

            modelBuilder.Entity<DogBreed>(entity =>
            {
                entity.HasKey(e => e.BreedId);

                entity.ToTable("dog_breed");

                entity.HasIndex(e => e.BreedId)
                    .HasName("DOG_BREEDS_BREED_ID_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.SectionId)
                    .HasName("dog_breeds_breed_sections_SECTION_ID_fk");

                entity.Property(e => e.BreedId)
                    .HasColumnName("BREED_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NamePolish)
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.SectionId)
                    .HasColumnName("SECTION_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.DogBreed)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("dogbreed_breedsection_SECTION_ID_fk");
            });

            modelBuilder.Entity<DogClass>(entity =>
            {
                entity.HasKey(e => e.ClassId);

                entity.ToTable("dog_class");

                entity.HasIndex(e => e.ClassId)
                    .HasName("dog_classes_CLASS_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.ClassId)
                    .HasColumnName("CLASS_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.NamePolish)
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(30)");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("grade");

                entity.HasIndex(e => e.GradeId)
                    .HasName("grades_GRADE_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.GradeId)
                    .HasColumnName("GRADE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ForPuppies)
                    .HasColumnName("FOR_PUPPIES")
                    .HasColumnType("bit");

                entity.Property(e => e.GradeLevel)
                    .HasColumnName("GRADE_LEVEL")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NameEnglish)
                    .HasColumnName("NAME_ENGLISH")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.NamePolish)
                    .IsRequired()
                    .HasColumnName("NAME_POLISH")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Participation>(entity =>
            {
                entity.ToTable("participation");

                entity.HasIndex(e => e.ContestId)
                    .HasName("participation_contesttype_CONTEST_ID_fk");

                entity.HasIndex(e => e.DogId)
                    .HasName("participation_dog_OG_ID_fk");

                entity.HasIndex(e => e.GradeId)
                    .HasName("participation_grade_GRADE_ID_fk");

                entity.HasIndex(e => e.ParticipationId)
                    .HasName("participation_PARTICIPATION_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.ParticipationId)
                    .HasColumnName("PARTICIPATION_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ContestId)
                    .HasColumnName("CONTEST_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DogId)
                    .HasColumnName("DOG_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GradeId)
                    .HasColumnName("GRADE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Place)
                    .HasColumnName("PLACE")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Contest)
                    .WithMany(p => p.Participation)
                    .HasForeignKey(d => d.ContestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("participation_contesttype_CONTEST_ID_fk");

                entity.HasOne(d => d.Dog)
                    .WithMany(p => p.Participation)
                    .HasForeignKey(d => d.DogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("participation_dog_OG_ID_fk");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Participation)
                    .HasForeignKey(d => d.GradeId)
                    .HasConstraintName("participation_grade_GRADE_ID_fk");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("place");

                entity.HasIndex(e => e.PlaceId)
                    .HasName("places_PLACE_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.PlaceId)
                    .HasColumnName("PLACE_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserId)
                    .HasName("USERS_USER_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.FirstName)
                    .HasColumnName("FIRST_NAME")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.LastName)
                    .HasColumnName("LAST_NAME")
                    .HasColumnType("varchar(30)");
            });

            modelBuilder.Entity<UsersPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("users_permission");

                entity.Property(e => e.PermissionId)
                    .HasColumnName("PERMISSION_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<UsersSecurity>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("users_security");

                entity.HasIndex(e => e.PermissionLevel)
                    .HasName("users_security_users_permissions_PERMISSION_ID_fk");

                entity.HasIndex(e => e.UserId)
                    .HasName("USERS_SECURITY_USER_ID_uindex")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PermissionLevel)
                    .HasColumnName("PERMISSION_LEVEL")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserHash)
                    .IsRequired()
                    .HasColumnName("USER_HASH")
                    .HasColumnType("blob");

                entity.Property(e => e.UserSalt)
                    .IsRequired()
                    .HasColumnName("USER_SALT")
                    .HasColumnType("blob");

                entity.HasOne(d => d.PermissionLevelNavigation)
                    .WithMany(p => p.UsersSecurity)
                    .HasForeignKey(d => d.PermissionLevel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_security_users_permission_PERMISSION_ID_fk");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UsersSecurity)
                    .HasForeignKey<UsersSecurity>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_security_users_USER_ID_fk");
            });
        }
    }
}
