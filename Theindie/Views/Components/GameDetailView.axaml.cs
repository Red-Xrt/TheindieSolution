using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree; // Cần thêm thư viện này để tìm cha (FindAncestor)
using System;

namespace Theindie.Views.Components
{
    public partial class GameDetailView : UserControl
    {
        public event EventHandler<RoutedEventArgs>? BackRequested;
        public event EventHandler<RoutedEventArgs>? InstallRequested;
        public event EventHandler<RoutedEventArgs>? UninstallRequested;

        public GameDetailView()
        {
            InitializeComponent();
        }

        // --- 👇 FIX LỖI: LOGIC KÉO CỬA SỔ AN TOÀN ---
        private void OnRootPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // 1. Kiểm tra xem người dùng có đang bấm vào một cái Nút nào đó không?
            var source = e.Source as Visual;
            if (source.FindAncestorOfType<Button>() != null)
            {
                // Nếu đang bấm nút -> Không kéo cửa sổ -> Để yên cho nút hoạt động
                return;
            }

            // 2. Nếu bấm vào vùng trống -> Cho phép kéo
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var topLevel = TopLevel.GetTopLevel(this) as Window;
                topLevel?.BeginMoveDrag(e);
            }
        }
        // ---------------------------------------------

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true; // Chặn sự kiện lan ra ngoài
            BackRequested?.Invoke(this, e);
        }

        private void InstallButton_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            InstallRequested?.Invoke(this, e);
        }

        private void UninstallButton_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true; // Quan trọng: Chặn sự kiện để không kích hoạt kéo cửa sổ
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = true;
            }
        }

        private void CancelUninstall_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = false;
            }
        }

        private void ConfirmUninstall_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (UninstallConfirmOverlay != null)
            {
                UninstallConfirmOverlay.IsVisible = false;
            }
            UninstallRequested?.Invoke(this, e);
        }
    }
}