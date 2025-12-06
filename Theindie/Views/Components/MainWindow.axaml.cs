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

            if (Screens.Primary != null)
            {
                // 1. Lấy thông số màn hình
                var workArea = Screens.Primary.WorkingArea; // Kích thước vật lý (Pixel thật)
                var scaling = this.DesktopScaling; // Tỷ lệ phóng to của Windows (1.0, 1.25, 1.5...)

                // Chia cho scaling để ra đơn vị hiển thị (Logical)
                var targetLogicalWidth = (workArea.Width / scaling) * 0.85;
                var targetLogicalHeight = (workArea.Height / scaling) * 0.90;

                this.Width = targetLogicalWidth;
                this.Height = targetLogicalHeight;

                // 3. Tính toán vị trí CĂN GIỮA (Center)
                // Phải đổi ngược kích thước App ra Pixel thật để tính tọa độ
                var appPhysicalWidth = targetLogicalWidth * scaling;
                var appPhysicalHeight = targetLogicalHeight * scaling;

                var newX = workArea.X + (workArea.Width - appPhysicalWidth) / 2;
                var newY = workArea.Y + (workArea.Height - appPhysicalHeight) / 2;

                // 4. Áp dụng vị trí
                this.Position = new PixelPoint((int)newX, (int)newY);
            }
        }
        //HOVER SIDEBAR
        private void OnSidebarPointerEntered(object? sender, PointerEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.IsPaneOpen = true; // Mở rộng
            }
        }

        private void OnSidebarPointerExited(object? sender, PointerEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            // KIỂM TRA CHUẨN: Nếu chuột vẫn đang nằm trên Border (hoặc con của nó)
            // Thì RETURN NGAY (Không đóng).
            // IsPointerOver tự động xử lý chính xác việc di chuyển giữa các nút con.
            if (border.IsPointerOver)
            {
                return;
            }

            // Nếu thực sự đã ra ngoài -> Đóng
            if (DataContext is MainWindowViewModel vm)
            {
                vm.IsPaneOpen = false;
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

        // 3. QUY TRÌNH CÀI ĐẶT & GỠ CÀI ĐẶT (Updated)
        // Mở Wizard Install
        private void ShowInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null && DetailOverlay.DataContext is GameInfo info)
            {
                InstallWizard.IsVisible = true;

                // Initialize in Install Mode
                InstallWizard.InitInstallMode(info);

                // Unsubscribe old events to prevent memory leaks/duplicate calls
                InstallWizard.GameInstalled -= OnGameInstalled;
                InstallWizard.GameUninstalled -= OnGameUninstalled;

                // Subscribe new
                InstallWizard.GameInstalled += OnGameInstalled;
            }
        }

        // KHI BẤM NÚT ĐỒNG Ý GỠ Ở POPUP CONFIRM (Update logic này)
        private void OnUninstallRequested(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null && DetailOverlay.DataContext is GameInfo info)
            {
                InstallWizard.IsVisible = true;

                // Initialize in Uninstall Mode
                InstallWizard.InitUninstallMode(info);

                InstallWizard.GameInstalled -= OnGameInstalled;
                InstallWizard.GameUninstalled -= OnGameUninstalled;

                InstallWizard.GameUninstalled += OnGameUninstalled;
            }
        }

        // Logic xử lý sau khi Wizard chạy xong (Update status ViewModel)
        private void OnGameInstalled(object? sender, GameInfo game)
        {
            game.IsInstalled = true;
            if (DataContext is MainWindowViewModel vm) vm.RefreshList();
        }

        private void OnGameUninstalled(object? sender, GameInfo game)
        {
            game.IsInstalled = false;
            if (DataContext is MainWindowViewModel vm) vm.RefreshList();
        }

        // Đóng Wizard (Giữ nguyên)
        private void HideInstall_Wizard(object? sender, RoutedEventArgs e)
        {
            if (InstallWizard != null) InstallWizard.IsVisible = false;
        }
    }
}