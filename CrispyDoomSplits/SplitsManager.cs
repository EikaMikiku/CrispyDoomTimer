using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrispyDoomSplits {
    class SplitsManager {
        public int CurrentSplit = 0;
        public Text Label;
        public List<Split> Splits = new List<Split>();
        private string Filename = "splits.tsv";

        public SplitsManager() {
            //Load splits from file
            string text = File.ReadAllText(Filename);
            string[] lines = text.Split('\n');
            bool isLabel = true;

            foreach(string line in lines) {
                if(line.StartsWith("#") || line.Trim().Length == 0) {
                    continue; //Skip comments or empty lines
                }
                if(isLabel) {
                    //Label is the first data line
                    Label = new Text(line.Trim(), Settings.Font, Settings.FontSize);
                    Label.Position = new Vector2f(0, 0);
                    isLabel = false;
                } else {
                    Splits.Add(new Split(line.Trim()));
                }
            }
        }

        public void DrawSplits(RenderWindow wnd) {
            int totalSplitTime = 0;
            int totalCurrentTime = 0;

            //Render Label
            Label.FillColor = Color.White;
            wnd.Draw(Label);

            var yOffset = 18f;

            //Header
            Text header = new Text("Level".PadRight(21, ' ') + "Split   Run     Difference", Settings.Font, Settings.FontSize);
            header.Position = new Vector2f(0, yOffset);
            wnd.Draw(header);

            yOffset += 2f;

            //Separator
            Text sep = new Text("".PadRight(47, '_'), Settings.Font, Settings.FontSize);
            sep.Position = new Vector2f(0, yOffset);
            wnd.Draw(sep);

            yOffset += 12f;

            //Splits
            foreach(Split s in Splits) {
                var idx = Splits.IndexOf(s);
                if(idx > CurrentSplit) {
                    //Future split, reset RunTime in case of restarts
                    s.RunTime = "00:00";
                }

                //Drawing label, split time, run time
                var str = ((idx + 1) + ")").PadRight(4, ' ');
                str += s.Label.PadRight(17, ' ');
                str += s.SplitTime + "   " + s.RunTime;// + "   " + GetSecDifference(s);
                Text t = new Text(str, Settings.Font, Settings.FontSize);
                t.Position = new Vector2f(0, yOffset);
                wnd.Draw(t);

                //Drawing colored sec difference
                string d = GetSecDifference(s);
                if(d.Length > 0) {
                    str = "".PadRight(37) + d;
                    if(CurrentSplit == idx) {
                        str += "  <=";
                    }

                    Text diff = new Text(str, Settings.Font, Settings.FontSize);
                    diff.Position = new Vector2f(0, yOffset); //Same position, but padded gives consistent spacing

                    int int_d = Int32.Parse(d);
                    if(str.Contains("-")) {
                        diff.FillColor = Color.Green;
                    } else if(int_d > 10) {
                        diff.FillColor = Color.Red;
                    } else {
                        diff.FillColor = Color.Yellow;
                    }
                    wnd.Draw(diff);
                }

                yOffset += 12f;

                //Adding to totals
                totalSplitTime += s.GetSplitSeconds();
                totalCurrentTime += s.GetRunSeconds();
            }

            //Separator
            sep.Position = new Vector2f(0, yOffset - 10f);
            wnd.Draw(sep);

            //Totals
            Text totals = new Text("Totals:  " + GetTimeFromSeconds(totalSplitTime) + " vs " + GetTimeFromSeconds(totalCurrentTime), Settings.Font, Settings.FontSize * 2);
            totals.Style = (Text.Styles.Underlined);
            totals.Position = new Vector2f(0, yOffset);
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