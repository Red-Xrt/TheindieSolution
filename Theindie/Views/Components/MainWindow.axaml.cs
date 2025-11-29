using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input; // Cần dòng này
using System.Collections.Generic;
using Theindie.Models;

namespace Theindie.Views
{
    public partial class MainWindow : Window
    {
        public List<GameInfo> Games { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Dữ liệu giả (Giữ nguyên)
            Games = new List<GameInfo>
            {
                new GameInfo { Title = "Stardew Valley", Tags = "Nông trại, RPG", Version = "v1.5.6", Size = "500 MB", UpdateDate = "Mới nhất", Description = "Bạn được thừa hưởng trang trại cũ của ông nội...", ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg" },
                new GameInfo { Title = "Hollow Knight", Tags = "Metroidvania", Version = "v1.4.3", Size = "1.2 GB", UpdateDate = "2024", Description = "Khám phá vương quốc côn trùng...", ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg" },
                new GameInfo { Title = "Hades", Tags = "Roguelike", Version = "v1.0", Size = "2.5 GB", UpdateDate = "2024", Description = "Vào vai Hoàng tử Địa ngục...", ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg" },
                new GameInfo { Title = "Sea of Stars", Tags = "JRPG", Version = "v1.0.4", Size = "3.2 GB", UpdateDate = "2024", Description = "Hành trình của những đứa con Mặt Trời...", ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg" },
                new GameInfo { Title = "Elden Ring", Tags = "Souls-like", Version = "v1.10", Size = "60 GB", UpdateDate = "2024", Description = "Trở thành Elden Lord...", ImagePath = "avares://Theindie/Assets/Images/placeholder.jpg" },
            };

            DataContext = this;
        }

        // --- 👇 HÀM MỚI: KÉO CỬA SỔ KHI BẤM VÀO HEADER ---
        private void OnHeaderPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // Chỉ kéo khi nhấn chuột trái
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                this.BeginMoveDrag(e);
            }
        }

        private void OnRootPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            this.Focus();
        }

        // --- CÁC HÀM CŨ GIỮ NGUYÊN ---
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
        private void HideDetail_Overlay(object? sender, RoutedEventArgs e) { if (DetailOverlay != null) DetailOverlay.IsVisible = false; }
        private void ShowInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null && DetailOverlay.DataContext is GameInfo info)
            {
                InstallWizard.DataContext = info;
                InstallWizard.IsVisible = true;
            }
        }
        private void HideInstall_Wizard(object? sender, RoutedEventArgs e) { if (InstallWizard != null) InstallWizard.IsVisible = false; }

        private void Minimize_Click(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Maximize_Click(object? sender, RoutedEventArgs e) => WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        private void Close_Click(object? sender, RoutedEventArgs e) => Close();
    }
}