using System;
using System.Windows;
using System.Windows.Media;

namespace FakeFrame
{
    public static class DependencyObjectExtensions
    {
        public static T FindFirst<T>( this DependencyObject current )
            where T : DependencyObject
            => FindFirst<T>( current, new Predicate<T>( x => true ) );

        public static T FindFirst<T>( this DependencyObject current, Predicate<T> predicate )
            where T : DependencyObject
        {
            int countChildren = VisualTreeHelper.GetChildrenCount( current );
            for (var i = 0; i < countChildren; ++i) {
                var child = VisualTreeHelper.GetChild( current, i );
                if (child is T target && predicate( target ))
                    return target;
                var found = FindFirst( child, predicate );
                if (found != null)
                    return found;
            }

            return null;
        }

        public static bool IsParentOf( this DependencyObject current, DependencyObject view )
        {
            int countChildren = VisualTreeHelper.GetChildrenCount( current );
            for (var i = 0; i < countChildren; ++i) {
                var child = VisualTreeHelper.GetChild( current, i );
                if (child == view)
                    return true;
                if (IsParentOf( child, view ))
                    return true;
            }

            return false;
        }
    }
}
