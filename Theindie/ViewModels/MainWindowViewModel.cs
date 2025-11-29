using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Theindie.Models;

namespace Theindie.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly List<GameInfo> _allGames;

        [ObservableProperty]
        private ObservableCollection<GameInfo> _games;

        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private int _currentPage = 0;

        [ObservableProperty]
        private string _pageTitle = "TRANG CHỦ";

        // 👇 TRẠNG THÁI MỚI: Sidebar có đang mở rộng không?
        [ObservableProperty]
        private bool _isPaneOpen = false; // Mặc định đóng (chỉ hiện icon)

        public MainWindowViewModel()
        {
            // Database giả lập
            _allGames = new List<GameInfo>
            {
                new GameInfo { Title = "Stardew Valley", IsInstalled = false, Tags = "Nông trại, RPG", Version = "v1.5.6", Size = "500 MB", UpdateDate = "Mới nhất", Description = "Bạn được thừa hưởng trang trại cũ của ông nội...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Hollow Knight", IsInstalled = false, Tags = "Metroidvania", Version = "v1.4.3", Size = "1.2 GB", UpdateDate = "2024", Description = "Khám phá vương quốc côn trùng...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Hades", IsInstalled = false, Tags = "Roguelike", Version = "v1.0", Size = "2.5 GB", UpdateDate = "2024", Description = "Vào vai Hoàng tử Địa ngục...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Sea of Stars", IsInstalled = false, Tags = "JRPG", Version = "v1.0.4", Size = "3.2 GB", UpdateDate = "2024", Description = "Hành trình của những đứa con Mặt Trời...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Elden Ring", IsInstalled = false, Tags = "Souls-like", Version = "v1.10", Size = "60 GB", UpdateDate = "2024", Description = "Trở thành Elden Lord...", ImagePath = "/Assets/Icons/Logo.ico" },
            };

            Games = new ObservableCollection<GameInfo>(_allGames);
        }

        partial void OnSearchTextChanged(string value) => ApplyFilter();

        partial void OnCurrentPageChanged(int value)
        {
            switch (value)
            {
                case 0: PageTitle = "TRANG CHỦ"; break;
                case 1: PageTitle = "GAME ĐÃ CÀI ĐẶT"; break;
                case 2: PageTitle = "LỊCH SỬ TẢI XUỐNG"; break;
                case 3: PageTitle = "CẤU HÌNH HỆ THỐNG"; break;
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = _searchText.ToLower();
            var filtered = _allGames.Where(g =>
                g.Title.ToLower().Contains(query) &&
                (CurrentPage != 1 || g.IsInstalled)
            );
            Games = new ObservableCollection<GameInfo>(filtered);
        }

        public void RefreshList() => ApplyFilter();

        [RelayCommand]
        public void SwitchPage(string pageIndex)
        {
            if (int.TryParse(pageIndex, out int index)) CurrentPage = index;
        }

        // Lệnh Toggle thủ công (nếu cần nút Hamburger)
        [RelayCommand]
        public void TogglePane() => IsPaneOpen = !IsPaneOpen;
    }
}