using System;
using Avalonia.Controls;
using Avalonia.Input; // Cần dòng này để dùng PointerPressedEventArgs
using Avalonia.Interactivity; // Cần dòng này để dùng RoutedEventArgs

namespace Theindie.Views.Components
{
    public partial class GameCardView : UserControl
    {
        // 1. Khai báo sự kiện Click
        public event EventHandler<RoutedEventArgs>? Click;

        public GameCardView()
        {
            InitializeComponent();
        }

        // 2. Hàm xử lý khi nhấn chuột (File giao diện đang gọi hàm này)
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // Chỉ nhận chuột trái
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                // Phát tín hiệu Click ra ngoài cho MainWindow biết
                Click?.Invoke(this, new RoutedEventArgs());
            }
        }
    }
}