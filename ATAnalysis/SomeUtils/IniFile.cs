using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PmsSettings.Util
{

    public interface IIniFile
    {
        string Path { get; set; }
        string Read(string section, string key);
        int ReadInt(string section, string key, int defaultValue = -1);
        void Write(string section, string key, string value);
    }

    public class IniFile : IIniFile
    {
        public static readonly string ValueNotFound = "Value Not Found";

        public string Path { get; set; }

        public IniFile(string path)
        {
            Path = path;
        }


        // Read data from the INI file
        public string Read(string section, string key)
        {
            return Read(section, key, ValueNotFound);
        }

        public string Read(string section, string key, string defaultValue)
        {
            var stringBuilder = new StringBuilder(800);
            int i = NativeMethods.GetPrivateProfileString(section, key, defaultValue, stringBuilder, 800, Path);

            return stringBuilder.ToString();
        }

        public int ReadInt(string section, string key, int defaultValue = -1)
        {
            uint i = NativeMethods.GetPrivateProfileInt(section, key, defaultValue, Path);
            return Convert.ToInt32(i);
        }

        // Write data to the INI file
        public void Write(string section, string key, string value)
        {
            NativeMethods.WritePrivateProfileString(section, key, value, Path);
        }

        public IList<string> GetSections()
        {
            byte[] buffer = new byte[1024];
            int i = NativeMethods.GetPrivateProfileString(null, null, ValueNotFound, buffer, 1024, Path);
            var str = Encoding.Default.GetString(buffer);
            var sections = str.Split('\0').Where(c => !string.IsNullOrEmpty(c)).ToList();

            return sections;
        }

        public IList<KeyValuePair<string, string>> GetKeyValuePairs(string section)
        {
            byte[] buffer = new byte[1024];
            int i = NativeMethods.GetPrivateProfileString(section, null, ValueNotFound, buffer, 1024, Path);
            var str = Encoding.Default.GetString(buffer);
            var keys = str.Split('\0').Where(c => !string.IsNullOrEmpty(c)).ToList();

            var result = new List<KeyValuePair<string, string>>();
            foreach (var key in keys)
            {
                result.Add(new KeyValuePair<string, string>(key, Read(section,key)));
            }

            return result;
        }
    }
}
