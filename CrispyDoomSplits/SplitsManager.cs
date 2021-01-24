using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrispyDoomSplits {
    class SplitsManager {
        public int CurrentSplit = 0;
        public List<Split> Splits = new List<Split>();
        private string Filename = "splits.tsv";

        public SplitsManager() {
            //Load splits from file
            string text = File.ReadAllText(Filename);
            string[] lines = text.Split('\n');

            foreach(string line in lines) {
                if(line.StartsWith("#") || line.Trim().Length == 0) {
                    continue; //Skip comments or empty lines
                }
                Splits.Add(new Split(line.Trim()));
            }
        }

        public void DrawSplits(RenderWindow wnd, bool isEndScreen) {
            //Resets split times when quick restarting
            if(CurrentSplit == 0) {
                for(var i = 1; i < Splits.Count; i++) {
                    Splits[i].RunTime = "00:00";
                    Splits[i].RunTicks = 0;
                }
            }
            int totalSplitTime = 0;
            int totalCurrentTime = 0;

            const int COL1 = 19;
            const int ROWHEIGHT = 13;
            const int SEPOFFSET = 1;
            const int AFTERSEPOFFSET = 3;

            var yOffset = 0;

            //Header
            Text header = new Text("Level".PadRight(COL1) + "Split  Run       Delta", Settings.Font, Settings.FontSize);
            header.Position = new Vector2f(0, yOffset);
            wnd.Draw(header);

            yOffset += SEPOFFSET;

            //Separator
            Text sep = new Text("".PadRight(150, '_'), Settings.Font, Settings.FontSize);
            sep.Position = new Vector2f(-50, yOffset);
            wnd.Draw(sep);

            yOffset += ROWHEIGHT + AFTERSEPOFFSET;

            //Splits
            foreach(Split s in Splits) {
                var idx = Splits.IndexOf(s);

                //Drawing label, split time
                var str = s.Label.PadRight(COL1) + s.SplitTime;

                Text t = new Text(str, Settings.Font, Settings.FontSize);
                t.Position = new Vector2f(0, yOffset);
                wnd.Draw(t);

                //Drawing run time
                if(idx <= CurrentSplit && CurrentSplit >= 0 && s.RunTicks != 0) {
                    //Show run time on past and current splits
                    str = "".PadRight(COL1 + 7) + s.GetFullRunTime();
                    Text trun = new Text(str, Settings.Font, Settings.FontSize);
                    trun.Position = new Vector2f(0, yOffset);
                    if(s.IsPersonalBest && (isEndScreen || CurrentSplit > idx)) {
                        trun.FillColor = Settings.PBSplitColor;
                    }
                    wnd.Draw(trun);
                }

                //Drawing colored sec difference
                string d = GetSecDifference(s);
                if(d.Length > 0) {
                    str = "".PadRight(COL1 + 17) + d;

                    Text diff = new Text(str, Settings.Font, Settings.FontSize);
                    diff.Position = new Vector2f(0, yOffset); //Same position, but padded gives consistent spacing
                    diff.FillColor = GetDeltaColor(Int32.Parse(d));
                    wnd.Draw(diff);
                }

                yOffset += ROWHEIGHT;

                //Adding to totals
                totalSplitTime += s.GetSplitSeconds();
                if(s.RunTicks != 0) {
                    totalCurrentTime += s.GetRunSeconds();
                }
            }

            //Separator
            yOffset += SEPOFFSET - ROWHEIGHT;
            sep.Position = new Vector2f(-50, yOffset);
            wnd.Draw(sep);

            //Totals
            yOffset += ROWHEIGHT;
            string tstr = "".PadRight(COL1) + GetTimeFromSeconds(totalSplitTime);
            if(CurrentSplit >= 0) {
                tstr += "  " + GetTimeFromSeconds(totalCurrentTime);
                string accum = GetAccumDelta(isEndScreen);
                Text totalAccum = new Text("".PadRight(COL1 + 17) + accum, Settings.Font, Settings.FontSize);
                totalAccum.FillColor = GetDeltaColor(Int32.Parse(accum));
                totalAccum.Position = new Vector2f(0, yOffset + AFTERSEPOFFSET);
                wnd.Draw(totalAccum);
            }
            Text totals = new Text(tstr, Settings.Font, Settings.FontSize);
            totals.Position = new Vector2f(0, yOffset + AFTERSEPOFFSET);
            wnd.Draw(totals);

        }

        private string GetTimeFromSeconds(int sec) {
            int min = sec / 60;
            sec = sec - (min * 60);

            return min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0');
        }

        private string GetSecDifference(Split split) {
            if(split.GetRunSeconds() == 0 || Splits.IndexOf(split) > CurrentSplit) {
                return "";
            }
            var diff = split.GetRunSeconds() - split.GetSplitSeconds();
            return (diff > -1 ? "+" + diff : diff.ToString());
        }

        private string GetAccumDelta(bool isEndScreen) {
            var sum = 0;
            //Include current split if end screen
            var lim = isEndScreen ? CurrentSplit + 1 : CurrentSplit;

            for(var i = 0; i < lim; i++) {
                var diff = GetSecDifference(Splits[i]);
                if(diff.Length > 0) {
                    var idiff = Int32.Parse(diff);
                    sum += idiff;
                }
            }

            return (sum > -1 ? "+" + sum : sum.ToString());
        }

        private Color GetDeltaColor(int delta) {
            if(delta > Settings.BadSplitWhenOver) {
                return Settings.BadSplitColor;
            } else if(delta < 0) {
                return Settings.GoodSplitColor;
            }

            return Settings.OKSplitColor;
        }
    }
}