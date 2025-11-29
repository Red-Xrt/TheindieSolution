using CommunityToolkit.Mvvm.ComponentModel;

namespace Theindie.Models
{
    // Kế thừa ObservableObject để giao diện tự cập nhật khi biến thay đổi
    public partial class GameInfo : ObservableObject
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Tags { get; set; } = "";
        public string Version { get; set; } = "";
        public string Size { get; set; } = "";
        public string UpdateDate { get; set; } = "";
        public string Rating { get; set; } = "5.0";
        public string ImagePath { get; set; } = "";

        // TRẠNG THÁI QUAN TRỌNG: Đã cài đặt hay chưa?
        [ObservableProperty]
        private bool _isInstalled = false;
        public bool IsVietHoa { get; set; } = true;
    }
}