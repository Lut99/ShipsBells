using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipsBells
{
    class ShipsBellsContext : ApplicationContext
    {
        private int watch;
        private int n_bells;
        private bool notifications;
        private Constants.Locale language;
        private Constants.WatchSystem watchsystem;

        private NotifyIcon icon;
        private System.Timers.Timer timer;
        private SoundPlayer[] bells;

        public ShipsBellsContext()
        {
            // Set the language & watchsystem to default
            this.language = Constants.Locale.en_gb;
            this.watchsystem = Constants.WatchSystem.Traditional;
            this.notifications = false;

            // Overwrite any settings written
            if (File.Exists(Constants.ConfigPath))
            {
                // Read the file
                string[] lines = File.ReadAllLines(Constants.ConfigPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    // Read up to comment start (#)
                    string line = lines[i];
                    int pos = line.IndexOf('#');
                    if (pos >= 0) { line = line.Substring(0, pos); }
                    if (line.Length == 0) { continue; }

                    // Next, split on a '=' (if there is any)
                    pos = line.IndexOf('=');
                    if (pos == -1) {
                        MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": No '=' present", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    string key = line.Substring(0, pos).ToLower();
                    string value = line.Substring(pos + 1).ToLower();

                    // Strip whitespaces, since we're kind
                    key = Constants.StripWhitespaces(key);
                    value = Constants.StripWhitespaces(value);

                    // Check if the key is valid
                    if (key == "notifications")
                    {
                        if (value == "true" || value == "yes" || value == "1")
                        {
                            this.notifications = true;
                        }
                        else if (value == "false" || value == "no" || value == "0")
                        {
                            this.notifications = false;
                        }
                        else
                        {
                            MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": Invalid value '" + value + "' for key 'notifications'", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                    }
                    else if (key == "locale")
                    {
                        // Read the locale
                        if (Localization.ConfigLocaleMap.ContainsKey(value))
                        {
                            this.language = Localization.ConfigLocaleMap[value];
                        }
                        else
                        {
                            MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": Invalid value '" + value + "' for key 'locale'", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                    }
                    else if (key == "watchsystem")
                    {
                        // Read the watchsystem
                        if (Localization.ConfigWatchSystemMap.ContainsKey(value))
                        {
                            this.watchsystem = Localization.ConfigWatchSystemMap[value];
                        }
                        else
                        {
                            MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": Invalid value '" + value + "' for key 'watchsystem'", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error reading config file\nLine " + i.ToString() + ": Unknown key '" + key + "'", "Ship's Bells - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
            }

            // Initialize the SoundPlayers
            bells = new SoundPlayer[8];
            for (int i = 0; i < 8; i++)
            {
                Stream str;
                switch (i)
                {
                    case 0:
                        str = Properties.Resources._1_bell;
                        break;
                    case 1:
                        str = Properties.Resources._2_bells;
                        break;
                    case 2:
                        str = Properties.Resources._3_bells;
                        break;
                    case 3:
                        str = Properties.Resources._4_bells;
                        break;
                    case 4:
                        str = Properties.Resources._5_bells;
                        break;
                    case 5:
                        str = Properties.Resources._6_bells;
                        break;
                    case 6:
                        str = Properties.Resources._7_bells;
                        break;
                    case 7:
                        str = Properties.Resources._8_bells;
                        break;
                    default:
                        throw new System.ArgumentException("Number of bells has to be within 1 and 8", "n_bells");
                }
                bells[i] = new SoundPlayer(str);
                bells[i].Load();
            }

            // Compute the current watch & bells
            DateTime now = DateTime.Now;
            this.watch = Constants.GetWatchTraditional(this.watchsystem, now);
            this.n_bells = (now.Hour % 4 * 2) + (now.Minute >= 30 ? 1 : 0);
            if (this.n_bells == 0) { this.n_bells = 8; }

            // Initialize the notifyicon
            this.icon = new NotifyIcon();
            this.icon.Visible = true;
            this.icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            // Set the language of the menu
            this.icon.ContextMenuStrip = new ContextMenuStrip();
            this.SetLanguage();

            // Launch the timer with a specific time in offset
            this.timer = new System.Timers.Timer();
            this.timer.Interval = (new DateTime(now.Year, now.Month, now.Day, now.Minute < 30 ? now.Hour : now.Hour + 1, now.Minute < 30 ? 30 : 0, 0) - now).TotalMilliseconds;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(ShipsBellsContext_Elapsed);

            // Start the timer
            this.timer.Start();
        }

        public void ShipsBellsContext_Elapsed(Object obj, System.Timers.ElapsedEventArgs e)
        {
            // Reset the timer to match the passing of seconds
            this.timer.Interval = 30 * 60 * 1000;

            // Compute how many bells
            if (++this.n_bells == 9) { this.n_bells = 1; }

            // Compute which watch
            DateTime now = DateTime.Now;
            this.watch = Constants.GetWatchTraditional(this.watchsystem, now);

            // Next, ring the bells
            this.bells[this.n_bells - 1].Play();

            // Optionally show a notification if needed
            if (((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[0]).Checked)
            {
                string text = Localization.NotificationText[this.language][this.n_bells - 1] + Localization.WatchText[this.language][this.watchsystem][this.watch];
                this.icon.ShowBalloonTip(Constants.BellDurations[this.n_bells - 1], "Ding-Ding!", text, ToolTipIcon.None);
            }

            // Update the notifcation icon's tooltip
            this.icon.Text = Constants.Capitalize(Localization.WatchText[this.language][this.watchsystem][this.watch]) + Localization.TooltipText[this.language][this.n_bells - 1];
        }

        public void ShipsBellsContext_ToggleNotifications(Object obj, EventArgs e)
        {
            this.notifications = !this.notifications;
            ((ToolStripMenuItem) this.icon.ContextMenuStrip.Items[0]).Checked = this.notifications;

            // Update the configuration file
            if (File.Exists(Constants.ConfigPath))
            {
                Constants.InsertIntoConfig("notifications", this.notifications.ToString());
            }
            else
            {
                // Create the file, filling in the current settings
                string file = Properties.Resources.config;
                file = file.Replace("%%%NOTIFICATION_VALUE%%%", this.notifications.ToString());
                file = file.Replace("%%%LOCALE_VALUE%%%", Localization.LocaleConfigMap[this.language]);
                file = file.Replace("%%%WATCHSYSTEM_VALUE%%%", Localization.WatchSystemConfigMap[this.watchsystem]);
                File.WriteAllText(Constants.ConfigPath, file);
            }
        }

        public void ShipsBellsContext_ChangeLanguage(Object obj, EventArgs e)
        {
            // Find which of the languages is, well, selected
            for (int i = 0; i < ((ToolStripMenuItem) this.icon.ContextMenuStrip.Items[1]).DropDownItems.Count; i++)
            {
                if (obj.ToString() == ((ToolStripMenuItem) ((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[1]).DropDownItems[i]).ToString())
                {
                    // Select this language
                    this.language = (Constants.Locale) i;
                }
            }

            // Actually propagate the change
            this.SetLanguage();

            // Update the configuration file
            if (File.Exists(Constants.ConfigPath))
            {
                Constants.InsertIntoConfig("locale", this.notifications.ToString());
            }
            else
            {
                // Create the file, filling in the current settings
                string file = Properties.Resources.config;
                file = file.Replace("%%%NOTIFICATION_VALUE%%%", this.notifications.ToString());
                file = file.Replace("%%%LOCALE_VALUE%%%", Localization.LocaleConfigMap[this.language]);
                file = file.Replace("%%%WATCHSYSTEM_VALUE%%%", Localization.WatchSystemConfigMap[this.watchsystem]);
                File.WriteAllText(Constants.ConfigPath, file);
            }
        }

        public void ShipsBellsContext_ChangeWatchSystem(Object obj, EventArgs e)
        {
            // Find which of the languages is, well, selected
            for (int i = 0; i < ((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems.Count; i++)
            {
                if (obj.ToString() == ((ToolStripMenuItem)((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems[i]).ToString())
                {
                    // Select this watchsystem
                    this.watchsystem = (Constants.WatchSystem) i;
                    ((ToolStripMenuItem)((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems[i]).Checked = true;
                }
                else
                {
                    ((ToolStripMenuItem)((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems[i]).Checked = false;
                }
            }

            // Rewrite the thingy
            this.icon.Text = Constants.Capitalize(Localization.WatchText[this.language][this.watchsystem][this.watch]) + Localization.TooltipText[this.language][this.n_bells - 1];

            // Update the configuration file
            if (File.Exists(Constants.ConfigPath))
            {
                Constants.InsertIntoConfig("watchsystem", this.notifications.ToString());
            }
            else
            {
                // Create the file, filling in the current settings
                string file = Properties.Resources.config;
                file = file.Replace("%%%NOTIFICATION_VALUE%%%", this.notifications.ToString());
                file = file.Replace("%%%LOCALE_VALUE%%%", Localization.LocaleConfigMap[this.language]);
                file = file.Replace("%%%WATCHSYSTEM_VALUE%%%", Localization.WatchSystemConfigMap[this.watchsystem]);
                File.WriteAllText(Constants.ConfigPath, file);
            }
        }

        public void ShipsBellsContext_Exit(Object obj, EventArgs e)
        {
            Application.Exit();
        }

        void SetLanguage()
        {
            // Create the contextmenustrip
            this.icon.ContextMenuStrip.Items.Clear();
            this.icon.ContextMenuStrip.Items.Add(Localization.ContextMenuText.Notifications[this.language], null, ShipsBellsContext_ToggleNotifications);
            ((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[0]).Checked = this.notifications;
            this.icon.ContextMenuStrip.Items.Add(Localization.ContextMenuText.Language[this.language], null, null);
            for (int i = 0; i < Enum.GetNames(typeof(Constants.Locale)).Length; i++)
            {
                ((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[1]).DropDownItems.Add(Localization.ContextMenuText.Languages[this.language][i], null, ShipsBellsContext_ChangeLanguage);
                ((ToolStripMenuItem)((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[1]).DropDownItems[i]).Checked = i == (int) this.language;
            }
            this.icon.ContextMenuStrip.Items.Add(Localization.ContextMenuText.WatchSystem[this.language], null, null);
            for (int i = 0; i < Enum.GetNames(typeof(Constants.WatchSystem)).Length; i++)
            {
                ((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems.Add(Localization.ContextMenuText.WatchSystems[this.language][i], null, ShipsBellsContext_ChangeWatchSystem);
                ((ToolStripMenuItem)((ToolStripMenuItem)this.icon.ContextMenuStrip.Items[2]).DropDownItems[i]).Checked = i == (int)this.watchsystem;
            }
            this.icon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.icon.ContextMenuStrip.Items.Add(Localization.ContextMenuText.Exit[this.language], null, ShipsBellsContext_Exit);

            // Rewrite the icon text
            this.icon.Text = Constants.Capitalize(Localization.WatchText[this.language][this.watchsystem][this.watch]) + Localization.TooltipText[this.language][this.n_bells - 1];
        }
    }
}
