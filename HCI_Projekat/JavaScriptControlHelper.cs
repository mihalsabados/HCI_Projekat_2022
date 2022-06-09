using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;

namespace HCI_Projekat
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        Page prozor;
        public JavaScriptControlHelper(Page w)
        {
            prozor = w;
        }
    }
}