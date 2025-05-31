using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public class BaoCaoTongKetHocKyDAL
{
    public static BaoCaoHocKyResult LapBaoCaoTongKetHocKy(string maHK, int mocDiemDat)
    {
        var context = DataContext.Context;

        // Debug: In ra giá trị mocDiemDat
        System.Diagnostics.Debug.WriteLine($"DEBUG Sprint6: mocDiemDat = {mocDiemDat}");

        var semesterScores = context.BANGDIEMMON.Include(b => b.HocSinh).ThenInclude(hs => hs.Lop).Where(b => b.MaHK == maHK && b.HocSinh != null && b.HocSinh.Lop != null).ToList();

        var reportByClass = semesterScores.GroupBy(b => b.HocSinh.Lop).Select(classGroup => {
                var lop = classGroup.Key;

                // Lấy tất cả học sinh trong lớp (không chỉ học sinh có điểm)
                var allStudentsInClass = context.HOCSINH.Where(hs => hs.MaLop == lop.MaLop).ToList();
                int siSo = allStudentsInClass.Count;
                int soLuongDat = 0;

                foreach (var student in allStudentsInClass)
                {
                    // Lấy tất cả điểm của học sinh trong học kỳ này
                    var studentScoresInSemester = context.BANGDIEMMON
                        .Where(b => b.MaHocSinh == student.MaHS && b.MaHK == maHK && b.DiemCuoiKy.HasValue)
                        .ToList();

                    // Debug: In thông tin học sinh
                    System.Diagnostics.Debug.WriteLine($"DEBUG: Student {student.MaHS} - Scores count: {studentScoresInSemester.Count}");

                    // Nếu học sinh không có điểm nào trong học kỳ → không đạt
                    if (!studentScoresInSemester.Any())
                    {
                        System.Diagnostics.Debug.WriteLine($"DEBUG: Student {student.MaHS} - No scores, KHÔNG ĐẠT");
                        continue; // Không đạt
                    }

                    // Tính điểm trung bình tổng của học sinh trong học kỳ
                    double diemTrungBinh = studentScoresInSemester.Average(b => b.DiemCuoiKy.Value);
                    System.Diagnostics.Debug.WriteLine($"DEBUG: Student {student.MaHS} - Average: {diemTrungBinh}, MocDiemDat: {mocDiemDat}");

                    // So sánh điểm trung bình với mốc điểm đạt
                    if (diemTrungBinh >= mocDiemDat)
                    {
                        System.Diagnostics.Debug.WriteLine($"DEBUG: Student {student.MaHS} - ĐẠT");
                        soLuongDat++;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"DEBUG: Student {student.MaHS} - KHÔNG ĐẠT");
                    }
                }

                double tyLe = (siSo > 0) ? (double)soLuongDat * 100.0 / siSo : 0;

                // Debug: In kết quả cuối cùng
                System.Diagnostics.Debug.WriteLine($"DEBUG: Class {lop.TenLop} - SiSo: {siSo}, SoLuongDat: {soLuongDat}, TyLe: {tyLe}%");

                return new ChiTietBaoCaoHocKyLop
                {
                    TenLop = lop.TenLop,
                    SiSo = siSo,
                    SoLuongDat = soLuongDat,
                    TyLeDat = Math.Round(tyLe, 2)
                };
            }).OrderBy(r => r.TenLop).ToList();

        return new BaoCaoHocKyResult
        {
            DanhSachThongKeLop = reportByClass
        };
    }
}
