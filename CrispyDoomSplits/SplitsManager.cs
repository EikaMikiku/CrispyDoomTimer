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

        public void DrawSplits(RenderWindow wnd) {
            int totalSplitTime = 0;
            int totalCurrentTime = 0;

            const int COL1 = 19;
            const int ROWHEIGHT = 13;
            const int SEPOFFSET = 1;
            const int AFTERSEPOFFSET = 3;

            var yOffset = 0;

            //Header
            Text header = new Text("Level".PadRight(COL1) + "Split  Run    Delta", Settings.Font, Settings.FontSize);
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

                //Drawing label, split time, run time
                var str = s.Label.PadRight(COL1) + s.SplitTime;
                if(idx <= CurrentSplit && CurrentSplit >= 0) {
                    //Show run time on past and current splits
                    str += "  " + s.RunTime;
                }
                Text t = new Text(str, Settings.Font, Settings.FontSize);
                t.Position = new Vector2f(0, yOffset);
                wnd.Draw(t);

                //Drawing colored sec difference
                string d = GetSecDifference(s);
                if(d.Length > 0) {
                    str = "".PadRight(COL1 + 14) + d;

                    Text diff = new Text(str, Settings.Font, Settings.FontSize);
                    diff.Position = new Vector2f(0, yOffset); //Same position, but padded gives consistent spacing

                    int int_d = Int32.Parse(d);
                    if(str.Contains("-")) {
                        diff.FillColor = Color.Green;
                    } else if(int_d > 5) {
                        diff.FillColor = Color.Red;
                    } else {
                        diff.FillColor = Color.Yellow;
                    }
                    wnd.Draw(diff);
                }

                yOffset += ROWHEIGHT;

                //Adding to totals
                totalSplitTime += s.GetSplitSeconds();
                totalCurrentTime += s.GetRunSeconds();
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
                string accum = GetAccumDelta();
                Text totalAccum = new Text("".PadRight(COL1 + 14) + accum, Settings.Font, Settings.FontSize);
                int int_d = Int32.Parse(accum);
                if(accum.Contains("-")) {
                    totalAccum.FillColor = Color.Green;
                } else if(int_d > 5) {
                    totalAccum.FillColor = Color.Red;
                } else {
                    totalAccum.FillColor = Color.Yellow;
                }
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
            if(split.GetRunSeconds() == 0) {
                return "";
            }
            var diff = split.GetRunSeconds() - split.GetSplitSeconds();
            return (diff > -1 ? "+" + diff : diff.ToString());
        }

        public void ResetSplits() {
            foreach(Split s in Splits) {
                s.RunTime = "00:00";
            }
            CurrentSplit = 0;
        }
    }
}