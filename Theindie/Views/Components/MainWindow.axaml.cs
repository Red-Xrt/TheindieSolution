using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Collections.Generic;
using Theindie.Models;

namespace Theindie.Views
{
    public partial class MainWindow : Window
    {
        // Danh sách game sẽ hiển thị
        public List<GameInfo> Games { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // 1. TẠO DỮ LIỆU (Nếu không có đoạn này thì danh sách trống)
            Games = new List<GameInfo>
            {
                new GameInfo { Title = "Stardew Valley", Tags = "Nông trại", ImagePath = "/Assets/Images/placeholder.jpg", Version="v1.5", UpdateDate="Mới nhất" },
                new GameInfo { Title = "Hollow Knight", Tags = "Hành động", ImagePath = "/Assets/Images/placeholder.jpg", Version="v1.4", UpdateDate="2024" },
                new GameInfo { Title = "Hades", Tags = "Roguelike", ImagePath = "/Assets/Images/placeholder.jpg", Version="v1.0", UpdateDate="2024" },
                // Thêm game khác vào đây...
            };

            // 2. KÍCH HOẠT DỮ LIỆU (Thiếu dòng này là không hiện gì cả)
            DataContext = this;
        }

        // ... (Giữ nguyên các hàm OnGameCardClick, Minimize, Close cũ của ông)
        private void OnGameCardClick(object? sender, RoutedEventArgs e)
        {
            var card = sender as Control;
            var selectedGame = card?.DataContext as GameInfo;
            if (selectedGame != null && DetailOverlay != null)
            {
                DetailOverlay.DataContext = selectedGame;
                DetailOverlay.IsVisible = true;
            }
        }
        private void ShowInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu game từ trang Detail chuyển sang cho Wizard
            if (DetailOverlay.DataContext is GameInfo gameInfo && InstallWizard != null)
            {
                InstallWizard.DataContext = gameInfo;
                InstallWizard.IsVisible = true;
            }
        }
        private void OnRootPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            this.Focus();
        }
        // Ẩn Wizard Cài Đặt
        private void HideInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null) InstallWizard.IsVisible = false;
        }
        private void HideDetail_Overlay(object? sender, RoutedEventArgs e) { if (DetailOverlay != null) DetailOverlay.IsVisible = false; }
        private void Minimize_Click(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Maximize_Click(object? sender, RoutedEventArgs e) => WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        private void Close_Click(object? sender, RoutedEventArgs e) => Close();
    }
}