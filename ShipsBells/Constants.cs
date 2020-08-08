using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ShipsBells
{
    class Constants
    {
        public static string Capitalize(string text)
        {
            // Return the first char capitalized
            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        public static string StripWhitespaces(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ' && text[i] != '\t' && text[i] != '\r' && text[i] != '\n')
                {
                    result += text[i];
                }
            }
            return result;
        }

        public static void InsertIntoConfig(string key, string value)
        {
            // Loop through the lines to find the correct one
            string[] lines = File.ReadAllLines(ConfigPath);
            for (int i = 0; i < lines.Length; i++)
            {
                // Dodge empty lines and comments
                string line = lines[i];
                int pos = line.IndexOf('#');
                if (pos >= 0) { line = line.Substring(0, pos); }
                if (line.Length == 0) { continue; }

                // Next, split on a '=' (if there is any)
                pos = line.IndexOf('=');
                if (pos == -1)
                {
                    MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": No '=' present", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                string file_key = line.Substring(0, pos).ToLower();

                // Strip whitespaces, since we're kind
                file_key = Constants.StripWhitespaces(file_key);

                // If the key is correct, update it with the value
                if (file_key == key)
                {
                    lines[i] = key + " = " + value;
                    break;
                }
            }

            // Write everything back again
            File.WriteAllLines(ConfigPath, lines);
        }

        public static int GetWatchTraditional(WatchSystem system, DateTime time)
        {
            for (int i = 0; i < WatchSystemTimes[system].Length; i++)
            {
                if (time.Hour >= WatchSystemTimes[system][i][0] && time.Hour < WatchSystemTimes[system][i][1])
                {
                    return i;
                }
            }
            throw new System.ArgumentException("Time does not allow for any watch to exist, which is weird.", "Current time");
        }

        public enum Locale
        {
            en_gb,
            nl_nl
        }

        public enum WatchSystem
        {
            Traditional,
            Dutch
        }

        public static Dictionary<WatchSystem, int[][]> WatchSystemTimes = new Dictionary<WatchSystem, int[][]>()
        {
            {
                WatchSystem.Traditional,
                new int[][]
                {
                    new int[] { 20, 24 },
                    new int[] { 0, 4 },
                    new int[] { 4, 8 },
                    new int[] { 8, 12 },
                    new int[] { 12, 16 },
                    new int[] { 16, 18 },
                    new int[] { 18, 20 }
                }
            },
            {
                WatchSystem.Dutch,
                new int[][]
                {
                    new int[] { 20, 24 },
                    new int[] { 0, 4 },
                    new int[] { 4, 8 },
                    new int[] { 8, 12 },
                    new int[] { 12, 16 },
                    new int[] { 16, 20 }
                }
            }
        };

        public static int[] BellDurations = new int[]
        {
            2000,
            3000,
            4000,
            4500,
            5500,
            6000,
            7000,
            7500
        };

        public static string ConfigPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ship's Bells/config.txt");
    }
}
