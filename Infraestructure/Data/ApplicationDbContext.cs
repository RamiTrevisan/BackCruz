using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de las entidades
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("T_Usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdAdmin");
                entity.Property(e => e.Username).HasColumnName("Usuario").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasColumnName("Contrasena").HasMaxLength(50).IsRequired();
                entity.Property(e => e.RoleId).HasColumnName("IdRol").IsRequired();
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("T_Clientes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdCliente");
                entity.Property(e => e.FullName).HasColumnName("NombreApellido").HasMaxLength(50);
                entity.Property(e => e.Address).HasColumnName("Direccion").HasMaxLength(80);
                entity.Property(e => e.Phone).HasColumnName("Telefono");
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.ToTable("T_Comidas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdComida");
                entity.Property(e => e.Name).HasColumnName("Comida").HasMaxLength(100).IsRequired();
                entity.Property(e => e.CategoryId).HasColumnName("CodComida").IsRequired();
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.FoodItems)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("T_Menues");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdMenu");
                entity.Property(e => e.Number).HasColumnName("Numero").IsRequired();
                entity.Property(e => e.Price).HasColumnName("Precio");
                entity.Property(e => e.IsActive).HasColumnName("Activo").IsRequired();
                entity.Property(e => e.Stock).HasColumnName("Stock");
            });

            modelBuilder.Entity<MenuFoodItem>(entity =>
            {
                entity.ToTable("T_DetalleMenu");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdDetalleMenu");
                entity.Property(e => e.MenuId).HasColumnName("IdMenu").IsRequired();
                entity.Property(e => e.FoodItemId).HasColumnName("IdComida").IsRequired();
                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.MenuId);
                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.FoodItemId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("T_Pedidos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdPedido");
                entity.Property(e => e.OrderDate).HasColumnName("Fecha");
                entity.Property(e => e.CustomerId).HasColumnName("IdCliente").IsRequired();
                entity.Property(e => e.Total).HasColumnName("Total");
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("T_DetallePedidos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdDetallePedido");
                entity.Property(e => e.OrderId).HasColumnName("IdPedido").IsRequired();
                entity.Property(e => e.MenuId).HasColumnName("IdMenu").IsRequired();
                entity.Property(e => e.Quantity).HasColumnName("Cantidad").IsRequired();
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId);
                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MenuId);
            });
        }
    }
}