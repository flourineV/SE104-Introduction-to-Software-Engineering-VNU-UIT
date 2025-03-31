﻿using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BLL;

public static class HocSinhBLL
{
    static List<HocSinh> hocSinh = HocSinhDAL.LayTatCaHocSinh();
    public static bool XoaHocSinh(string MaHS)
    {
        for (int i = 0; i < hocSinh.Count; i++)
        {
            if (hocSinh[i].MaHS==MaHS)
            {
                hocSinh.Remove(hocSinh[i]);
                
                return true;
            }
        }
        HocSinhDAL.XoaHocSinh(MaHS);
        return false;
    }

    public static string LayMaHocSinhTuDong()
    {
        string maHS = "HS";
        int soLuongHSHienTai = HocSinhDAL.LaySoLuongHocSinh();
        return maHS + string.Format("{0:D4}", LaySoThuTuTuDong(hocSinh));
    }
    private static int LaySoThuTuTuDong(List<HocSinh> hocSinh)
    {
        Algorithm.sort(hocSinh, 0, hocSinh.Count - 1);
        for (int i = 0; i < hocSinh.Count; i++)
        {
            if (i + 1 != int.Parse(hocSinh[i].MaHS.Remove(0, 2)))
            {
                return i+1;
            }
        }
        return hocSinh.Count + 1;
    }
    public static bool TiepNhanHocSinh(HocSinh hs)
    {
        string ErrorMessage = "";
        if (string.IsNullOrEmpty(hs.HoTen))
        {
            ErrorMessage += "Họ tên không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.GioiTinh))
        {
            ErrorMessage += "Giới tính không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.Email))
        {
            ErrorMessage += "Email không được để trống\n";
        }
        if (!KiemTraTuoiHopLeVoiHocSinh(hs))
        {
            ErrorMessage += "Ngày sinh không hợp lệ\n";
        }
        if (string.IsNullOrEmpty(hs.DiaChi))
        {
            ErrorMessage += "Địa chỉ không được để trống\n";
        }
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            throw new Exception(ErrorMessage);
        }

        if (HocSinhDAL.TiepNhanHocSinh(hs) == 1)
        {
            hocSinh.Add(hs);
            return true;
        }
        return false;
    }
    public static List<HocSinh> TimKiemHocSinh(string DuLieu,List<HocSinh>TatCaHocSinh)
    {
        List<HocSinh> CacKetQuaKhaThi = new List<HocSinh>();
        try
        {
           
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].MaHS)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                    return CacKetQuaKhaThi;
                }
            }
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].Email)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                }
                return CacKetQuaKhaThi;
            }
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].HoTen)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                }
                return CacKetQuaKhaThi;
            }
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].GioiTinh)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                }
                return CacKetQuaKhaThi;
            }
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].MaLop)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                }
                return CacKetQuaKhaThi;
            }
            for (int i = 0; i < TatCaHocSinh.Count; i++)
            {
                if (DuLieu == TatCaHocSinh[i].DiaChi)
                {
                    CacKetQuaKhaThi.Add(TatCaHocSinh[i]);
                }
                return CacKetQuaKhaThi;
            }
            throw new Exception("Khong Tim Thay hoc Sinh");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "LOI TIM KIEM", MessageBoxButton.OK, MessageBoxImage.Error);
            
        }
        return CacKetQuaKhaThi;
    }
    private static bool KiemTraTuoiHopLeVoiHocSinh(HocSinh hs)
    {
        int tuoiToiThieu = ThamSoDAL.LayTuoiToiThieu();
        int tuoiToiDa = ThamSoDAL.LayTuoiToiDa();

        DateTime ngayToiDa = DateTime.Now.AddYears(-tuoiToiThieu);
        DateTime ngayToiThieu = DateTime.Now.AddYears(-tuoiToiDa);
        return hs.NgaySinh >= ngayToiThieu && hs.NgaySinh <= ngayToiDa;
    }
    public static bool SuaHocSinh(HocSinh hs)
    {
        string ErrorMessage = "";
        if (string.IsNullOrEmpty(hs.HoTen))
        {
            ErrorMessage += "Họ tên không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.GioiTinh))
        {
            ErrorMessage += "Giới tính không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.Email))
        {
            ErrorMessage += "Email không được để trống\n";
        }
        if (!KiemTraTuoiHopLeVoiHocSinh(hs))
        {
            ErrorMessage += "Ngày sinh không hợp lệ\n";
        }
        if (string.IsNullOrEmpty(hs.DiaChi))
        {
            ErrorMessage += "Địa chỉ không được để trống\n";
        }
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            throw new Exception(ErrorMessage);
        }

        if (HocSinhDAL.CapNhatHocSinh(hs) == 1)
        {
            for (int i = 0;i<hocSinh.Count;i++)
            {
                if(hocSinh[i].MaHS == hs.MaHS)
                {
                    hocSinh[i] = hs;
                    break;
                }
            }
            return true;
        }
        return false;
    }
}
public static class Algorithm
{
    private static int partition(List<HocSinh> hocSinh,int low,int high)
    {
        int pivot = int.Parse(hocSinh[high].MaHS.Remove(0,2));
        int i = low;
        for (int j = low; j < high; j++)
        {
            if (int.Parse(hocSinh[j].MaHS.Remove(0, 2))<pivot)
            {
                HocSinh Temp= hocSinh[j];
                hocSinh[j]=hocSinh[i];
                hocSinh[i]=Temp;
                i++;
            }
        }
        HocSinh Temp2 = hocSinh[high];
        hocSinh[high]=hocSinh[i];
        hocSinh[i] = Temp2;
        return i;
    }
    public static void sort(List<HocSinh> hocSinh,int low=0,int high=0)
    {
        if(low<high)
        {
            int i=partition(hocSinh,low,high);
            sort(hocSinh,low,i-1);
            sort(hocSinh,i+1,high);
        }
    }
}
