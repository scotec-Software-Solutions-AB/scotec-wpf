#region

using System.Globalization;
using System.Windows.Data;

#endregion


namespace Scotec.Wpf
{
    public class CultureBinding : Binding
    {
        public CultureBinding()
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }

        public CultureBinding( string path )
            : base( path )
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }
    }
}