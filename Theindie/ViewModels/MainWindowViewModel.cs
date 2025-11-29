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

        public MainWindowViewModel()
        {
            // SỬA: Thay đổi ImagePath thành "/Assets/Icons/Logo.ico" để không bị lỗi crash
            // Hoặc bạn có thể dùng link online nếu muốn đẹp: "https://upload.wikimedia.org/wikipedia/en/f/fd/Stardew_Valley_cover.jpg"
            _allGames = new List<GameInfo>
            {
                new GameInfo { Title = "Stardew Valley", Tags = "Nông trại, RPG", Version = "v1.5.6", Size = "500 MB", UpdateDate = "Mới nhất", Description = "Bạn được thừa hưởng trang trại cũ của ông nội...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Hollow Knight", Tags = "Metroidvania", Version = "v1.4.3", Size = "1.2 GB", UpdateDate = "2024", Description = "Khám phá vương quốc côn trùng...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Hades", Tags = "Roguelike", Version = "v1.0", Size = "2.5 GB", UpdateDate = "2024", Description = "Vào vai Hoàng tử Địa ngục...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Sea of Stars", Tags = "JRPG", Version = "v1.0.4", Size = "3.2 GB", UpdateDate = "2024", Description = "Hành trình của những đứa con Mặt Trời...", ImagePath = "/Assets/Icons/Logo.ico" },
                new GameInfo { Title = "Elden Ring", Tags = "Souls-like", Version = "v1.10", Size = "60 GB", UpdateDate = "2024", Description = "Trở thành Elden Lord...", ImagePath = "/Assets/Icons/Logo.ico" },
            };

            Games = new ObservableCollection<GameInfo>(_allGames);
        }

        partial void OnSearchTextChanged(string value) => ApplyFilter();

        partial void OnCurrentPageChanged(int value)
        {
            switch (value)
            {
                case 0: PageTitle = "TRANG CHỦ"; break;
                case 1: PageTitle = "THƯ VIỆN CỦA TÔI"; break;
                case 2: PageTitle = "TẢI XUỐNG"; break;
                case 3: PageTitle = "CÀI ĐẶT HỆ THỐNG"; break;
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = _searchText.ToLower();
            var filtered = _allGames.Where(g =>
                g.Title.ToLower().Contains(query) &&
                (CurrentPage != 1 || g.Title.Contains("Stardew") || g.Title.Contains("Hollow"))
            );
            Games = new ObservableCollection<GameInfo>(filtered);
        }

        [RelayCommand]
        public void SwitchPage(string pageIndex)
        {
            if (int.TryParse(pageIndex, out int index)) CurrentPage = index;
        }
    }
}