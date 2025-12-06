using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using Theindie.Models;

namespace Theindie.Views.Components
{
    // ENUM: Define the operation mode of the wizard
    public enum WizardMode
    {
        Install,
        Uninstall
    }

    public partial class InstallationWizardView : UserControl
    {
        public event EventHandler<RoutedEventArgs>? CloseRequested;
        public event EventHandler<GameInfo>? GameInstalled;
        public event EventHandler<GameInfo>? GameUninstalled; // New event for uninstallation

        private int _currentStep = 1;
        private WizardMode _currentMode = WizardMode.Install; // Default mode

        // Simulation Timer
        private DispatcherTimer? _simulationTimer;
        private int _progressValue = 0;

        public InstallationWizardView()
        {
            InitializeComponent();
            UpdateUI();
        }

        /// <summary>
        /// Initialize the wizard in Installation mode.
        /// </summary>
        public void InitInstallMode(GameInfo game)
        {
            DataContext = game;
            _currentMode = WizardMode.Install;
            ResetWizard();
        }

        /// <summary>
        /// Initialize the wizard in Uninstallation mode.
        /// Starts directly at the processing step (Step 3).
        /// </summary>
        public void InitUninstallMode(GameInfo game)
        {
            DataContext = game;
            _currentMode = WizardMode.Uninstall;
            _currentMode = WizardMode.Uninstall;
            if (ProgressBarFill != null)
            {
                ProgressBarFill.Width = 0;
            }
            _currentStep = 3;
            UpdateUI();
        }

        // --- 1. FOLDER SELECTION LOGIC ---
        private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select Game Folder",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                TxtFolderPath.Text = folders[0].Path.LocalPath;
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            }
        }

        // --- 2. WIZARD NAVIGATION LOGIC ---
        private void NextButton_Click(object? sender, RoutedEventArgs e)
        {
            // Validate Step 2 (Only for Install mode)
            if (_currentStep == 2 && _currentMode == WizardMode.Install)
            {
                if (string.IsNullOrWhiteSpace(TxtFolderPath.Text))
                {
                    TxtFolderPath.BorderBrush = Brushes.Red;
                    return;
                }
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            }

            _currentStep++;
            UpdateUI();
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep == 3) return; // Prevent closing while running

            // Finished Step (Step 4)
            if (_currentStep == 4)
            {
                if (DataContext is GameInfo game)
                {
                    if (_currentMode == WizardMode.Install)
                    {
                        GameInstalled?.Invoke(this, game);
                    }
                    else
                    {
                        GameUninstalled?.Invoke(this, game);
                    }
                }
            }

            CloseRequested?.Invoke(this, e);
            ResetWizard();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep > 1) { _currentStep--; UpdateUI(); }
        }

        // --- 3. UI UPDATE & SIMULATION ---
        private void UpdateUI()
        {
            Step1_Confirm.IsVisible = false;
            Step2_Folder.IsVisible = false;
            Step3_Installing.IsVisible = false;
            Step4_Success.IsVisible = false;

            Dot1.Classes.Remove("Active");
            Dot2.Classes.Remove("Active");
            Dot3.Classes.Remove("Active");

            // Dynamic Text based on Mode
            string processText = _currentMode == WizardMode.Install ? "ĐANG CÀI ĐẶT..." : "ĐANG GỠ CÀI ĐẶT...";
            string successText = _currentMode == WizardMode.Install ? "HOÀN TẤT!" : "ĐÃ GỠ XONG!";
            // Green for install, Red/Orange for uninstall (optional), keeping Green for success is fine mostly.

            switch (_currentStep)
            {
                case 1: // Confirm (Install Only)
                    Step1_Confirm.IsVisible = true;
                    Dot1.Classes.Add("Active");
                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = true;
                    BtnNext.Content = "TIẾP TỤC";
                    BtnClose.IsVisible = true;
                    break;

                case 2: // Folder Select (Install Only)
                    Step2_Folder.IsVisible = true;
                    Dot2.Classes.Add("Active");
                    BtnBack.IsVisible = true;
                    BtnNext.IsVisible = true;
                    BtnNext.Content = "CÀI ĐẶT";
                    BtnClose.IsVisible = true;
                    break;

                case 3: // Processing (Both)
                    Step3_Installing.IsVisible = true;
                    // For Uninstall, we treat it as the final progress step
                    if (_currentMode == WizardMode.Install) Dot3.Classes.Add("Active");
                    else { /* Update dots logic if needed for uninstall, or hide dots */ }

                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false;
                    BtnClose.IsVisible = false;

                    // Update Title dynamically
                    TxtProcessTitle.Text = processText;

                    StartSimulation();
                    break;

                case 4: // Success (Both)
                    Step4_Success.IsVisible = true;
                    Dot1.Classes.Add("Active"); Dot2.Classes.Add("Active"); Dot3.Classes.Add("Active");

                    BtnBack.IsVisible = false;
                    BtnNext.IsVisible = false;

                    // Only show "Play Now" if installed
                    BtnPlay.IsVisible = (_currentMode == WizardMode.Install);

                    BtnClose.IsVisible = true;

                    // Update Success Text
                    TxtSuccessTitle.Text = successText;
                    break;
            }
        }

        private void StartSimulation()
        {
            _progressValue = 0;
            ProgressBarFill.Width = 0;
            TxtPercent.Text = "0%";
            TxtStatus.Text = _currentMode == WizardMode.Install ? "Đang kiểm tra thư mục..." : "Đang tìm file rác...";

            _simulationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) }; // Faster tick for uninstall maybe?
            _simulationTimer.Tick += (s, e) =>
            {
                _progressValue += 2;

                double maxWidth = 400;
                ProgressBarFill.Width = (_progressValue / 100.0) * maxWidth;
                TxtPercent.Text = $"{_progressValue}%";

                // Update status text based on Mode
                UpdateStatusText();

                if (_progressValue >= 100)
                {
                    _simulationTimer.Stop();
                    _currentStep = 4;
                    UpdateUI();
                }
            };
            _simulationTimer.Start();
        }

        private void UpdateStatusText()
        {
            if (_currentMode == WizardMode.Install)
            {
                if (_progressValue < 30) TxtStatus.Text = "Đang giải nén dữ liệu...";
                else if (_progressValue < 70) TxtStatus.Text = "Copying files...";
                else if (_progressValue < 90) TxtStatus.Text = "Đang xác thực file...";
                else TxtStatus.Text = "Hoàn tất cài đặt...";
            }
            else // Uninstall Mode
            {
                if (_progressValue < 30) TxtStatus.Text = "Đang xóa dữ liệu...";
                else if (_progressValue < 70) TxtStatus.Text = "Đang dọn dẹp Registry...";
                else if (_progressValue < 90) TxtStatus.Text = "Khôi phục file gốc...";
                else TxtStatus.Text = "Hoàn tất gỡ cài đặt...";
            }
        }

        private void ResetWizard()
        {
            _simulationTimer?.Stop();
            _currentStep = 1;
            if (TxtFolderPath != null)
            {
                TxtFolderPath.Text = "";
                TxtFolderPath.BorderBrush = new SolidColorBrush(Color.Parse("#333333"));
            }
            if (BtnPlay != null) BtnPlay.IsVisible = false;
            if (ProgressBarFill != null) ProgressBarFill.Width = 0;
            if (TxtPercent != null) TxtPercent.Text = "0%";
            if (TxtStatus != null) TxtStatus.Text = "Đang chờ...";
            UpdateUI();
        }
    }
}