using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QUANLYBANHANG.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

// Đảm bảo namespace này khớp với vị trí file của bạn
namespace QUANLYBANHANGRAZORPAGE.Pages.SanPham
{
    // ⚠️ LƯU Ý: Lớp DTO nên được đặt trong thư mục Models/DTOs và được tham chiếu bằng 'using'
    // Nhưng để tiện, ta định nghĩa nó ngay dưới đây.

    // --- Định nghĩa DTO/ViewModel để hứng dữ liệu từ API ---
    public class TbSanphamDTO
    {
        public int Masanpham { get; set; }

        public string? Tensanpham { get; set; }

        public decimal? Dongia { get; set; }

        public decimal? Soluong { get; set; }

        public string? Hinhanh { get; set; }

        public string? Mota { get; set; }

        public int? Madanhmuc { get; set; }

        public virtual TbDanhmuc? MadanhmucNavigation { get; set; }

    }


    // --- Lớp PageModel Chính ---
    public class IndexModel : PageModel
    {
        // Inject IHttpClientFactory đã được cấu hình trong Program.cs
        private readonly IHttpClientFactory _httpClientFactory;

        // Thuộc tính để View (Index.cshtml) hiển thị
        public IList<TbSanphamDTO> Sanphams { get; set; } = new List<TbSanphamDTO>();

        // Constructor để Inject dịch vụ
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Logic được chạy khi trang được yêu cầu (GET)
        public async Task OnGetAsync()
        {
            try
            {
                // Lấy Client đã được cấu hình BaseAddress
                var client = _httpClientFactory.CreateClient("BackendApi");

                // GỌI API: Tên endpoint là "products/client" (Base URL: https://localhost:7056/api/)
                var response = await client.GetAsync("Sanpham");

                if (response.IsSuccessStatusCode)
                {
                    // ĐỌC JSON và gán vào list DTO
                    Sanphams = await response.Content.ReadFromJsonAsync<IList<TbSanphamDTO>>()
                               ?? new List<TbSanphamDTO>();
                }
                else
                {
                    // Xử lý lỗi nếu API trả về trạng thái lỗi (4xx hoặc 5xx)
                    ModelState.AddModelError(string.Empty, $"Lỗi từ API: {response.ReasonPhrase}. Vui lòng kiểm tra API Backend.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi kết nối (ví dụ: API chưa chạy, lỗi SSL)
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối: Không thể kết nối với API Backend. {ex.Message}");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Lỗi không xác định khi tải dữ liệu.");
            }
        }
    }
}