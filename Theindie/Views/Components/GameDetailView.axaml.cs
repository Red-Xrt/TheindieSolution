using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Theindie.Views.Components
{
    public partial class GameDetailView : UserControl
    {
        // Các sự kiện giao tiếp với bên ngoài
        public event EventHandler<RoutedEventArgs>? BackRequested;
        public event EventHandler<RoutedEventArgs>? InstallRequested;
        public event EventHandler<RoutedEventArgs>? UninstallRequested;

        public GameDetailView()
        {
            InitializeComponent();
        }

        // Logic xử lý nút Back và Install
        private void BackButton_Click(object? sender, RoutedEventArgs e) => BackRequested?.Invoke(this, e);
        private void InstallButton_Click(object? sender, RoutedEventArgs e) => InstallRequested?.Invoke(this, e);

        // --- LOGIC HỘP THOẠI GỠ CÀI ĐẶT ---

        // 1. Hiện hộp thoại
        private void UninstallButton_Click(object? sender, RoutedEventArgs e)
        {
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = true;
            }
        }

        // 2. Ẩn hộp thoại (Hủy)
        private void CancelUninstall_Click(object? sender, RoutedEventArgs e)
        {
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = false;
            }
        }

        // 3. Xác nhận gỡ (Đồng ý)
        private void ConfirmUninstall_Click(object? sender, RoutedEventArgs e)
        {
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = false;
            }
            // Gửi tín hiệu ra MainWindow để xử lý logic gỡ
            UninstallRequested?.Invoke(this, e);
        }
    }
}