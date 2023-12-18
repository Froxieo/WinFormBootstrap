using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Security.Policy;

namespace FormBootstrap
{
    public partial class Form1 : Form
    {
        WebClient web = new WebClient();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\AppData\\Roaming\\.FLauncher\\Launcher\\";

        string diskLauncherVersion;
        string diskJDKVersion;
        string OnlineLauncherVersion;
        string OnlineJDKVersion;

        int i = 0;


        public Form1()
        {
            InitializeComponent(); label1.Text = "Starting ...";
            Main();
        }
        async void VerifyIfAlreadyInstall()
        {
            void Launcher()
            {
                if (File.Exists(path+"Launcher.jar"))
                {
                    label1.Text = "Le Launcher est deja installé, skipping...";
                }
                else
                {
                    DownloadLauncher(); 
                }
                return;
            }
            void JDK()
            {
                if (Directory.Exists(path+"JDK"))
                {
                    label1.Text ="Java est deja installé, skipping...";
                }
                else
                {
                    DownloadJDK();
                }
                return;
            }
            if (File.Exists(path+"Launcher.jar") && Directory.Exists(path+"JDK"))
            {
                VersionManager();
            }
            Launcher();
            await CustomWaitAsync(1000);
            JDK();
            await CustomWaitAsync(1000);
            return;
        }
        void Launcherupdate()
        {
            if(File.Exists(path+"launcher-version.txt") &&  File.Exists(path+"jdk-version.txt"))
            {
                VersionManager();
            }
        }
        void VersionManager()
        {
            GetVersion();
            if (diskLauncherVersion != OnlineLauncherVersion)
            {
                DownloadLauncher();
            }
            if (diskJDKVersion != OnlineJDKVersion)
            {
                DownloadJDK();
            }
            if ((diskJDKVersion == OnlineJDKVersion) && (diskLauncherVersion == OnlineLauncherVersion))
            {
                Console.WriteLine("All Good !");
                start();
            }
        }
        void createFileAndDir()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return;
        }

        void GetVersion()
        {
            StreamReader GetdiskJDKVersion = new StreamReader(path+"jdk-version.txt");
            diskJDKVersion = GetdiskJDKVersion.ReadToEnd();
            GetdiskJDKVersion.Close();
            StreamReader GetdiskLauncherVersion = new StreamReader(path+"launcher-version.txt");
            diskLauncherVersion = GetdiskLauncherVersion.ReadToEnd();
            GetdiskLauncherVersion.Close();

            OnlineJDKVersion = web.DownloadString("http://129.151.254.89/update/Launcher-JDK/jdk-version.txt");
            OnlineLauncherVersion = web.DownloadString("http://129.151.254.89/update/Launcher-JDK/launcher-version.txt");
        }
        async void Main()
        {
            await CustomWaitAsync(1000); createFileAndDir(); VerifyIfAlreadyInstall();
        }
        async void DownloadLauncher()
        {
            if (File.Exists(path+"launcher.jar"))
            {
                File.Delete(path+"launcher.jar");
            }

            label1.Text = "Downloading Launcher ..."; await CustomWaitAsync(1000); web.DownloadFile("http://129.151.254.89/update/Launcher-JDK/javafx-launcher-1.0.0-all.jar", path+"launcher.jar");   updateDisplay("Launcher Successfully downloaded");
            File.Delete(path+"launcher-version.txt");   GetLauncherVersion(); label1.Text = "Launcher is now install"; 
            await CustomWaitAsync(1000); Launcherupdate();
        }
        async void DownloadJDK()
        {
            if (Directory.Exists(path+"JDK"))
            {
                Directory.Delete(path+"JDK", true);
            }

            label1.Text = "Downloading JDK ..."; await CustomWaitAsync(1000); web.DownloadFile("https://download.oracle.com/java/17/latest/jdk-17_windows-x64_bin.zip", path+"jdk.zip"); ZipFile.ExtractToDirectory(path+"jdk.zip", path+"JDK"); File.Delete(path+"jdk.zip"); updateDisplay("JDK Successfully downloaded");
            File.Delete(path+"jdk-version.txt"); label1.Text = "Java is now install";
            await CustomWaitAsync(1000); GetJDKVersion(); VersionManager();
        }
        async void start()
        {
            string execLauncher = path+"JDK\\jdk-17.0.9\\bin\\java.exe -jar "+path+"launcher.jar";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("powershell.exe",execLauncher);
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            await CustomWaitAsync(2000); this.Close();
        }
        void GetLauncherVersion()
        {
            WebClient getlauncherversion = new WebClient();
            getlauncherversion.DownloadFile("http://129.151.254.89/update/Launcher-JDK/launcher-version.txt", path+"launcher-version.txt");
            return;
        }
        void GetJDKVersion()
        {
            WebClient getjdkversion = new WebClient();
            getjdkversion.DownloadFile("http://129.151.254.89/update/Launcher-JDK/jdk-version.txt", path+"jdk-version.txt");
            return;

        }














        void updateDisplay(string content)
        {
            label1.Text = content;
        }
        private static async Task CustomWaitAsync(int milliseconds)
        {
            await Task.Run(() => Thread.Sleep(milliseconds));
        }
    }
}
