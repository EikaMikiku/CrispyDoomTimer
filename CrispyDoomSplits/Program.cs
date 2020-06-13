using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace CrispyDoomSplits {
    class Program {
        static void Main(string[] args) {
            var WIDTH = 300u;
            var HEIGHT = 300u;
            var wnd = new RenderWindow(new VideoMode(WIDTH, HEIGHT), "Hey");
            wnd.Closed += new EventHandler((o, e) => ((RenderWindow)o).Close());
            while(wnd.IsOpen) {
                wnd.DispatchEvents();
                wnd.Clear(Color.Black);
                wnd.Display();
            }
        }
    }
}
