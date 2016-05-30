using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace LockScreenTray
{
    public class LockTrayApp : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new LockTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public LockTrayApp()
        {
            // Create a simple tray menu with only one item
            trayMenu = new ContextMenu();
            //Alternate way to lock screen aside from doubleclick the icon - menu and then pick option
            trayMenu.MenuItems.Add("Lock Screen", this.trayIcon_DoubleClick);
            //Close the app
            trayMenu.MenuItems.Add("Exit", OnExit);

            // New Notification tray icon object
            trayIcon = new NotifyIcon();
            // Tooltip
            trayIcon.Text = "Double Click to Lock the Screen";
            // Get the white padlock ico as the tray icon and associate it with the NotifyIcon obj
            trayIcon.Icon = new Icon(LockScreenTray.Properties.Resources.LockTrayIcon, 40, 40);

            // Handle the DoubleClick event to lock the screen
            trayIcon.DoubleClick += new EventHandler(this.trayIcon_DoubleClick);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }


        protected override void OnLoad(EventArgs e)
        {
            // Hide form window so we just have the icon in the system tray
            Visible = false;
            // annnd remove from taskbar for the same reason
            ShowInTaskbar = false; 
            base.OnLoad(e);
        }


        //Access the LockWorkStation function without having to do a system call
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();

        //DoubleClick event handler to lock the screen - also handle click on the 'Lock Screen' menu item
        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            LockWorkStation();
        }


        //Exit the app cleanly
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Don't leave the icon hanging around in the menu after exit
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}