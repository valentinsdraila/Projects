using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Common;

namespace StoryUtils2
{

    public sealed class ShortLabelExpert
    {

        public static string GetShortLabelFromInstallationPath(EnumInstallationType type, string installerPath, string release)
        {
            var shortLabel = "";

            if (string.IsNullOrEmpty(installerPath))
            {
                throw new ArgumentException( $"ShortLabelExpert for {type}: Can't handle empty installer path: {installerPath}");
            }

            installerPath = RemoveExeIfNeeded(installerPath);


            if (CEnumUtils.IsTestLabBasedInstallation(type))
            {
                string[] parts = installerPath.Split(Path.DirectorySeparatorChar);
                string lv_sResult = parts[8];
                if (parts[7] == "version64")
                    lv_sResult += "_64Bit";

                shortLabel = lv_sResult;

            }
            else if (type == EnumInstallationType._setupTCB)
            {
                // \\belnspdev006.net.plm.eds.com\CD-Layouts\TCB\intern\Core\2206\221221_1.1.1_core

                // assume:
                var language = "en";
                var is64Bit = true;

                var parts = installerPath.Split(Path.DirectorySeparatorChar);
                var partWithDate = parts.Last();

                var date = partWithDate.Substring(0, 6);

                shortLabel = $"{release}.{date}_{language}_{CEnumUtils.GetInstallationAbbreviationForLabel(type)}";
                if (is64Bit)
                {
                    shortLabel += "_64Bit";
                }
            }
            else
            {
                throw new ArgumentException($"ShortLabelExpert for {type}, {installerPath}: Can't handle type {type}");
            }

            return shortLabel;
        }

        public static string GetSimplePart(string extendedShortLabel)
        {
            // TODO: workaround...
            var partsDot = extendedShortLabel.Split('.');
            var datePart = partsDot[1].Split('_')[0];

            return $"{partsDot[0]}.{datePart}";
        }

        private static string RemoveExeIfNeeded(string installerPath)
        {
            installerPath.Trim();
            // TODO: Get/HasExtension detects '.', can't verify whether we have a file or directory
            // TODO: File.GetAttributes needs the file to exist

            //var attr = File.GetAttributes(installerPath);
            //if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
            //{
            //    installerPath = Path.GetDirectoryName(installerPath);
            //}

            //if(Path.GetExtension(installerPath) != "")
            //{
            //    installerPath = Path.GetDirectoryName(installerPath);
            //}

            var lastPart = installerPath.Split(System.IO.Path.DirectorySeparatorChar).Last().Trim();
            if (lastPart.EndsWith(".exe") || lastPart.EndsWith(".msi"))
            {
                installerPath = Path.GetDirectoryName(installerPath);
            }

            return installerPath;
        }
    }


    /// <summary>
    /// A class to parse a build Label and get the info.
    /// </summary>
    public sealed class LabelExpert
    {
        // TODO: improve class
        //   * Extend + use on more locations
        //   * Have release info in the parsers, such that changing inputLabel format doesn't mean we have to make even more if-soup

        /*
         * examples
         *
            _setupTCB    -> \\belnspdev006.net.plm.eds.com\CD-Layouts\TCB\intern\Core\2206\221222_1.1.1_core
            _setup   -> 23A.220825_en_64Bit
            _setupAP   -> \\belnspdev006\CD-Layouts\Test.Lab\Release_23A\intern\version64\23A.220826_en_DP
            _setupDT   -> 19A.190515_en_DT_64Bit
            _setupStart
            _setupAP
            220826233944_23A.220826_2019.20_debug_64_en
            220827035228_22A.220826_2019.20_debug_32_en
            220826234312_22A.220826_2019.20_debug_64_en
            201211135800_21A.201210-1_2019.20_debugrelease_32_en
            
            Personal build
            22A-newSetup-IEEE1588.Debugx86.PersonalBuild
            \\BELNSPDEVBLD02\22A-newSetup-IEEE1588.Debugx86
            22A.PersonalBuild.220330
         */

        public static readonly string DbValue64Bit = "intel_amd64_winxp";
        public static readonly string DbValue32Bit = "intel_winnt4.0";


        private static readonly Lazy<LabelExpert> Lazy =
            new Lazy<LabelExpert>(() => new LabelExpert());

        public static LabelExpert Instance => Lazy.Value;

        private LabelExpert()
        {
            //DebugLog("test");
        }


        private static Action<string> DebugLog = Console.WriteLine;

        public static LabelExpert WithLogger(Action<string> logger)
        {
            DebugLog = logger;
            return Instance;
        }


