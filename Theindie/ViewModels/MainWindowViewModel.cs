using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel; // Cần thiết cho danh sách
using Theindie.Models;

namespace Theindie.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        // ObservableCollection giúp UI tự động cập nhật khi dữ liệu thay đổi
        [ObservableProperty]
        private ObservableCollection<GameInfo> _games;

        public MainWindowViewModel()
        {
            // Khởi tạo dữ liệu giả lập (Được chuyển từ MainWindow.axaml.cs sang)
            // LƯU Ý: Đây là nơi Logic cư trú, tách biệt hoàn toàn với Giao diện
            Games = new ObservableCollection<GameInfo>
            {
                new GameInfo
                {
                    Title = "Stardew Valley",
                    Tags = "Nông trại, RPG",
                    Version = "v1.5.6",
                    Size = "500 MB",
                    UpdateDate = "Mới nhất",
                    Description = "Bạn được thừa hưởng trang trại cũ của ông nội...",
                    ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg"
                },
                new GameInfo
                {
                    Title = "Hollow Knight",
                    Tags = "Metroidvania",
                    Version = "v1.4.3",
                    Size = "1.2 GB",
                    UpdateDate = "2024",
                    Description = "Khám phá vương quốc côn trùng...",
                    ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg"
                },
                new GameInfo
                {
                    Title = "Hades",
                    Tags = "Roguelike",
                    Version = "v1.0",
                    Size = "2.5 GB",
                    UpdateDate = "2024",
                    Description = "Vào vai Hoàng tử Địa ngục...",
                    ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg"
                },
                new GameInfo
                {
                    Title = "Sea of Stars",
                    Tags = "JRPG",
                    Version = "v1.0.4",
                    Size = "3.2 GB",
                    UpdateDate = "2024",
                    Description = "Hành trình của những đứa con Mặt Trời...",
                    ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg"
                },
                new GameInfo
                {
                    Title = "Elden Ring",
                    Tags = "Souls-like",
                    Version = "v1.10",
                    Size = "60 GB",
                    UpdateDate = "2024",
                    Description = "Trở thành Elden Lord...",
                    ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg"
                },
            };
        }
    }
}