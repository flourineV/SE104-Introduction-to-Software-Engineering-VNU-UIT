﻿using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL;

public static class BangDiemMonDAL
{
    // Thêm hoặc cập nhật điểm
    public static void LuuDiem(BangDiemMon diem)
    {
        var context = DataContext.Context;
        var diemCu = context.BANGDIEMMON
            .FirstOrDefault(b => b.MaHocSinh == diem.MaHocSinh && b.MaMH == diem.MaMH && b.MaHK == diem.MaHK);

        if (diemCu != null)
        {
            diemCu.Diem15P = diem.Diem15P;
            diemCu.Diem1T = diem.Diem1T;
            diemCu.DiemCuoiKy = diem.DiemCuoiKy;
        }
        else
        {
            context.BANGDIEMMON.Add(diem);
        }

        context.SaveChanges();
    }

    // Lấy danh sách điểm theo học sinh
    public static List<BangDiemMon> LayDiemTheoHocSinh(string maHocSinh)
    {
        return DataContext.Context.BANGDIEMMON
            .Where(b => b.MaHocSinh == maHocSinh)
            .Include(b => b.MonHoc)
            .Include(b => b.HocKy)
            .ToList();
    }

    // Xóa điểm
    public static bool XoaDiem(string maHS, string maMH, string maHK)
    {
        var context = DataContext.Context;
        var diem = context.BANGDIEMMON
            .FirstOrDefault(b => b.MaHocSinh == maHS && b.MaMH == maMH && b.MaHK == maHK);

        if (diem != null)
        {
            context.BANGDIEMMON.Remove(diem);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public static BangDiemMon LayBangDiem(string maHS, string maMH, string maHK)
    {
        var context = DataContext.Context;
        var bangDiem = context.BANGDIEMMON
            .Include(b => b.MonHoc)
            .Include(b => b.HocKy)
            .Include(b => b.HocSinh)
            .FirstOrDefault(b => b.MaHocSinh == maHS && b.MaMH == maMH && b.MaHK == maHK);
        return bangDiem;    
    }

    public static List<BangDiemMon> TruyXuatBangDiem()
    {
        var context = DataContext.Context;
        return context.BANGDIEMMON
            .Include(b => b.MonHoc)
            .Include(b => b.HocKy)
            .Include(b => b.HocSinh)
            .ToList();
    }
}