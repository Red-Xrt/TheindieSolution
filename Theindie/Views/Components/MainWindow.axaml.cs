using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using Theindie.Models;
using Theindie.ViewModels;

namespace Theindie.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // --- 1. XỬ LÝ CỬA SỔ (Header, Kéo thả, Focus) ---
        private void OnHeaderPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                this.BeginMoveDrag(e);
            }
        }

        private void OnRootPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            topLevel?.FocusManager?.ClearFocus();
        }

        private void Minimize_Click(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Close_Click(object? sender, RoutedEventArgs e) => Close();

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            // Lấy thông tin màn hình hiện tại
            if (Screens.Primary != null)
            {
                var workArea = Screens.Primary.WorkingArea; // Vùng làm việc (trừ thanh Taskbar)
                var screenWidth = workArea.Width;
                var screenHeight = workArea.Height;

                // 1. Tính toán kích thước mong muốn (70% Rộng, 75% Cao)
                var newWidth = screenWidth * 0.70;
                var newHeight = screenHeight * 0.75;

                this.Width = newWidth;
                this.Height = newHeight;

                // 2. TÍNH TOÁN VỊ TRÍ CĂN GIỮA (CENTER)
                // Công thức: Tọa độ = Góc màn hình + (Kích thước màn - Kích thước app) / 2
                var newX = (int)(workArea.X + (screenWidth - newWidth) / 2);
                var newY = (int)(workArea.Y + (screenHeight - newHeight) / 2);

                // 3. Áp dụng vị trí mới
                this.Position = new PixelPoint(newX, newY);
            }
        }
        // --- 2. XỬ LÝ OVERLAY CHI TIẾT GAME ---
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

        private void HideDetail_Overlay(object? sender, RoutedEventArgs e)
        {
            if (DetailOverlay != null) DetailOverlay.IsVisible = false;
        }


        // --- 3. QUY TRÌNH CÀI ĐẶT (WIZARD & LIFECYCLE) ---

        // Mở Wizard và Đăng ký sự kiện
        private void ShowInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null && DetailOverlay.DataContext is GameInfo info)
            {
                InstallWizard.DataContext = info;
                InstallWizard.IsVisible = true;

                // Quan trọng: Lắng nghe sự kiện cài xong (Xóa cũ trước để tránh trùng lặp)
                InstallWizard.GameInstalled -= OnGameInstalled;
                InstallWizard.GameInstalled += OnGameInstalled;
            }
        }

        // Đóng Wizard
        private void HideInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null) InstallWizard.IsVisible = false;
        }

        // KHI CÀI ĐẶT THÀNH CÔNG -> Cập nhật trạng thái
        private void OnGameInstalled(object? sender, GameInfo game)
        {
            // Set trạng thái -> ĐÃ CÀI
            game.IsInstalled = true;

            // Yêu cầu ViewModel cập nhật danh sách
            if (DataContext is MainWindowViewModel vm) vm.RefreshList();
        }

        // KHI BẤM NÚT GỠ VIỆT HÓA -> Cập nhật trạng thái
        private void OnUninstallRequested(object? sender, RoutedEventArgs e)
        {
            if (DetailOverlay.DataContext is GameInfo game)
            {
                // Set trạng thái -> CHƯA CÀI
                game.IsInstalled = false;

                // Cập nhật danh sách
                if (DataContext is MainWindowViewModel vm) vm.RefreshList();
            }
        }
    }
}