#region

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Systecs.Framework.ServiceLocation;
using Systecs.Framework.WPF.Tools;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace Systecs.Framework.WPF.Controls
{
    public class Dialog : ContentControl
    {
        #region static

        public static readonly DependencyProperty ShowingProperty;
        public static readonly DependencyProperty CancelCommandProperty;


        static Dialog()
        {
            //Standard default style.
            DefaultStyleKeyProperty.OverrideMetadata( typeof( Dialog ),
                                                      new FrameworkPropertyMetadata( typeof( Dialog ) ) );

            ShowingProperty = DependencyProperty.Register( "Showing", typeof( bool ), typeof( Dialog ),
                                                           new FrameworkPropertyMetadata( false,
                                                                                          ShowingPropertyChangedCallback ) );

            CancelCommandProperty = DependencyProperty.Register( "CancelCommand", typeof( ICommand ), typeof( Dialog ),
                                                                 new FrameworkPropertyMetadata( null ) );
        }

        #endregion


        public Dialog()
        {
            DialogWindowExtensionPath = "Framework.WPF.DialogWindow";
            DialogViewModelExtensionPath = "Framework.WPF.DialogViewModel";
        }

        private bool _hasDispatched;
        private Window _window;


        public bool Showing
        {
            get { return (bool)GetValue( ShowingProperty ); }
            set { SetValue( ShowingProperty, value ); }
        }

        /// <summary>
        ///   This command, if present, is invoked when the user cancels the dialog.
        /// </summary>
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue( CancelCommandProperty ); }
            set { SetValue( CancelCommandProperty, value ); }
        }


        /// <summary>
        /// Gets or sets the ExtensionPath where the DialogWindow is searched in. The registered type must be Window. The default path is "Framework.WPF.DialogWindow".
        /// </summary>
        public string DialogWindowExtensionPath { get; set; }

        /// <summary>
        /// Gets or sets the ExtensionPath where the DialogViewModel is searched in. The registered type must be IDialogViewModel. The default path is "Framework.WPF.DialogViewModel".
        /// </summary>
        public string DialogViewModelExtensionPath { get; set; }


        private static void ShowingPropertyChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
           // The If-statement below is a workaround for a known bug in .NET 4.0. The bug is fixed in .NET 4.5.
           // For more information see the following web sites:
           // https://connect.microsoft.com/VisualStudio/feedback/details/619658/wpf-virtualized-control-disconnecteditem-reference-when-datacontext-switch#
           // http://social.msdn.microsoft.com/Forums/vstudio/en-US/e6643abc-4457-44aa-a3ee-dd389c88bd86/wpf-4-datacontextchanged-receive-a-disconnecteditem
           //if ((d != null) && !d.ToString().Equals("Systecs.Framework.WPF.Controls.Dialog: {DisconnectedItem}"))
           {
              //We're in the middle of handling shifting Dependency Property
              //values. We don't know what value Showing is going to end up
              //with, so we don't want to do anything yet. So we just leave
              //a message for ourselves in the next message pump go around to
              //check the showing value.

              var dialog = (Dialog)d;
              //Only leave the message once.
              dialog.DialogStateChanged((bool)e.OldValue);
           }
        }


        protected override void OnVisualParentChanged( DependencyObject oldParent )
        {
            base.OnVisualParentChanged( oldParent );
            LogicalTreeWalker.FindParentOfType<Window>( this );
        }


        protected override void OnContentChanged( object oldContent, object newContent )
        {
            base.OnContentChanged( oldContent, newContent );

            Showing = newContent != null;
        }


        private void DialogStateChanged( bool oldShowing )
        {
            if( !_hasDispatched )
            {
                Application.Current?.Dispatcher?.BeginInvoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)(() => SynchronizeDialogShowing( oldShowing )) );
                _hasDispatched = true;
            }
        }


        private void SynchronizeDialogShowing( bool oldShowing )
        {
            var newShowing = Showing;
            //If the state has changed, hide the Window.
            if( oldShowing && newShowing == false )
                CloseAndDestroyDialog();
            else if( oldShowing == false && newShowing )
            {
                Application.Current?.Dispatcher?.BeginInvoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)(CreateAndShowDialog) );
            }
            _hasDispatched = false;
        }


        private void CreateAndShowDialog()
        {
            _window = GetDialogWindow();

            var dialogViewModel = GetDialogViewModel();


            var contentViewModel = (IViewModel)Content;
            dialogViewModel.ContentViewModel = contentViewModel;

            _window.DataContext = dialogViewModel;

            if( _window != null )
            {
                _window.Closing += WindowOnClosing;
                _window.Closed += WindowOnClosed;
                _window.ShowDialog();
            }
        }

        private void WindowOnClosing( object sender, CancelEventArgs cancelEventArgs )
        {
            //The Window was cancelled probably by the user pressing the X close. If the CancelCommand does not allow execution, cancel the closing so that the dialog remains opened.
            if ( CancelCommand != null && !CancelCommand.CanExecute( null ) )
                cancelEventArgs.Cancel = true;
        }

        protected virtual Window GetDialogWindow()
        {
            return ServiceLocator.Current.GetInstance<Window>(DialogWindowExtensionPath);
        }

        protected virtual IDialogViewModel GetDialogViewModel()
        {
            return ServiceLocator.Current.GetInstance<IDialogViewModel>(DialogViewModelExtensionPath);
        }


        private Binding BindControlToWindow( string propertyName, DependencyProperty dependencyProperty )
        {
            var binding = new Binding( propertyName ) { Source = this, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding( _window, dependencyProperty, binding );
            return binding;
        }


        private void WindowOnClosed( object sender, EventArgs e )
        {
            ClearValue(ShowingProperty);

            //The Window was cancelled probably by the user pressing the X close. Execute the CancelCommand if one is provided to inform the ViewModel that this happened.
            if( CancelCommand != null && CancelCommand.CanExecute( null ) )
            {
                CancelCommand.Execute( null );
            }
            else
            {
                var contentViewModel = Content as IViewModel;
                if( contentViewModel?.Owner != null )
                    contentViewModel.Owner.SubViewModel = null;
                //var viewModel = DataContext as IViewModel;
                //if( viewModel != null )
                //    viewModel.SubViewModel = null;

            }
        }


        private void CloseAndDestroyDialog()
        {
            if( _window != null )
            {
                _window.Closed -= WindowOnClosed;
                _window.Close();
                _window = null;
            }
        }
    }
}
