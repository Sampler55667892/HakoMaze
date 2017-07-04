using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakeUIParts
{
    public partial class CommandHolder : UserControl
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached( "Command",
                typeof(ICommand),
                typeof(CommandHolder),
                new PropertyMetadata( null ) );

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached( "CommandProperty",
                typeof(object),
                typeof(CommandHolder),
                new PropertyMetadata( null ) );

        public ICommand Command
        {
            get { return GetValue( CommandProperty ) as ICommand; }
            set { SetValue( CommandProperty, value ); }
        }

        public object CommandParameter
        {
            get { return GetValue( CommandParameterProperty ); }
            set { SetValue( CommandParameterProperty, value ); }
        }

        public CommandHolder()
        {
            InitializeComponent();
            Loaded += CommandHolder_Loaded;
        }

        void CommandHolder_Loaded( object sender, RoutedEventArgs e )
        {
            Loaded -= CommandHolder_Loaded;
            SetupHandler();
        }

        void SetupHandler()
        {
            if (Content is FrameworkElement view)
                view.PreviewMouseLeftButtonDown += View_PreviewMouseLeftButtonDown;
        }

        public void Dispose()
        {
            if (Content is FrameworkElement view)
                view.PreviewMouseLeftButtonDown -= View_PreviewMouseLeftButtonDown;
        }

        void View_PreviewMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            Command?.Execute( CommandParameter );
            e.Handled = true;
        }
    }
}
