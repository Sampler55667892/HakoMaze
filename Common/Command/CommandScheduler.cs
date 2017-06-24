﻿using System;
using System.Windows;
using System.Windows.Input;

namespace HakoMaze.Common
{
    public sealed class CommandScheduler
    {
        FrameworkElement view;
        FrameworkElement childView;
        Command activeCommand;

        public FrameworkElement ChildView
        {
            get { return childView; }
            set {
                if (!view.IsParentOf( value ))
                    throw new Exception( $"{value.GetType().Name} は {view.GetType().Name} の子ではありません" );
                childView = value;
            }
        }

        public bool ComputesRelativePositionFromChildView { get; set; }

        public Command ActiveCommand
        {
            get { return activeCommand; }
            set {
                activeCommand?.OnFinalize();
                activeCommand = value;
                value?.OnInitialize();
            }
        }

        public CommandScheduler( FrameworkElement view )
        {
            this.view = view;
            view.MouseMove += View_MouseMove;
            view.MouseLeftButtonDown += View_MouseLeftButtonDown;
        }

        public void Dispose()
        {
            view.MouseMove -= View_MouseMove;
            view.MouseLeftButtonDown -= View_MouseLeftButtonDown;
        }

        // メモ：e.GetPosition() は System.Xaml アセンブリで定義されている
        void View_MouseMove( object sender, MouseEventArgs e )
        {
            if (ActiveCommand == null)
                return;

            var position = ComputePosition( e );
            if (!position.HasValue)
                return;

            ActiveCommand.Position = position.Value;
            ActiveCommand.OnMove();
        }

        void View_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if (ActiveCommand == null)
                return;

            ActiveCommand.OnAct();
            ActiveCommand.StopsAct = false;
        }

        Point? ComputePosition( MouseEventArgs e )
        {
            // LeftButtonDown 時に (0, 0) になる
            var position = e.GetPosition( this.view );
            if (position.X == 0 && position.Y == 0)
                return null;

            if (ComputesRelativePositionFromChildView && ChildView != null)
                position = e.GetPosition( childView );

            return position;
        }
    }
}