        private readonly ConcurrentDictionary<string, ILabel> _convertedLabels = new ConcurrentDictionary<string, ILabel>();


        public ILabel ParseLabel(string label, out bool successful, out IList<string> issues)
        {
            DebugLog($"Request for {label}");
            ILabel result = new NullLabel();
            bool successParse = false;
            IList<string> issueList = new List<string>();

            if (_convertedLabels.ContainsKey(label))
            {
                DebugLog($"Exists in cache");
                if(_convertedLabels.TryGetValue(label, out var labelObject))
                {
                    DebugLog($"Found in cache");
                    successParse = true;
                    result = labelObject;
                }
            }

            if(result is NullLabel)
            {
                DebugLog($"New parsing needed for {label}");
                result = InitFromLabel(label, out successParse, out issueList);
                DebugLog($"Parse {(successParse ? "V" : "X")}: {result.GetType()} -> {GetBuildDate(result.BuildDate)}");

                AddToCollection(label, result);
            }

            successful = successParse;
            issues = issueList;
            return result;
        }

        private void AddToCollection(string label, ILabel labelObject)
        {
            DebugLog(_convertedLabels.TryAdd(label, labelObject)
                ? $"Added {label} to collection"
                : $"Issue adding {label} to collection");
        }

        public ILabel InitFromLabel(string inputLabel, out bool successful, out IList<string> issues)
        {
            issues = new List<string>();

            ILabel labelObject = new NullLabel();
            var success = true;

            if (inputLabel == null)
            {
                issues.Add($"Label is null...");
                successful = false;
                return labelObject;
            }


            try
            {
                //if (IsPersonalBuild(inputLabel))
                //{
                //    success = false;

                //    try
                //    {
                //        var builds = CBuildsPerSite.ReadLabelFromDatabase(new UtilitiesFactory(), inputLabel, false);
                //        if (builds.Any())
                //        {
                //            var build = builds.FirstOrDefault();

                //            var shortLabel = build.VersionControlLabel;
                //            int idx = shortLabel.LastIndexOf('.');

                //            var date = "";
                //            if (idx != -1)
                //            {
                //                date = shortLabel.Substring(idx + 1);
                //            }

                //            var fashion = build.BuildProperties.Fashion.Equals("debug",
                //                StringComparison.InvariantCultureIgnoreCase)
                //                ? EnumInstallationType.Debug
                //                : build.BuildProperties.Fashion.Equals("release",
                //                    StringComparison.InvariantCultureIgnoreCase) ? EnumInstallationType._setup : EnumInstallationType.DebugRelease;

                //            if (CEnumUtils.IsInstallationType(fashion))
                //            {
                //                labelObject = new SetupLabel(build.BuildProperties.Release,
                //                    build.BuildProperties.Platform.Contains("64"),
                //                    date,
                //                    "",
                //                    fashion,
                //                    true);
                //            }
                //            else
                //            {
                //                labelObject = new DebugLabel(build.BuildProperties.Release,
                //                    build.BuildProperties.Platform,
                //                    date,
                //                    "",
                //                    fashion,
                //                    true);
                //            }
                //            labelObject.BuildProperties = build.BuildProperties;

                //            success = true;
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        Debug.WriteLine(e);
                //        success = false;
                //        issues.Add($"Issue for Personal Build: {e.Message}");
                //    }
                //}
                //else if (IsDebug(inputLabel))
                if (IsDebug(inputLabel))
                {
                    // 220826233944_23A.220826_2019.20_debug_64_en
                    // 201211135800_21A.201210-1_2019.20_debugrelease_32_en


                    var oldLabelFormat = false;
                    var weirdLabel = IsWeirdLabel(inputLabel);
                    var whatExactly = $"Start (weird {weirdLabel})";

                    try
                    {
                        whatExactly = $"Splitting '.'";
                        var dotParts = inputLabel.Split('.');

                        var first = dotParts[0];
                        var last = dotParts[dotParts.Length - 1];
                        var middle = "";
                        if (dotParts.Length > 2)
                        {
                            middle = inputLabel.Replace($"{first}.", "").Replace($".{last}", "");
                        }
                        else
                        {
                            whatExactly += $" Old label? (weird: {weirdLabel})";
                            oldLabelFormat = true;
                            middle = last.Split('_')[0];
                            last = last.Replace($"{middle}_", "");
                        }


                        whatExactly = $"Splitting 'firstPart' on '_'";
                        var firstPart = first.Split('_');
                        whatExactly += $" %{string.Join(" _ ", firstPart)}% ";
                        var hieroglyphs = firstPart[0];
                        var release = first.Replace($"{hieroglyphs}_", "");


                        var buildDate = "";
                        var buildOrder = 0;
                        if (!oldLabelFormat)
                        {
                            whatExactly = $"Splitting 'secondPart' on '_'";
                            var secondPart = middle.Split('_');
                            whatExactly += $" %{string.Join(" _ ", secondPart)}% ";
                            var labelDatePart = secondPart[0];
                            var net = secondPart[1];
                            buildDate = labelDatePart;
                            if (labelDatePart.Contains('-'))
                            {
                                var dateParts = labelDatePart.Split('-');
                                buildDate = dateParts[0];
                                buildOrder = int.Parse(dateParts[1]);
                            } 
                        }
                        else
                        {
                            whatExactly = $"Parsing 'secondPart' for Old";
                            buildDate = middle;
                        }


                        whatExactly = $"Splitting 'thirdPart' on '_'";
                        var thirdPart = last.Split('_');
                        whatExactly += $" %{string.Join(" _ ", last)}% ";

                        var visualStudio = thirdPart[0];
                        var fashion = thirdPart[1];
                        var platform = thirdPart[2];
                        var language = thirdPart[3];


                        whatExactly = $"Paring fashion etc.";
                        var parseSuccess = Enum.TryParse(fashion, true, out EnumInstallationType instType);
                        var goodFashion = parseSuccess && !CEnumUtils.IsInstallationType(instType);
                        if (!goodFashion)
                        {
                            success = false;
                            issues.Add($"Failed to parse {fashion} as non-installation enum. (labelObject: {instType})");
                        }

                        labelObject = new DebugLabel(release, platform, buildDate, language, instType)
                        {
                            VisualStudio = visualStudio,
                            BuildOrder = buildOrder,
                            //BuildProperties = new CBuildProps()
                        };
                    }
                    catch (Exception e)
                    {
                        var errorMessage = $"Error parsing {inputLabel} as DebugLabel at '{whatExactly}'. {e.Message}";
                        throw new ArgumentException(errorMessage, e);
                    }

                }
                else if (IsSetup(inputLabel))
                {
                    //23A.220825_en
                    //23A.220825_en_64Bit
                    //19A.190515_en_DT_64Bit
                    //19A.190515_en_DT

                    var splitRelease = inputLabel.Split('.');
                    var release = splitRelease[0];

                    var parts = splitRelease[1].Split('_');

                    var buildDate = parts[0];
                    var language = parts[1];


                    var fashion = EnumInstallationType._setup;
                    //var platform = "";

                    var partsInterpret = new List<string>();
                    for (int i = 2; i < parts.Length; i++)
                    {
                        partsInterpret.Add(parts[i]);
                    }

                    var is64Bit = partsInterpret.Any(c => c.Contains("64"));
                    var inst = partsInterpret.Where(d =>
                        !CEnumUtils.GetInstallationFromAbbreviation(d).Equals(EnumInstallationType._None));
                    if (inst.Any())
                    {
                        fashion = CEnumUtils.GetInstallationFromAbbreviation(inst.FirstOrDefault());
                    }

                    labelObject = new SetupLabel(release, is64Bit, buildDate, language, fashion)
                    {
                        //BuildProperties = new CBuildProps()
                    };

                }
                else
                {
                    success = false;
                    labelObject = new NullLabel();
                }
            }
            catch (Exception ex)
            {
                success = false;
                issues.Add(ex.ToString());
                Debug.WriteLine(ex);
            }

            successful = success;

            return labelObject;
        }

