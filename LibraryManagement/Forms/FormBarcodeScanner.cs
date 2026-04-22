using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using LibraryManagement.Services;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace LibraryManagement.Forms
{
    public sealed partial class FormBarcodeScanner : Form
    {
        private readonly BarcodeService _barcodeService;
        private readonly CancellationToken _cancellationToken;
        private readonly System.Windows.Forms.Timer _timer;

        private VideoCapture? _capture;
        private Mat? _frame;
        private bool _running;

        public string? BarcodeText { get; private set; }

        public FormBarcodeScanner() : this(new BarcodeService(), CancellationToken.None)
        {
        }

        public FormBarcodeScanner(BarcodeService barcodeService, CancellationToken cancellationToken)
        {
            InitializeComponent();

            _barcodeService = barcodeService;
            _cancellationToken = cancellationToken;
            _timer = new System.Windows.Forms.Timer { Interval = 120 };

            Load += FormBarcodeScanner_Load;
            FormClosing += FormBarcodeScanner_FormClosing;
            _timer.Tick += Timer_Tick;
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            StartCamera();
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            StopCamera();
        }

        private void BtnUseManual_Click(object? sender, EventArgs e)
        {
            UseManual();
        }

        private void TxtManual_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UseManual();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void FormBarcodeScanner_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            EnumerateCameras();

            if (_cboCamera.Items.Count > 0)
                _cboCamera.SelectedIndex = 0;

            _lblStatus.Text = _cboCamera.Items.Count > 0 ? "Sẵn sàng" : "Không tìm thấy camera";

            if (_cancellationToken.CanBeCanceled)
            {
                _cancellationToken.Register(() =>
                {
                    try
                    {
                        if (!IsHandleCreated) return;
                        BeginInvoke(new Action(() =>
                        {
                            DialogResult = DialogResult.Cancel;
                            Close();
                        }));
                    }
                    catch { }
                });
            }
        }

        private void EnumerateCameras()
        {
            _cboCamera.Items.Clear();
            for (int i = 0; i < 6; i++)
            {
                using var test = new VideoCapture(i);
                if (test.IsOpened())
                    _cboCamera.Items.Add(i);
            }
        }

        private void StartCamera()
        {
            if (_running) return;
            if (_cboCamera.SelectedItem == null) return;

            int index = Convert.ToInt32(_cboCamera.SelectedItem);
            _capture = new VideoCapture(index);
            if (!_capture.IsOpened())
            {
                _lblStatus.Text = "Không mở được camera";
                _capture.Dispose();
                _capture = null;
                return;
            }

            _frame = new Mat();
            _running = true;
            _lblStatus.Text = "Đang quét...";
            _timer.Start();
        }

        private void StopCamera()
        {
            if (!_running) return;
            _timer.Stop();
            _running = false;

            if (_capture != null)
            {
                _capture.Release();
                _capture.Dispose();
                _capture = null;
            }

            if (_frame != null)
            {
                _frame.Dispose();
                _frame = null;
            }

            _lblStatus.Text = "Đã dừng";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_running || _capture == null || _frame == null) return;
            if (_cancellationToken.IsCancellationRequested) return;

            try
            {
                if (!_capture.Read(_frame) || _frame.Empty())
                    return;

                using var bitmap = _frame.ToBitmap();

                var previous = _picPreview.Image;
                _picPreview.Image = (Bitmap)bitmap.Clone();
                previous?.Dispose();

                string? decoded = _barcodeService.TryDecodeBarcode(bitmap);
                if (!string.IsNullOrWhiteSpace(decoded))
                {
                    BarcodeText = decoded;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch
            {
            }
        }

        private void UseManual()
        {
            string value = _txtManual.Text.Trim();
            if (string.IsNullOrWhiteSpace(value))
                return;

            BarcodeText = value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormBarcodeScanner_FormClosing(object? sender, FormClosingEventArgs e)
        {
            StopCamera();

            var previous = _picPreview.Image;
            _picPreview.Image = null;
            previous?.Dispose();
        }
    }
}

