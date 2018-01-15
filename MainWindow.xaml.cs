using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinOSShutDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShutDownBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PopupMessageBox("This Device will be Shut down.", "Power Off", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ShutDown();
            }
        }

        private void ShutDown()
        {
            ManagementBaseObject osShutdown = null;
            ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            osClass.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject osBaseObject = osClass.GetMethodParameters("Win32Shutdown");
            // 0 (0x0) : log off
            // 4 (0x4) : Forced Log Off (0 + 4)
            // 1 (0x1) : Shutdown
            // 5 (0x5) : Forced Shutdown (1 + 4)
            // 2 (0x2) : Reboot 
            // 6 (0x6) : Forced Reboot (2 + 4)
            // 8 (0x8) : Power Off
            // 12 (0xC) : Forced Power Off (8 + 4)
            osBaseObject["Flags"] = "5";
            osBaseObject["Reserved"] = "0";
            foreach(ManagementObject osObj in osClass.GetInstances())
            {
                osShutdown = osObj.InvokeMethod("Win32Shutdown", osBaseObject, null);
            }

        }

        private MessageBoxResult PopupMessageBox(string msg, string caption, MessageBoxButton button)
        {
            return MessageBox.Show(msg, caption, button);
        }
    }
}