        /// <summary>
        /// For tuning the class... Some e.g. 18 labels don't follow the current convention.
        /// </summary>
        /// <param name="inputLabel"></param>
        /// <returns></returns>
        private bool IsWeirdLabel(string inputLabel)
        {
            var isWeird = false;
            try
            {
                var dotParts = inputLabel.Split('.');
                if (dotParts.Length < 3)
                {
                    return true;
                }

                if (dotParts[0].Split('_').Length < 2)
                {
                    return true;
                }
                if (dotParts[1].Split('_').Length < 2)
                {
                    return true;
                }
                if (dotParts[2].Split('_').Length < 4)
                {
                    return true;
                }


            }
            catch (Exception)
            {
                // ignored
                return true;
            }

            return isWeird;
        }

        private bool IsSetup(string label)
        {
            // TODO: maybe a better way...
            var parts = label.Split('_');
            return parts.Length >= 2 && parts.Length <= 4; // TODO: check 32 bit => also suffix?
        }

        private bool IsDebug(string label)
        {
            return label.Split('_').Length >= 6;
        }

        private bool IsPersonalBuild(string label)
        {
            return label.EndsWith(".PersonalBuild", StringComparison.CurrentCultureIgnoreCase);
        }

        public static string GetBuildDate(DateTime date)
        {
            return date.ToString("yyMMdd");
        }

