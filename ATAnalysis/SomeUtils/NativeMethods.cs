using System.Runtime.InteropServices;
using System.Text;

namespace PmsSettings.Util
{
    public class NativeMethods
    {
        #region Ini file
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder returnValue, int size, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, byte[] buffer, int size, string filePath);

        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string section, string key, int def, string filePath);
        #endregion
    }
}
