using System.Collections.Generic;

namespace Theindie.Models
{
    public class GameInfo
    {
        // Các thông tin cơ bản của một game
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Tags { get; set; } = ""; // Ví dụ: "Nhập vai, Nông trại"
        public string Version { get; set; } = "";
        public string Size { get; set; } = "";
        public string UpdateDate { get; set; } = "";
        public string Rating { get; set; } = "5.0";

        // Đường dẫn ảnh (Tạm thời dùng ảnh trong Assets)
        // Sau này có thể là URL online
        public string ImagePath { get; set; } = "";
    }
}