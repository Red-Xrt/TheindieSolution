using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using Theindie.Models;

namespace Theindie.Views.Components
{
    public partial class InstallationWizardView : UserControl
    {
        public event EventHandler<RoutedEventArgs>? CloseRequested;
        public event EventHandler<GameInfo>? GameInstalled;

        private int _currentStep = 1;

        // Timer để chạy giả lập tiến trình
        private DispatcherTimer? _simulationTimer;
        private int _progressValue = 0;

        public InstallationWizardView()
        {
            InitializeComponent();
            UpdateUI();
        }

        // --- 1. LOGIC CHỌN FOLDER ---
        private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Chọn thư mục game gốc",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                TxtFolderPath.Text = folders[0].Path.LocalPath;
                // Reset viền lỗi nếu có
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            }
        }

        // --- 2. LOGIC ĐIỀU HƯỚNG WIZARD ---
        private void NextButton_Click(object? sender, RoutedEventArgs e)
        {
            // Validate Step 2
            if (_currentStep == 2)
            {
                if (string.IsNullOrWhiteSpace(TxtFolderPath.Text))
                {
                    TxtFolderPath.BorderBrush = Brushes.Red; // Báo lỗi chưa chọn folder
                    return;
                }
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            }

            _currentStep++;
            UpdateUI();
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep == 3) return; // Đang chạy thì không cho đóng

            // Nếu đã xong (Step 4), báo ra ngoài là GameInstalled
            if (_currentStep == 4)
            {
                if (DataContext is GameInfo game)
                {
                    GameInstalled?.Invoke(this, game);
                }
            }

            // Đóng Wizard và Reset
            CloseRequested?.Invoke(this, e);
            ResetWizard();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep > 1) { _currentStep--; UpdateUI(); }
        }

        // --- 3. LOGIC CẬP NHẬT GIAO DIỆN & GIẢ LẬP ---
        private void UpdateUI()
        {
            Step1_Confirm.IsVisible = false;
            Step2_Folder.IsVisible = false;
            Step3_Installing.IsVisible = false;
            Step4_Success.IsVisible = false;

            Dot1.Classes.Remove("Active");
            Dot2.Classes.Remove("Active");
            Dot3.Classes.Remove("Active");

            switch (_currentStep)
            {
                case 1: // Xác nhận
                    Step1_Confirm.IsVisible = true;
                    Dot1.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = true;

                    // SỬA LỖI: Thay TxtNext.Text bằng BtnNext.Content
                    BtnNext.Content = "TIẾP TỤC";

                    BtnClose.IsVisible = true;
                    break;

                case 2: // Chọn thư mục
                    Step2_Folder.IsVisible = true;
                    Dot2.Classes.Add("Active");
                    BtnBack.IsVisible = true;
                    BtnNext.IsVisible = true;

                    // SỬA LỖI: Thay TxtNext.Text bằng BtnNext.Content
                    BtnNext.Content = "CÀI ĐẶT";

                    BtnClose.IsVisible = true;
                    break;

                case 3: // Đang chạy (Loading...)
                    Step3_Installing.IsVisible = true;
                    Dot3.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false; // Ẩn nút Tiếp tục đi
                    BtnClose.IsVisible = false;

                    // BẮT ĐẦU CHẠY GIẢ LẬP
                    StartSimulation();
                    break;

                case 4: // Thành công
                    Step4_Success.IsVisible = true;
                    Dot1.Classes.Add("Active"); Dot2.Classes.Add("Active"); Dot3.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false;
                    BtnPlay.IsVisible = true; // Hiện nút Chơi Ngay
                    BtnClose.IsVisible = true;
                    break;
            }
        }

        private void StartSimulation()
        {
            _progressValue = 0;
            ProgressBarFill.Width = 0;
            TxtPercent.Text = "0%";
            TxtStatus.Text = "Đang kiểm tra thư mục...";

            // Timer chạy mỗi 50ms
            _simulationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            _simulationTimer.Tick += (s, e) =>
            {
                _progressValue += 2; // Tăng 2% mỗi lần tick

                // Cập nhật Width cho thanh ProgressBar
                // Giả sử Width max là khoảng 350px (hoặc binding width cha, nhưng hardcode logic UI tạm ở đây cho mượt)
                // Vì ProgressBarFill nằm trong Border cha có Width cố định hoặc Stretch, 
                // ta nên set Width theo % hoặc dùng Relative, nhưng ở đây ta set Width trực tiếp cho Border con.
                // Lưu ý: Trong XAML mới, Border cha (container) có Height="6", Border con (ProgressBarFill) chạy bên trong.

                // Để đơn giản và khớp với XAML: 
                // XAML đang set Width="0". Ta sẽ tăng Width lên tối đa khoảng 400px (tùy grid cha).
                // Cách an toàn hơn là set Width theo % tiến trình.

                double maxWidth = 400; // Ước lượng width của container
                ProgressBarFill.Width = (_progressValue / 100.0) * maxWidth;

                TxtPercent.Text = $"{_progressValue}%";

                // Giả lập text trạng thái
                if (_progressValue < 30) TxtStatus.Text = "Đang giải nén dữ liệu...";
                else if (_progressValue < 70) TxtStatus.Text = "Copying files to destination...";
                else if (_progressValue < 90) TxtStatus.Text = "Đang xác thực file...";
                else TxtStatus.Text = "Hoàn tất cài đặt...";

                // Khi chạy xong
                if (_progressValue >= 100)
                {
                    _simulationTimer.Stop();
                    // Chuyển sang bước 4 (Thành công)
                    _currentStep = 4;
                    UpdateUI();
                }
            };
            _simulationTimer.Start();
        }

        private void ResetWizard()
        {
            _simulationTimer?.Stop();
            _currentStep = 1;
            TxtFolderPath.Text = "";
            TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            BtnPlay.IsVisible = false;
            UpdateUI();
        }
    }
}