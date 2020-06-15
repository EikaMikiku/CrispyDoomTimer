using SFML.Graphics;

namespace CrispyDoomSplits {
    static class Settings {
        //Defaults
        public static Font Font = new Font("ProggyCleanSZ.ttf");
        public static uint FontSize = 16u;
        public static Color Background = Color.Black;
        public static uint Width = 340;
        public static uint Height = 432;
        public static int LevelTimeAddress = 0x193090; //CDv5.8.0
        public static int MapIdAddress = 0x181EE4; //CDv5.8.0
    }
}
