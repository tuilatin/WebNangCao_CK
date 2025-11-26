using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QUANLYBANHANG.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHang { get; set; }

    public virtual DbSet<DanhMuc> DanhMuc { get; set; }

    public virtual DbSet<DonHang> DonHang { get; set; }

    public virtual DbSet<GioHang> GioHang { get; set; }

    public virtual DbSet<KhachHang> KhachHang { get; set; }

    public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToan { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<SanPham> SanPham { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietD__CDF0A114DE6F2B67");

            entity.HasIndex(e => e.MaDonHang, "IX_ChiTietDonHang_DonHang");

            entity.HasIndex(e => e.MaSanPham, "IX_ChiTietDonHang_SanPham");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenSanPham).HasMaxLength(200);
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", true)
                .HasColumnType("decimal(29, 2)");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHang)
                .HasForeignKey(d => d.MaDonHang)
                .HasConstraintName("FK_ChiTietDonHang_DonHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHang)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietDonHang_SanPham");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaGioHang, e.MaSanPham });

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", true)
                .HasColumnType("decimal(29, 2)");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ChiTietGioHang)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK_ChiTietGioHang_GioHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietGioHang)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietGioHang_SanPham");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMuc__B3750887E82CF4B0");

            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(200);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584AD4B795A45");

            entity.HasIndex(e => e.MaKhachHang, "IX_DonHang_KhachHang");

            entity.HasIndex(e => e.NgayDatHang, "IX_DonHang_NgayDatHang").IsDescending();

            entity.HasIndex(e => e.TrangThai, "IX_DonHang_TrangThai");

            entity.Property(e => e.NgayDatHang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NguoiDuyet).HasMaxLength(50);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("CHO_XU_LY");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DonHang)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_KhachHang");

            entity.HasOne(d => d.MaPhuongThucNavigation).WithMany(p => p.DonHang)
                .HasForeignKey(d => d.MaPhuongThuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_PhuongThuc");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__F5001DA381626261");

            entity.HasIndex(e => e.MaKhachHang, "UX_GioHang_KhachHang_DangMua")
                .IsUnique()
                .HasFilter("([TrangThai]=N'DANG_MUA')");

            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("DANG_MUA");

            entity.HasOne(d => d.MaKhachHangNavigation).WithOne(p => p.GioHang)
                .HasForeignKey<GioHang>(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_KhachHang");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E52FCDBD07");

            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PhuongThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaPhuongThuc).HasName("PK__PhuongTh__35F7404E63296913");

            entity.Property(e => e.PhiGiaoDich)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenPhuongThuc).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AF02123B8");

            entity.HasIndex(e => e.RoleName, "UQ__Role__8A2B61609B84DEFA").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442D7D2463C0");

            entity.HasIndex(e => e.MaDanhMuc, "IX_SanPham_DanhMuc");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(200);
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoLuong).HasDefaultValue(0);
            entity.Property(e => e.TenSanPham).HasMaxLength(200);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPham)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK_SanPham_DanhMuc");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529EC435F5A");

            entity.HasIndex(e => e.Username, "UQ__TaiKhoan__536C85E415558219").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.TaiKhoan)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK_TaiKhoan_KhachHang");

            entity.HasOne(d => d.Role).WithMany(p => p.TaiKhoan)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__TaiKhoan__RoleId__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
