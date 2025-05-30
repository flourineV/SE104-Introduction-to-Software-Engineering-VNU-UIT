﻿using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BangDiemMonBLL
    {
        public required string MaHocSinh { get; set; }
        public required string MaMH { get; set; }
        public required string MaHK { get; set; }
        private float? Diem15P;
        private float? Diem1T;
        private float? DiemCuoiKy;


        public BangDiemMonBLL TaoBangDiem(string MaHocSinh, string MaMH, string MaHk, string Diem15P = "", string Diem1T = "", string DiemCuoiKy = "")
        {
            if (MaHocSinh == null) { return null; }
            if (MaHK == null) { return null; }
            if (MaMH == null) { return null; }
            float? Diem15PChuyenDoi = float.TryParse(Diem15P, out float output) ? output : null;
            float? Diem1TChuyenDoi = float.TryParse(Diem1T, out float output1) ? output1 : null;
            float? DiemCuoiKyChuyenDoi = float.TryParse(DiemCuoiKy, out float output2) ? output2 : null;
            KiemTraCacDieuKien(Diem15PChuyenDoi, Diem1TChuyenDoi, DiemCuoiKyChuyenDoi);
            BangDiemMonBLL result = new BangDiemMonBLL { MaHocSinh = MaHocSinh, MaMH = MaMH, MaHK = MaHk };
            if (Diem1T != null)
            {
                result.Diem1T = Diem1TChuyenDoi;
            }
            if (Diem15P != null)
            {
                result.Diem15P = Diem15PChuyenDoi;
            }
            if (DiemCuoiKy != null)
            {
                result.DiemCuoiKy = DiemCuoiKyChuyenDoi;
            }
            return result;
        }
        private void KiemTraCacDieuKien(float? Diem15P, float? Diem1T, float? DiemCuoiKy)
        {
            var validHocKys = DataContext.Context.HOCKY
                         .Select(h => h.MaHK)
                         .ToList();
            if (!validHocKys.Contains(MaHK))
                throw new Exception($"Học kỳ {MaHK} không tồn tại");
            if (Diem15P < 0 || Diem15P > 10 || Diem1T < 0 || Diem1T > 10 || DiemCuoiKy > 10 || DiemCuoiKy < 0)
            {
                throw new Exception("Mot Trong Cac Diem Thanh Phan Khong Hop Le");
            }
        }

        public void XoaBangDiem(string MaHS, string MaMonHoc, string MaHK)
        {
            BangDiemMonDAL.XoaDiem(MaHS, MaMonHoc, MaHK);
        }

        public static void CapNhatBangDiem(BangDiemMonBLL bangDiemMoi)
        {
            var diem = new BangDiemMon
            {
                MaHocSinh = bangDiemMoi.MaHocSinh,
                MaMH = bangDiemMoi.MaMH,
                MaHK = bangDiemMoi.MaHK,
                Diem15P = bangDiemMoi.Diem15P,
                Diem1T = bangDiemMoi.Diem1T,
                DiemCuoiKy = bangDiemMoi.DiemCuoiKy
            };

            BangDiemMonDAL.LuuDiem(diem);
        }

        public static BangDiemMonBLL LayBangDiem(string maHocSinh, string maMonHoc, string maHocKy)
        {
            var bangDiemDTO = BangDiemMonDAL.LayBangDiem(maHocSinh, maMonHoc, maHocKy);

            if (bangDiemDTO == null) { return null; }

            BangDiemMonBLL bangDiemBLL = new BangDiemMonBLL
            {
                MaHocSinh = maHocSinh,
                MaMH = maMonHoc,
                MaHK = maHocKy
            };

            bangDiemBLL.Diem15P = bangDiemDTO.Diem15P;
            bangDiemBLL.Diem1T = bangDiemDTO.Diem1T;
            bangDiemBLL.DiemCuoiKy = bangDiemDTO.DiemCuoiKy;

            return bangDiemBLL;
        }

        public void TruyXuatBangDiem()
        {
            var bangDiemData = LayBangDiem(MaHocSinh, MaMH, MaHK);
            if (bangDiemData != null)
            {
                Diem15P = bangDiemData.Diem15P;
                Diem1T = bangDiemData.Diem1T;
                DiemCuoiKy = bangDiemData.DiemCuoiKy;
            }
            else
            {
                Diem15P = null;
                Diem1T = null;
                DiemCuoiKy = null;
            }
        }
    }
   
}
