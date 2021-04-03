using Microsoft.Win32;
using System;
using System.Text;
using System.Windows.Forms;

namespace Nvidia_Grouping_Tool {
    public static class NvidiaRetriever {

        private static int recordsDuration;
        public static int RecordsDuration {
            get {
                UpdateValues();
                return recordsDuration;
            }
        }

        private static string recordsLocation;
        public static string RecordsLocation {
            get {
                UpdateValues();
                return recordsLocation;
            }
        }


        static NvidiaRetriever() {
            UpdateValues();
        }

        private static void UpdateValues() {
            try {
                // Registry where values are stored.
                RegistryKey sk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\NVIDIA Corporation\\Global\\ShadowPlay\\NVSPCAPS");

                if (sk != null) {
                    // Retrieve records folder location.
                    byte[] locationReg = (byte[])sk.GetValue("DefaultPathW");
                    string folder = Encoding.Unicode.GetString(locationReg);
                    recordsLocation = folder.Substring(0, folder.Length - 1);

                    // Retrieve records duration.
                    byte[] durationReg = (byte[])sk.GetValue("DVRBufferLen");
                    string[] hexValues = BitConverter.ToString(durationReg).Split('-');
                    recordsDuration = Convert.ToInt32(hexValues[1] + hexValues[0], 16);
                } else {
                    MessageBox.Show("Is Geforce Experience installed on your computer ? If so this might be a bug, try again or report it please.");
                    Application.Exit();
                }

                sk.Close();

            } catch (Exception e) {
                MessageBox.Show("An error occured (please report it):\n" + e.ToString());
                Environment.Exit(1);
            }
        }


    }
}
