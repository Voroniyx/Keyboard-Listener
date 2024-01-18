using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

class Program
{
    [DllImport("user32.dll")]
    public static extern int GetAsyncKeyState(int i);

    private static NotifyIcon notifyIcon;
    private static Thread backgroundThread;

    [STAThread]
    static void Main()
    {
        // Initialisiere das NotifyIcon
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = SystemIcons.Application;
        notifyIcon.Visible = true;
        notifyIcon.Text = "F8 Gif Listener";

        // Erstelle ein Kontextmenü für das NotifyIcon
        ContextMenu contextMenu = new ContextMenu();
        contextMenu.MenuItems.Add("Beenden", OnExit);

        // Setze das Kontextmenü für das NotifyIcon
        notifyIcon.ContextMenu = contextMenu;

        // Starte den Hintergrundthread
        backgroundThread = new Thread(BackgroundTask);
        backgroundThread.SetApartmentState(ApartmentState.STA);
        backgroundThread.IsBackground = true;
        backgroundThread.Start();

        // Starte die Anwendungsschleife
        Application.Run();
    }

    static void BackgroundTask()
    {
        while (true)
        {
            if (IsKeyPressed(0x77)) // F8
            {
                Console.WriteLine("GIF wurde in die Zwischenablage kopiert");
                Clipboard.SetText("https://tenor.com/view/horse-stare-intense-gif-8871241302243172472");
            }

            Thread.Sleep(100);
        }
    }

    static bool IsKeyPressed(int keyCode)
    {
        return (GetAsyncKeyState(keyCode) & 0x8001) != 0;
    }

    static void OnExit(object sender, EventArgs e)
    {
        // Beende den Hintergrundthread und schließe die Anwendung
        backgroundThread.Abort();
        notifyIcon.Visible = false;
        Application.Exit();
    }
}
