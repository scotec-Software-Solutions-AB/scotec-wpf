#region

using System.Windows;
using System.Windows.Media;

#endregion


namespace Systecs.Framework.WPF.Tools
{
    public static class LogicalTreeWalker
    {
        /// <summary>
        ///   Walks up the logical tree starting at 'initial' and returns
        ///   the first element of the type T enountered.
        /// </summary>
        /// <param name = "initial">It is assumed that this element is in a logical tree.</param>
        public static TResult FindParentOfType<TResult>( DependencyObject initial ) where TResult : DependencyObject
        {
            var current = FindClosestLogicalAncestor( initial );

            while( current != null && !(current is TResult) )
                current = LogicalTreeHelper.GetParent( current );

            return (current as TResult);
        }


        /// <summary>
        /// Searches for an element of a given type at the specified position.
        /// </summary>
        /// <typeparam name="T">The type of the DependencyObject to search for.</typeparam>
        /// <param name="reference">The top parent that may contains the element we to search for.</param>
        /// <param name="point">The position.</param>
        /// <returns></returns>
        public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
        {
            var element = reference.InputHitTest(point) as DependencyObject;

            if (element == null)
                return null;

            var fromPoint = element as T;
            return fromPoint ?? FindParentOfType<T>(element);
        }

        /// <summary>
        ///   This method is necessary in case the element is not
        ///   part of a logical tree.  It finds the closest ancestor
        ///   element which is in a logical tree.
        /// </summary>
        /// <param name = "initial">The element on which the user clicked.</param>
        private static DependencyObject FindClosestLogicalAncestor(DependencyObject initial)
        {
            var current = initial;
            var result = initial;

            while (current != null)
            {
                var logicalParent = LogicalTreeHelper.GetParent(current);
                if (logicalParent != null)
                {
                    result = current;
                    break;
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return result;
        }


    }
}