        public static DateTime GetBuildDate(string date)
        {
            return DateTime.ParseExact(date, "yyMMdd", new CultureInfo("en-US"));
        }
    }


    public static class LabelExt
    {
        public static string ToStr(this ILabel label)
        {
            return $"{label.Release} - {label.Platform} - {label.Fashion} ({label.GetLabelFromProperties()})";
        }
    }

    public interface ILabel
    {
        string GetLabelFromProperties();

        string Release { get; }
        string Platform { get; }
        bool Is64Bit { get; }
        EnumInstallationType Fashion { get; }
        DateTime BuildDate { get; }
        bool IsPersonalBuild { get; }
        //IBuildProperties BuildProperties { get; set; }
    }

    public class NullLabel : ILabel
    {
        public string GetLabelFromProperties()
        {
            return "";
        }

        public string Release { get; set; }
        public string Platform { get; set; }
        public bool Is64Bit { get; set; }
        public EnumInstallationType Fashion { get; set; }
        public DateTime BuildDate { get; set; }
        public bool IsPersonalBuild { get; set; }
        //public IBuildProperties BuildProperties { get; set; }
    }

    public class SetupLabel : ILabel
    {
        public SetupLabel(string release, bool is64Bit, string buildDate, string language,
            EnumInstallationType fashion, bool isPersonalBuild = false)
        {
            Release = release;
            Is64Bit = is64Bit;
            Fashion = fashion;
            Language = language;

            IsPersonalBuild = isPersonalBuild;

            BuildDate = LabelExpert.GetBuildDate(buildDate);
        }

        

        public string GetLabelFromProperties()
        {
            //23A.220825_en_64Bit
            var label = $"{Release}.{LabelExpert.GetBuildDate(BuildDate)}_{Language.ToLower()}{(Is64Bit ? "_64Bit" : "")}";
            return label;
        }

        public string Release { get; }
        public string Platform => Is64Bit ? LabelExpert.DbValue64Bit : LabelExpert.DbValue32Bit;
        public bool Is64Bit { get; private set; }
        public EnumInstallationType Fashion { get; }
        public DateTime BuildDate { get; }
        public bool IsPersonalBuild { get; }
        public string Language { get; }
        //public IBuildProperties BuildProperties { get; set; }
    }

    public class DebugLabel : ILabel
    {
        public DebugLabel(string release, string platform, string buildDate, string language,
            EnumInstallationType fashion = EnumInstallationType.Debug, bool isPersonalBuild = false)
        {
            Release = release;
            Fashion = fashion;
            Is64Bit = platform.Contains("64");
            Language = language;

            IsPersonalBuild = isPersonalBuild;

            BuildDate = LabelExpert.GetBuildDate(buildDate);
        }



        public string GetLabelFromProperties()
        {
            // 220826233944_23A.220826_2019.20_debug_64_en
            
            var hieroglyphs = $"{LabelExpert.GetBuildDate(new DateTime(2000, 1, 1))}000000";

            var shortLabel = $"{Release}.{LabelExpert.GetBuildDate(BuildDate)}";
            var visualStudio = "2019.20";

            // note on the hieroglyphs/buildOrder: a rebuild usually happens the day after -> hieroglyphs different
            var buildOrderPart = BuildOrder == 0 ? "" : $"-{BuildOrder}";

            var label = $"{hieroglyphs}_{shortLabel}{buildOrderPart}_{visualStudio}_{Fashion.ToString().ToLower()}_{(Platform.Contains("64") ? 64 : 32)}_{Language.ToLower()}";

            return label;
        }


        public string Release { get; }
        public string Platform => Is64Bit ? LabelExpert.DbValue64Bit : LabelExpert.DbValue32Bit;
        public EnumInstallationType Fashion { get; }
        public DateTime BuildDate { get; }
        public string Language { get; }
        public string VisualStudio { get; set; }
        public int BuildOrder { get; set; }

        public bool Is64Bit { get; }

        public bool IsPersonalBuild { get; }
        //public IBuildProperties BuildProperties { get; set; }
    }
}