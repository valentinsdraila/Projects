using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AutomatedUnitTests")]
namespace Common
{
    public enum EnumInstallationType
    {
        _None = 0,
        Debug,
        DebugRelease,
        _setup,
        _setupRDC,
        _setupAP,
        _setupDT,
        _setupTCB,
        _setupStart,
        _setupTL
    }

    public enum EnumStatus
    {
        TODO = 0,
        BUSY,
        WAITING,
        DONE,
        ERROR,
        HIJACK,
        OTHER
    }

    public enum EnumPlatform
    {
        e32Bit,
        e64Bit
    }

    public static class CEnumUtils
    {
        public static string GetInstallationName(EnumInstallationType instType)
        {
            string fullName;
            switch (instType)
            {
                case EnumInstallationType._setup:
                    fullName = "Testlab Extended";
                    break;
                case EnumInstallationType._setupStart:
                case EnumInstallationType._setupTL:
                    fullName = "Testlab";
                    break;
                case EnumInstallationType._setupRDC:
                    fullName = "Recorder data convertor";
                    break;
                case EnumInstallationType._setupAP:
                    fullName = "Active Pictures";
                    break;
                case EnumInstallationType._setupDT:
                    fullName = "Data Tools";
                    break;
                case EnumInstallationType._setupTCB:
                    fullName = "Test Cloud Blueprint";
                    break;
                default:
                    fullName = "Unknown";
                    break;
            }

            return fullName;
        }

        public static EnumInstallationType GetInstallationFromAbbreviation(string abbreviation)
        {
            var abbr = Abbreviations.Where(c => c.Value.Equals(abbreviation)).Select(d => d.Key);

            return abbr.Any() ? abbr.FirstOrDefault() : EnumInstallationType._None;
        }

        public static string GetInstallationAbbreviationForLabel(EnumInstallationType instType)
        {
            switch (instType)
            {
                case EnumInstallationType._setup:
                case EnumInstallationType._setupStart:
                    return "";
                default:
                    return GetInstallationAbbreviation(instType);
            }
        }

        public static string GetInstallationAbbreviation(EnumInstallationType instType)
        {
            var abbr = Abbreviations.Where(c => c.Key.Equals(instType)).Select(d => d.Value);

            return abbr.Any() ? abbr.FirstOrDefault() : "Unknown";
        }

        public static readonly string UnknownAbbreviation = "Unknown";

        private static readonly IList<KeyValuePair<EnumInstallationType, string>> Abbreviations =
            new List<KeyValuePair<EnumInstallationType, string>>
            {
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setup, "TL"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupTL, "TL"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupStart, "TLSTART"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupRDC, "RDC"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupAP, "AP"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupDT, "DT"),
                new KeyValuePair<EnumInstallationType, string>(EnumInstallationType._setupTCB, "TCB"),
            };

        /// <summary>
        /// convert EnumInstallationType from the values in the Caches db.
        /// NOTE!! any release will match _Setup
        /// </summary>
        /// <param name="inst"></param>
        /// <returns></returns>
        public static string TranslateInstallationTypeToCachesDbType(EnumInstallationType inst)
        {
            switch (inst)
            {
                case EnumInstallationType.Debug:
                    return "debug";
                case EnumInstallationType.DebugRelease:
                    return "debugrelease";
                default:
                    return "release";
            }
        }


        /// <summary>
        /// convert from the values in the Caches db to EnumInstallationType.
        /// NOTE!! any release will match _Setup
        /// </summary>
        /// <param name="inst"></param>
        /// <returns></returns>
        public static EnumInstallationType TranslateCachesDbTypeToInstallationType(string inst)
        {
            if (inst.Equals("debug"))
                return EnumInstallationType.Debug;
            else if (inst.Equals("debugrelease"))
                return EnumInstallationType.DebugRelease;
            else
                return EnumInstallationType._setup;
        }

