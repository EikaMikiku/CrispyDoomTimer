using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace CrispyDoomSplits {
    class Program {
        static void Main(string[] args) {
            var showWaiting = true;
            var wnd = new RenderWindow(new VideoMode(Settings.Width, Settings.Height), "CrispyDoom Timer By Eika");

            wnd.Closed += OnClose;
            wnd.Resized += OnResize;
            OnResize(wnd, null);

            var sm = new SplitsManager();
            var pm = new ProcessReader();

            while(wnd.IsOpen) {
                wnd.DispatchEvents();
                wnd.Clear(Settings.Background);

                if(!pm.Ok) {
                    pm = new ProcessReader(); //Try again
                    if(showWaiting) {
                        Text waiting = new Text("Waiting for\nCrispy Doom\nTo open.", Settings.Font, Settings.FontSize * 2);
                        wnd.Draw(waiting);
                    }
                } else {
                    showWaiting = false; //Only show waiting first time we load things, no need to show it every reset
                    
                    int mapId = pm.ReadMapId();
                    int leveltime = pm.ReadLevelTime();

                    if(mapId > 0) {
                        sm.CurrentSplit = mapId - 1;
                        sm.Splits[sm.CurrentSplit].SetRunTime(leveltime);
                    } else if(mapId < 0) {
                        //Means process exited
                        sm.ResetSplits();
                        pm.Ok = false; //Reload process reader
                    }
                }
                if(pm.Ok || !showWaiting) {
                    sm.DrawSplits(wnd);
                }
                wnd.Display();
            }
        }

        static void OnClose(object sender, EventArgs e) {
            RenderWindow wnd = (RenderWindow)sender;
            wnd.Close();
        }

        static void OnResize(object sender, EventArgs e) {
            //Reset view to keep up with resizes
            RenderWindow wnd = (RenderWindow)sender;
            wnd.SetView(
                new View(
                    new Vector2f(wnd.Size.X / 2f - 5, wnd.Size.Y / 2f),
                    new Vector2f(wnd.Size.X, wnd.Size.Y)
                )
            );
        }
    }
}
