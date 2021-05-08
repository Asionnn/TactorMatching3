using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace TactorMatching3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Tactor Methods
        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int Discover(int type);

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int Connect(string name, int type, IntPtr _callback);

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitializeTI();

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int Pulse(int deviceID, int tacNum, int msDuration, int delay);

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetDiscoveredDeviceName(int index);

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
           CallingConvention = CallingConvention.Cdecl)]
        public static extern int ChangeGain(int deviceID, int tacNum, int gainval, int delay);

        [DllImport(@"C:\Users\minisim\Desktop\Tactors\TDKAPI_1.0.6.0\libraries\Windows\TactorInterface.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseAll();
        #endregion

        private static readonly int DURATION_MS = 215;
        private static readonly int DELAY_MS = 20;

        #region setup
        public void SetupTactors()
        {
            if (InitializeTI() == 0)
            {
                System.Diagnostics.Debug.WriteLine("TDK Initialized");
            }

            System.Diagnostics.Debug.WriteLine("Tactors: " + Discover(1));
            string name = Marshal.PtrToStringAnsi((IntPtr)GetDiscoveredDeviceName(0));
            System.Diagnostics.Debug.WriteLine("Name: " + name);


            if (Connect(name, 1, IntPtr.Zero) >= 0)
            {
                System.Diagnostics.Debug.WriteLine("Connected");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not connected");
            }
        }

        public void SetGain()
        {
            for(int x = 1; x <= 15; x++)
            {
                ChangeGain(0, x, 150, 0);
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SetupTactors();
            SetGain();
        }

        public void PulseTactors(int[] tactors, bool withDelay = false, bool parallel = false)
        {
            if (withDelay)
            {
                if (!parallel)
                {
                    for(int x = 0; x < 3; x++)
                    {
                        Pulse(0, tactors[x], DURATION_MS - DELAY_MS * x, DELAY_MS * x);
                    }
                }
                else
                {
                    for(int x = 0; x < 3; x++)
                    {
                        Pulse(0, tactors[x], DURATION_MS - DELAY_MS * x, DELAY_MS * x);
                        Pulse(0, tactors[x + 3], DURATION_MS - DELAY_MS * x, DELAY_MS * x);
                    }
                }
            }
            else
            {
                foreach(int t in tactors)
                {
                    Pulse(0, t, 645, 0);
                }
            }
        }


        private void BtnPanLeft_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 3, 2, 1 }, true);
        }

        private void BtnPanRight_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 6, 5, 4 }, true);
        }

        private void BtnPanAll_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 1, 2, 3, 4, 5, 6 });
        }

        private void BtnPanToLeft_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 6, 7, 3 }, true);
        }

        private void BtnPanToRight_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 3, 7, 6 }, true);
        }

        private void BtnPanToBack_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 1, 2, 3, 4, 5, 6 }, true, true);
        }

        private void BtnBackLeft_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 9, 10, 11 }, true);
        }

        private void BtnBackRight_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 12, 13, 14 }, true);
        }

        private void BtnBackAll_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 9, 10, 11, 12, 13, 14 });
        }

        private void BtnBackToLeft_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 12, 15, 9 }, true);
        }

        private void BtnBackToRight_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 9, 15, 12 }, true);
        }

        private void BtnBackToBack_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 11, 10, 9, 14, 13, 12 }, true, true);
        }

        private void BtnBaseline_Click(object sender, RoutedEventArgs e)
        {
            PulseTactors(new int[] { 3, 6, 9, 12 });
        }
    }
}
