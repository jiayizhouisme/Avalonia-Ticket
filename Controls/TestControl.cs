using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Controls
{
    public class TestControl : ContentControl
    {
        public static readonly RoutedEvent<RoutedEventArgs> TapEvent =
            RoutedEvent.Register<TestControl, RoutedEventArgs>(nameof(Tap), RoutingStrategies.Bubble);

        // Provide CLR accessors for the event
        public event EventHandler<RoutedEventArgs> Tap
        {
            add => AddHandler(TapEvent, value);
            remove => RemoveHandler(TapEvent, value);
        }

        public static readonly StyledProperty<int> TypeProperty =
            AvaloniaProperty.Register<Control, int>(nameof(Type));
        public int Type
        {
            get { return GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Defines the <see cref="IsChecked"/> property.
        /// </summary>
        public static readonly DirectProperty<TestControl, string> MyContentProperty =
            AvaloniaProperty.RegisterDirect<TestControl, string>(
                nameof(MyContent),
                o => o.MyContent,
                (o, v) => o.MyContent = v,
                defaultBindingMode: BindingMode.TwoWay);

        private string myContent = "";
        public string MyContent
        {
            get {
                return myContent;
            }
            set {
                SetAndRaise(MyContentProperty,ref myContent, value);
                RaiseEvent(new RoutedEventArgs(TapEvent));
            }
        }


    }
}
