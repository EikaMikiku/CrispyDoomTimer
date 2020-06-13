using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CrispyDoomSplits {
    class ProcessReader {
        const int PROCESS_WM_READ = 0x0010;
        const int MAP_ID = 0x18FEC4;
        const int LEVEL_TIME = 0x1A1070;
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private IntPtr? ProcessHandle = null;
        private IntPtr? BaseAddress = null;
        private Process Process;

        public bool Ok = false;

        public ProcessReader() {
            //Open process
            try {
                Process = Process.GetProcessesByName("crispy-doom")[0];
                BaseAddress = Process.MainModule.BaseAddress;
                ProcessHandle = OpenProcess(PROCESS_WM_READ, false, Process.Id);
                Ok = true;
            } catch {
                //KEEP AT IT!
            }
        }
        
        public int ReadMapId() {
            if(Process.HasExited) {
                return -1;
            }
            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)ProcessHandle, MAP_ID + BaseAddress.Value.ToInt32(), buffer, buffer.Length, ref bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadLevelTime() {
            if(Process.HasExited) {
                return -1;
            }
            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)ProcessHandle, LEVEL_TIME + BaseAddress.Value.ToInt32(), buffer, buffer.Length, ref bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