        public static bool IsInstallationType(EnumInstallationType type)
        {
            switch (type)
            {
                case EnumInstallationType._setup:
                case EnumInstallationType._setupStart:
                case EnumInstallationType._setupTCB:
                case EnumInstallationType._setupAP:
                case EnumInstallationType._setupDT:
                case EnumInstallationType._setupRDC:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsTestLabBasedInstallation(EnumInstallationType type)
        {
            switch (type)
            {
                case EnumInstallationType._setup:
                case EnumInstallationType._setupStart:
                case EnumInstallationType._setupAP:
                case EnumInstallationType._setupDT:
                case EnumInstallationType._setupRDC:
                    return true;
                case EnumInstallationType._setupTCB:
                    return false;
                default:
                    return false;
            }
        }

        public static bool IsInstallationType(string type)
        {
            Enum.TryParse(type, true, out EnumInstallationType eType);
            return IsInstallationType(eType);
        }

        public static bool IsInstallationType(string type, out EnumInstallationType installationType)
        {
            var success = Enum.TryParse(type, true, out EnumInstallationType eType);

            installationType = success ? eType : EnumInstallationType._None;

            return IsInstallationType(eType);
        }

        public static bool IsStartupFashion(EnumInstallationType type)
        {
            switch (type)
            {
                case EnumInstallationType._setupStart:
                    return true;
                default:
                    return false;
            }
        }


        internal static readonly Dictionary<EnumInstallationType, IList<EnumInstallationType>> subCategories
            = new Dictionary<EnumInstallationType, IList<EnumInstallationType>>()
        {
            { EnumInstallationType._setup, new List<EnumInstallationType>(){ EnumInstallationType._setupStart } }
        };

        /// <summary>
        /// This function return the 'not Startup' variant of the fashion:
        /// e.g.: _SetupStart used the _Setup build etc.
        /// </summary>
        /// <param name="fashion"></param>
        /// <returns></returns>
        public static EnumInstallationType GetBaseFashion(EnumInstallationType fashion)
        {

            // Not specified as subtype...
            if(!subCategories.Any(c => c.Value.Contains(fashion)))
            {
                return fashion;
            }

            var whereBasetype = subCategories.Where(c => c.Value.Contains(fashion));
            if(whereBasetype.Count() == 1)
            {
                return whereBasetype.First().Key;
            }
            else
            {
                throw new ArgumentException($"{fashion} has no or too many base types");
            }
        }

        public static string GetBaseFashion(string fashion)
        {
            bool validEnum = Enum.TryParse(fashion, true, out EnumInstallationType type);
            if (!validEnum)
            {
                throw new ArgumentException($"{fashion} cannot be parsed to EnumInstallationType");
            }

            return GetBaseFashion(type).ToString();
        }

        public static IList<EnumInstallationType> GetSubFashion(EnumInstallationType fashion)
        {
            IList<EnumInstallationType> result = new List<EnumInstallationType>() { };
            if(subCategories.ContainsKey(fashion))
            {
                result = subCategories[fashion];
            }

            return result;
        }

        public static IList<string> GetSubFashion(string fashion)
        {
            bool validEnum = Enum.TryParse(fashion, true, out EnumInstallationType type);
            if (!validEnum)
            {
                throw new ArgumentException($"{fashion} cannot be parsed to EnumInstallationType");
            }

            var results = GetSubFashion(type).Select(c => c.ToString()).ToList();

            return results;
        }
    }

    public class PlatformUtils
    {

        public static readonly string DbValue64Bit = "intel_amd64_winxp";
        public static readonly string DbValue32Bit = "intel_winnt4.0";


        protected static readonly IDictionary<EnumPlatform, string> Values = new Dictionary<EnumPlatform, string>
        {
            {EnumPlatform.e32Bit, DbValue32Bit},
            {EnumPlatform.e64Bit, DbValue64Bit},
        };

        public static EnumPlatform GetPlatform(bool is64Bit)
        {
            return is64Bit ? EnumPlatform.e64Bit : EnumPlatform.e32Bit;
        }

        public static string GetPlatformStr(bool is64Bit)
        {
            var value = Values.FirstOrDefault(c => c.Key == GetPlatform(is64Bit)).Value;
            return value;
        }

        public static string GetPlatform(EnumPlatform platform)
        {
            var value = Values.FirstOrDefault(c => c.Key == platform).Value;
            return value;
        }

        public static EnumPlatform GetPlatform(string platform)
        {
            return GetPlatform(Is64Bit(platform));
        }


        public static bool Is64Bit(string platform)
        {
            return platform.Contains("64");
        }

        public static bool Is64Bit(EnumPlatform platform)
        {
            return platform == EnumPlatform.e64Bit;
        }
    }
}
