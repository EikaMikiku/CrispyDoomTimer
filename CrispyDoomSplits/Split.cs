using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrispyDoomSplits {
    class Split {
        public string Label = "";
        public string SplitTime = null;
        public string RunTime = "00:00";
        public string PersonalBestTime = "00:00.00";
        public int RunTicks = 0;
        public string Offset = null;
        public bool IsPersonalBest = false;

        public Split(string data) {
            string[] splitData = data.Split('\t');
            Label = splitData[0].Trim();
            SplitTime = splitData[1].Trim();
            if(splitData.Length == 3) {
                PersonalBestTime = splitData[2].Trim();
            }
        }

        public int GetSplitSeconds() {
            var s = SplitTime.Split(':');
            var min = Int32.Parse(s[0]);
            var sec = Int32.Parse(s[1]);

            return min * 60 + sec;
        }

        public int GetRunSeconds() {
            var min = Int32.Parse(RunTime.Substring(0, 2));
            var sec = Int32.Parse(RunTime.Substring(3, 2));

            return min * 60 + sec;
        }

        public string GetFullRunTime() {
            int mins = RunTicks / (60 * 35);
            int secs = (RunTicks % (60 * 35)) / 35;
            int millis = (int)Math.Round(100 * (RunTicks - (mins * 60 * 35) - (secs * 35)) / 35.0);

            return mins.ToString().PadLeft(2, '0') + ":" +
                secs.ToString().PadLeft(2, '0') + "." + 
                millis.ToString().PadLeft(2, '0');
        }

        public void SetRunTime(int leveltime) {
            int mins = leveltime / (60 * 35);
            int secs = (leveltime % (60 * 35)) / 35;
            RunTime = mins.ToString().PadLeft(2, '0') + ":" + secs.ToString().PadLeft(2, '0');

            RunTicks = leveltime;

            //Is it better than personal best?
            if(String.Compare(PersonalBestTime, GetFullRunTime()) == 1) {
                IsPersonalBest = true;
            } else {
                IsPersonalBest = false;
            }
        }
    }
}
