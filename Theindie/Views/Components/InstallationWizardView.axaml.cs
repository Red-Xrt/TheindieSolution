using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media; // Cần dòng này
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;

namespace Theindie.Views.Components
{
    public partial class InstallationWizardView : UserControl
    {
        public event EventHandler<RoutedEventArgs>? CloseRequested;
        private int _currentStep = 1;

        public InstallationWizardView()
        {
            InitializeComponent();
            UpdateUI();
        }

        // --- XỬ LÝ CHỌN FOLDER ---
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

                // Xóa viền đỏ lỗi bằng màu xám mờ
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#30FFFFFF"));
            }
        }

        // --- XỬ LÝ NÚT TIẾP TỤC / CÀI ĐẶT ---
        private void NextButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep == 2)
            {
                if (string.IsNullOrWhiteSpace(TxtFolderPath.Text))
                {
                    // Báo lỗi viền đỏ
                    TxtFolderPath.BorderBrush = Brushes.Red;
                    return;
                }
                else
                {
                    // Reset viền
                    TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#30FFFFFF"));
                }
            }

            _currentStep++;
            UpdateUI();
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep == 3) return;
            CloseRequested?.Invoke(this, e);
            ResetWizard();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep > 1) { _currentStep--; UpdateUI(); }
        }

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
                case 1:
                    Step1_Confirm.IsVisible = true;
                    Dot1.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = true;
                    TxtNext.Text = "TIẾP TỤC";
                    BtnClose.IsVisible = true;
                    break;
                case 2:
                    Step2_Folder.IsVisible = true;
                    Dot2.Classes.Add("Active");
                    BtnBack.IsVisible = true;
                    BtnNext.IsVisible = true;
                    TxtNext.Text = "CÀI ĐẶT";
                    BtnClose.IsVisible = true;
                    break;
                case 3:
                    Step3_Installing.IsVisible = true;
                    Dot3.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false;
                    BtnClose.IsVisible = false;
                    DispatcherTimer.RunOnce(() => { _currentStep = 4; UpdateUI(); }, TimeSpan.FromSeconds(3));
                    break;
                case 4:
                    Step4_Success.IsVisible = true;
                    Dot1.Classes.Add("Active"); Dot2.Classes.Add("Active"); Dot3.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false;
                    BtnPlay.IsVisible = true;
                    BtnClose.IsVisible = true;
                    break;
            }
        }

        private void ResetWizard()
        {
            _currentStep = 1;
            TxtFolderPath.Text = "";
            TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#30FFFFFF"));
            BtnPlay.IsVisible = false;
            UpdateUI();
        }
    }
}