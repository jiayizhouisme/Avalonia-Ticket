using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
namespace GetStartedApp.Views
{
    public partial class BookingPage : UserControl
    {
        private bool _isDragging = false;
        private Point _startPoint;
        private double _startOffset;
        private ScrollViewer _dateScrollViewer;

        public BookingPage()
        {
            InitializeComponent();
            _dateScrollViewer = this.FindControl<ScrollViewer>("DateScrollViewer");

        }


        private void Border_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                _isDragging = true;
                _startPoint = e.GetPosition(_dateScrollViewer);
                _startOffset = _dateScrollViewer.Offset.X;
                e.Pointer.Capture((IInputElement)sender);
            }
        }

        private void Border_PointerMoved(object sender, PointerEventArgs e)
        {
            if (_isDragging && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var currentPoint = e.GetPosition(_dateScrollViewer);
                var deltaX = _startPoint.X - currentPoint.X;
                _dateScrollViewer.Offset = new Vector(_startOffset + deltaX, 0);
            }
        }

        private void Border_PointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
        {
            _isDragging = false;
        }
    }
}