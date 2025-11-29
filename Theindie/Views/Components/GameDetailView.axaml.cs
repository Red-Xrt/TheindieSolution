using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Theindie.Views.Components
{
    public partial class GameDetailView : UserControl
    {
        // 1. Sự kiện quay lại (cho nút Back)
        public event EventHandler<RoutedEventArgs>? BackRequested;

        // 2. Sự kiện cài đặt (cho nút Cài Đặt Ngay) - Để mở Wizard
        public event EventHandler<RoutedEventArgs>? InstallRequested;

        public GameDetailView()
        {
            InitializeComponent();
        }

        // Hàm xử lý nút Quay lại
        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, e);
        }

        // Hàm xử lý nút Cài đặt ngay
        private void InstallButton_Click(object? sender, RoutedEventArgs e)
        {
            InstallRequested?.Invoke(this, e);
        }
    }
}