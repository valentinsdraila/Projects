using Common;
using System;
using System.Diagnostics;

namespace ScheduleCmd
{
    /// <summary>
    /// A message in the form 
    /// 'ScheduleAutTest|45538|10|230214232631_22A.230214_2019.20_debug_32_en|MACHINE07|y8cage|0|intel_winnt4.0|Debug|DI2BELVN0053WPC|EN|0|0'
    /// </summary>
    public class TheMessageToSend
    {
        private Action<string> _logger;

        /// <summary>
        /// This is a real message.
        /// </summary>
        /// <param name="logger">A logger, a way to pass the logger of the main program to here.</param>
        public TheMessageToSend(Action<string> logger)
        {
            _logger = logger;
        }


        public string Tag => "ScheduleAutTest";

        public int ScenarioId { get; set; }
        public int Prio { get; set; } = 5;
        public string BuildLabel { get; set; }
        public string TestSystemName { get; set; } = ""; // Emtpy string = 'Any system', which is the best default
        public string User { get; set; } = "";
        public bool MailResult { get; set; } = true;
        public string MachineName { get; set; } = Environment.MachineName;
        public string Platform { get; set; }
        public EnumInstallationType Fashion { get; set; }
        public string Language { get; set; } = "EN"; // most common case is 'EN'. Other option might be 'SYM', which is localized aka symbolic
        public bool Coverage => false; // Use default, Picasso coverage is disabled
        public bool NetCoverage { get; set; } = false;


        private bool _successfulParsePerformed = false;
        public bool SuccessfullyParsed => _successfulParsePerformed;
        public bool SomeSanityCheck => ScenarioId != 0 && !string.IsNullOrEmpty(User);

        /// <summary>
        /// Parses the build label and sets properties. For now only Debug is supported.
        /// </summary>
        /// <param name="builLabel"></param>
        /// <returns></returns>
        public bool SetLabelProps(string builLabel)
        {
            try
            {
                var label = StoryUtils2.LabelExpert.WithLogger(_logger).InitFromLabel(builLabel, out var success, out var issues);
                Console.WriteLine(builLabel);
                if (success)
                {
                    _successfulParsePerformed = true;
                    BuildLabel = builLabel;
                    Platform = label.Platform;
                    Fashion = label.Fashion;

                    if (!Fashion.Equals(EnumInstallationType.Debug))
                    {
                        // for e.g. _Setup we need an installation label. The Label has no knowledge of this.
                        _successfulParsePerformed = false;
                        _logger($"Unsupported type ({Fashion}). Only Debug is allowed.");
                    }
                }

                else
                {
                    _successfulParsePerformed = false;

                    _logger("Failed to parse label:");
                    foreach (var item in issues)
                    {
                        _logger($" * {item}");
                    }
                }

                return success;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _successfulParsePerformed = false;
                return false;
            }
        }

        public override string ToString()
        {
            // ScheduleAutTest|45538|10|230214232631_22A.230214_2019.20_debug_32_en|MACHINE07|y8cage|0|intel_winnt4.0|Debug|DI2BELVN0053WPC|EN|0|0

            return $"ScheduleAutTest|{ScenarioId}|{Prio}|{BuildLabel}|{TestSystemName}|{User}|{(MailResult ? 1 : 0)}|{Platform}|{Fashion}|{MachineName}|{Language}|{(Coverage ? 1 : 0)}|{(NetCoverage ? 1 : 0)}";
        }
    }


    public class MessagePing : TheMessageToSend
    {
        public new string Tag => "Ping";
        public string Text { get; set; }

        public new bool SuccessfullyParsed => true; // overrule base
        public new bool SomeSanityCheck => true; // overrule base

        /// <summary>
        /// A test message, possibility to add some text
        /// </summary>
        /// <param name="logger">A logger, a way to pass the logger of the main program to here.</param>
        /// <param name="text">The text to add to the message</param>
        public MessagePing(Action<string> logger, string text = "Test") : base(logger)
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"{Tag}|{Text}";
        }
    }

    public class MessageTestParent : TheMessageToSend
    {
        /// <summary>
        /// A test message, which add a real message as text
        /// </summary>
        /// <param name="logger">A logger, a way to pass the logger of the main program to here.</param>
        public MessageTestParent(Action<string> logger) : base(logger)
        {
        }

        public new string Tag => "Ping";
        public override string ToString()
        {
            return $"{Tag}|{base.ToString()}";
        }
    }
}