using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input; // Cần dòng này
using Theindie.Models;

namespace Theindie.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            var topLevel = TopLevel.GetTopLevel(this);
            topLevel?.FocusManager?.ClearFocus();
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