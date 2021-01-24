using SFML.Graphics;
using System;
using System.IO;

namespace CrispyDoomSplits {
    static class Settings {
        //Defaults
        public static Font Font = new Font("Consolas.ttf");
        public static uint FontSize = 16u;
        public static Color Background = Color.Black;
        public static uint Width = 340;
        public static uint Height = 432;

        public static int LevelTimeAddress = 0x1B1520; //CDv5.10.0
        public static int MapIdAddress = 0x19F11C; //CDv5.10.0
        public static int EndScreenAddress = 0x19F12C; //CDv5.10.0

        public static Color GoodSplitColor = Color.Green;
        public static Color OKSplitColor = Color.Yellow;
        public static Color BadSplitColor = Color.Red;
        public static Color PBSplitColor = Color.Magenta;

        public static int BadSplitWhenOver = 5;

        public static void LoadSettings() {
            string text = File.ReadAllText("config.tsv");
            string[] lines = text.Split('\n');
            foreach(string line in lines) {
                if(line.StartsWith("#") || line.Trim().Length == 0) {
                    continue; //Skip comments or empty lines
                }
                string[] s = line.Split('=');
                string val = s[1].Trim();
                switch(s[0]) {
                    case "WIDTH":
                        Settings.Width = UInt32.Parse(val); break;
                    case "HEIGHT":
                        Settings.Height = UInt32.Parse(val); break;
                    case "FONT":
                        Settings.Font = new Font(val); break;
                    case "FONTSIZE":
                        Settings.FontSize = UInt32.Parse(val); break;
                    case "LEVEL_TIME_ADDRESS":
                        Settings.LevelTimeAddress = Convert.ToInt32(val, 16); break;
                    case "MAP_ID_ADDRESS":
                        Settings.MapIdAddress = Convert.ToInt32(val, 16); break;
                    case "END_SCREEN_ADDRESS":
                        Settings.EndScreenAddress = Convert.ToInt32(val, 16); break;
                    case "GOOD_SPLIT_COLOR":
                        Settings.GoodSplitColor = new Color(Convert.ToUInt32(val, 16)); break;
                    case "OK_SPLIT_COLOR":
                        Settings.OKSplitColor = new Color(Convert.ToUInt32(val, 16)); break;
                    case "BAD_SPLIT_COLOR":
                        Settings.BadSplitColor = new Color(Convert.ToUInt32(val, 16)); break;
                    case "PB_SPLIT_COLOR":
                        Settings.PBSplitColor = new Color(Convert.ToUInt32(val, 16)); break;
                    case "BAD_SPLIT_WHEN_OVER":
                        Settings.BadSplitWhenOver = Int32.Parse(val); break;
                }
            }
        }
    }

}
