using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Theindie.Views.Components
{
    public partial class GameDetailView : UserControl
    {
        public event EventHandler<RoutedEventArgs>? BackRequested;
        public event EventHandler<RoutedEventArgs>? InstallRequested;
        // Thêm sự kiện Gỡ
        public event EventHandler<RoutedEventArgs>? UninstallRequested;

        public GameDetailView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e) => BackRequested?.Invoke(this, e);
        private void InstallButton_Click(object? sender, RoutedEventArgs e) => InstallRequested?.Invoke(this, e);

        // Xử lý click Gỡ
        private void UninstallButton_Click(object? sender, RoutedEventArgs e) => UninstallRequested?.Invoke(this, e);
    }
}