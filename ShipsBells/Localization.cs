using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ShipsBells
{
    class Localization
    {
        public static Dictionary<string, Constants.Locale> ConfigLocaleMap = new Dictionary<string, Constants.Locale>()
        {
            { "en_gb", Constants.Locale.en_gb },
            { "nl_nl", Constants.Locale.nl_nl }
        };

        public static Dictionary<Constants.Locale, string> LocaleConfigMap = new Dictionary<Constants.Locale, string>()
        {
            { Constants.Locale.en_gb, "en_gb" },
            { Constants.Locale.nl_nl, "nl_nl" }
        };

        public static Dictionary<string, Constants.WatchSystem> ConfigWatchSystemMap = new Dictionary<string, Constants.WatchSystem>()
        {
            { "traditional", Constants.WatchSystem.Traditional },
            { "dutch", Constants.WatchSystem.Dutch }
        };

        public static Dictionary<Constants.WatchSystem, string> WatchSystemConfigMap = new Dictionary<Constants.WatchSystem, string>()
        {
            { Constants.WatchSystem.Traditional, "traditional" },
            { Constants.WatchSystem.Dutch, "dutch" }
        };

        public static Dictionary<Constants.Locale, Dictionary<Constants.WatchSystem, string[]>> WatchText = new Dictionary<Constants.Locale, Dictionary<Constants.WatchSystem, string[]>>()
        {
            {
                Constants.Locale.en_gb,
                new Dictionary<Constants.WatchSystem, string[]>()
                {
                    {
                        Constants.WatchSystem.Traditional,
                        new string[]
                        {
                            "first watch",
                            "middle watch",
                            "morning watch",
                            "forenoon watch",
                            "afternoon watch",
                            "first dog watch",
                            "second dog watch"
                        }
                    },
                    {
                        Constants.WatchSystem.Dutch,
                        new string[]
                        {
                            "first watch",
                            "dog watch",
                            "day watch",
                            "forenoon watch",
                            "afternoon watch",
                            "flatfoot watch"
                        }
                    }
                }
            },
            {
                Constants.Locale.nl_nl,
                new Dictionary<Constants.WatchSystem, string[]>()
                {
                    {
                        Constants.WatchSystem.Traditional,
                        new string[]
                        {
                            "eerste wacht",
                            "middenwacht",
                            "ochtendwacht",
                            "voormiddagwacht",
                            "namiddagwacht",
                            "eerste hondenwacht",
                            "tweede hondenwacht"
                        }
                    },
                    {
                        Constants.WatchSystem.Dutch,
                        new string[]
                        {
                            "eerste wacht",
                            "middenwacht",
                            "dagwacht",
                            "voormiddagwacht",
                            "achtermiddagwacht",
                            "platvoetwacht"
                        }
                    }
                }
            }
        };

        public static Dictionary<Constants.Locale, string[]> NotificationText = new Dictionary<Constants.Locale, string[]>()
        {
            {
                Constants.Locale.en_gb,
                new string[]
                {
                    "First bell of the ",
                    "Second bell of the ",
                    "Third bell of the ",
                    "Fourth bell of the ",
                    "Fifth bell of the ",
                    "Sixth bell of the ",
                    "Seventh bell of the ",
                    "Eighth bell of the "
                }
            },
            {
                Constants.Locale.nl_nl,
                new string[]
                {
                    "Eerste bel van de ",
                    "Tweede bel van de ",
                    "Derde bel van de ",
                    "Vierde bel van de ",
                    "Vijfde bel van de ",
                    "Zesde bel van de ",
                    "Zevende bel van de ",
                    "Achtste bel van de "
                }
            }
        };

        public static Dictionary<Constants.Locale, string[]> TooltipText = new Dictionary<Constants.Locale, string[]>()
        {
            {
                Constants.Locale.en_gb,
                new string[]
                {
                    ", first bell",
                    ", second bell",
                    ", third bell",
                    ", fourth bell",
                    ", fifth bell",
                    ", sixth bell",
                    ", seventh bell",
                    ", eighth bell"
                }
            },
            {
                Constants.Locale.nl_nl,
                new string[]
                {
                    ", eerste bel",
                    ", tweede bel",
                    ", derde bel",
                    ", vierde bel",
                    ", vijfde bel",
                    ", zesde bel",
                    ", zevende bel",
                    ", achtste bel"
                }
            }
        };

        public class ContextMenuText
        {
            public static Dictionary<Constants.Locale, string> Mute = new Dictionary<Constants.Locale, string>()
            {
                { Constants.Locale.en_gb, "Mute" },
                { Constants.Locale.nl_nl, "Zet geluid uit" }
            };

            public static Dictionary<Constants.Locale, string> Notifications = new Dictionary<Constants.Locale, string>()
            {
                { Constants.Locale.en_gb, "Enable notifications" },
                { Constants.Locale.nl_nl, "Zet notificaties aan" }
            };

            public static Dictionary<Constants.Locale, string> Language = new Dictionary<Constants.Locale, string>()
            {
                { Constants.Locale.en_gb, "Language" },
                { Constants.Locale.nl_nl, "Taal" }
            };

            public static Dictionary<Constants.Locale, string> WatchSystem = new Dictionary<Constants.Locale, string>()
            {
                { Constants.Locale.en_gb, "Watch system" },
                { Constants.Locale.nl_nl, "Wachtsysteem" }
            };

            public static Dictionary<Constants.Locale, string> Exit = new Dictionary<Constants.Locale, string>()
            {
                { Constants.Locale.en_gb, "Exit" },
                { Constants.Locale.nl_nl, "Afsluiten" }
            };

            public static Dictionary<Constants.Locale, string[]> Languages = new Dictionary<Constants.Locale, string[]>()
            {
                {
                    Constants.Locale.en_gb,
                    new string[]
                    {
                        "English (English)",
                        "Nederlands (Dutch)"
                    }
                },
                {
                    Constants.Locale.nl_nl,
                    new string[]
                    {
                        "English (Engels)",
                        "Nederlands (Nederlands)"
                    }
                }
            };

            public static Dictionary<Constants.Locale, string[]> WatchSystems = new Dictionary<Constants.Locale, string[]>()
            {
                {
                    Constants.Locale.en_gb,
                    new string[]
                    {
                        "Traditional",
                        "Dutch"
                    }
                },
                {
                    Constants.Locale.nl_nl,
                    new string[]
                    {
                        "Traditioneel",
                        "Nederlands"
                    }
                }
            };
        };
    }
}
