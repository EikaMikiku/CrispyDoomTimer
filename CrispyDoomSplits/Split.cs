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
        public string Offset = null;

        public Split(string data) {
            string[] splitData = data.Split('\t');
            Label = splitData[1];
            SplitTime = splitData[2];
        }

        public int GetSplitSeconds() {
            var s = SplitTime.Split(':');
            var min = Int32.Parse(s[0]);
            var sec = Int32.Parse(s[1]);

            return min * 60 + sec;
        }

        public int GetRunSeconds() {
            var s = RunTime.Split(':');
            var min = Int32.Parse(s[0]);
            var sec = Int32.Parse(s[1]);

            return min * 60 + sec;
        }

        public void SetRunTime(int leveltime) {
            int mins = leveltime / (60 * 35);
            int secs = (leveltime % (60 * 35)) / 35;
            RunTime = mins.ToString().PadLeft(2, '0') + ":" + secs.ToString().PadLeft(2, '0');
        }
    }
}
