using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ChromeLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MyCustomApplicationContext());
        }

        public class MyCustomApplicationContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            private ContextMenuStrip cmu;

            private int Width = 230;

            private Color BackgroundColor = ColorTranslator.FromHtml("#2b2b2b");
            private Color ForegroundColor = Color.White;
            private Color SelectedColor = ColorTranslator.FromHtml("#414141");

            private string UIFont = "Segoe UI";

            private int titleFontSize = 15;
            private int labelFontSize = 12;

            private int labelHeight = 36;

            public MyCustomApplicationContext()
            {

                if (cmu == null)
                {
                    cmu = new ContextMenuStrip
                    {
                        ShowCheckMargin = false,
                        ShowImageMargin = false,

                        BackColor = BackgroundColor,
                        ForeColor = ForegroundColor,

                        Padding = new Padding(0),

                        Font = new Font(UIFont, labelFontSize)
                    };


                    ToolStripLabel TitleLabel = new ToolStripLabel("Select a Chrome Profile:")
                    {
                        Font = new Font(UIFont, titleFontSize),
                        AutoSize = true,
                        Padding = new Padding(0)
                    };

                    cmu.Items.Add(TitleLabel);

                    cmu.Items.Add(new ToolStripSeparator());

                    cmu.AutoSize = true;
                    cmu.MaximumSize = new Size(Width, 0);

                    cmu.DropShadowEnabled = false;

                    cmu.RenderMode = ToolStripRenderMode.System;

                    cmu.Items.Add(CreateMenuItem("Launch Social", (object sender, EventArgs e) => LaunchChrome("1")));
                    cmu.Items.Add(CreateMenuItem("Launch Personal", (object sender, EventArgs e) => LaunchChrome("2")));
                    cmu.Items.Add(CreateMenuItem("Launch Wooster", (object sender, EventArgs e) => LaunchChrome("3")));
                    cmu.Items.Add(CreateMenuItem("Launch Test", (object sender, EventArgs e) => LaunchChrome("4")));
                    cmu.Items.Add(CreateMenuItem("Launch Fairfield", (object sender, EventArgs e) => LaunchChrome("5")));
                    cmu.Items.Add(CreateMenuItem("Launch Self-Help", (object sender, EventArgs e) => LaunchChrome("8")));
                    cmu.Items.Add(CreateMenuItem("Quit ChromeLauncher", Exit));
                }

                if (trayIcon == null)
                {
                    // Initialize Tray Icon
                    trayIcon = new NotifyIcon()
                    {
                        Text = "Launch a Chrome Profile",
                        Icon = Properties.Resources.chrome,
                        ContextMenuStrip = cmu,
                        Visible = true
                    };
                }

                trayIcon.Click += (object sender, EventArgs e) =>
                {
                    cmu.Show(Control.MousePosition);
                };


            }

            ToolStripLabel CreateMenuItem(String name, EventHandler to)
            {
                name = name.PadLeft(name.Length + 5);
                ToolStripLabel a = new ToolStripLabel(name, null, false, to)
                {
                    AutoSize = false,
                    Height = labelHeight,
                    Width = Width,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 0, 0, 0)
                };

                a.MouseEnter += (object s, EventArgs e) =>
                {
                    a.BackColor = SelectedColor;
                };

                a.MouseLeave += (object s, EventArgs e) =>
                {
                    a.BackColor = BackgroundColor;
                };

                return a;
            }

            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;

                Application.Exit();
            }


            void LaunchChrome(String user)
            {

                ProcessStartInfo info = new ProcessStartInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")
                {
                    WindowStyle = ProcessWindowStyle.Maximized,
                    Arguments = "--new-window --window-position=0,0 --start-maximized --profile-directory=\"Profile " + user + "\""
                };

                Process.Start(info);

            }
        }
    }
}
