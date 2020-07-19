using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace KartoffelReminder
{
    public partial class GUI2 : Form
    {
        Rectangle resolution = Screen.PrimaryScreen.Bounds;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]

        internal static extern short GetAsyncKeyState(Keys vKey);
        
        public GUI2()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            Thread Start = new Thread(Worker1) { IsBackground = true };
            Start.Start();
            Thread Start2 = new Thread(Worker2) { IsBackground = true };
            Start2.Start();
            Thread Start3 = new Thread(Monitor) { IsBackground = true };
            Start3.Start();
            Globals.spX = resolution.Width / 2 - 3;
            Globals.spY = resolution.Height / 2 - 2;
            Globals.pX = resolution.Width / 2 - 1;
            Globals.pY = resolution.Height / 2 + 12;
            xInput.Maximum = resolution.Width;
            yInput.Maximum = resolution.Height;
            xInput.Value = resolution.Width / 2 - 1;
            yInput.Value = resolution.Height / 2 + 12;
        }

        private void Monitor()
        {
            while (true)
            {
                while (GetAsyncKeyState(Keys.LButton) < 0
                       && Globals.rf == 1
                       && Globals.pause == 0
                       && Globals.rfHK == 1
                       || GetAsyncKeyState(Keys.XButton1) < 0
                       && Globals.rf == 1
                       && Globals.pause == 0
                       && Globals.rfHK == 2
                       || GetAsyncKeyState(Keys.XButton2) < 0
                       && Globals.rf == 1
                       && Globals.pause == 0
                       && Globals.rfHK == 3
                       || GetAsyncKeyState(Keys.LShiftKey) < 0
                       && Globals.rf == 1
                       && Globals.pause == 0
                       && Globals.rfHK == 4)
                {
                    if (Globals.rButton == 1)
                    {
                        Globals.rButton = 0;
                    }
                    if (Globals.lButton == 0)
                    {
                        Globals.lButton = 1;
                    }
                    if (Globals.Lock == 2)
                    {
                        mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -120, 0);
                        Thread.Sleep(Globals.rfTime);
                    }
                    Globals.Lock = 0;
                    Thread.Sleep(5);
                }
                if (Globals.lButton == 1)
                {
                    Globals.lButton = 0;
                }
                while (GetAsyncKeyState(Keys.RButton) < 0
                       && Globals.ctb == 1
                       && Globals.pause == 0
                       && Globals.tbHK == 1
                       || GetAsyncKeyState(Keys.XButton1) < 0
                       && Globals.ctb == 1
                       && Globals.pause == 0
                       && Globals.tbHK == 2
                       || GetAsyncKeyState(Keys.XButton2) < 0
                       && Globals.ctb == 1
                       && Globals.pause == 0
                       && Globals.tbHK == 3
                       || GetAsyncKeyState(Keys.LShiftKey) < 0
                       && Globals.ctb == 1
                       && Globals.pause == 0
                       && Globals.tbHK == 4)
                {
                    if (GetAsyncKeyState(Keys.RControlKey) < 0)
                    {
                        GrabColor(1);
                    }
                    if (GetAsyncKeyState(Keys.Insert) < 0)
                    {
                        GrabColor(2);
                    }
                    if (GetAsyncKeyState(Keys.LButton) < 0
                        && Globals.rf == 1
                        && Globals.rfHK == 1
                        || GetAsyncKeyState(Keys.XButton1) < 0
                        && Globals.rf == 1
                        && Globals.rfHK == 2
                        || GetAsyncKeyState(Keys.XButton2) < 0
                        && Globals.rf == 1
                        && Globals.rfHK == 3
                        || GetAsyncKeyState(Keys.LShiftKey) < 0
                        && Globals.rf == 1
                        && Globals.rfHK == 4)
                    {
                        break;
                    }
                    if (Globals.rButton == 0)
                    {
                        Globals.rButton = 1;
                    }
                    if (Globals.Lock == 1)
                    {
                        mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -120, 0);
                        Thread.Sleep(Globals.tbTime);
                        Globals.Lock = 0;
                    }
                    Thread.Sleep(5);
                }
                if (GetAsyncKeyState(Keys.RButton) < 0
                    && Globals.ctb == 0
                    && Globals.tbHK == 1
                    || GetAsyncKeyState(Keys.XButton1) < 0
                    && Globals.ctb == 0
                    && Globals.tbHK == 2
                    || GetAsyncKeyState(Keys.XButton2) < 0
                    && Globals.ctb == 0
                    && Globals.tbHK == 3
                    || GetAsyncKeyState(Keys.LShiftKey) < 0
                    && Globals.ctb == 0
                    && Globals.tbHK == 4) 
                {
                    if (GetAsyncKeyState(Keys.RControlKey) < 0)
                    {
                        GrabColor(1);
                    }
                    if (GetAsyncKeyState(Keys.Insert) < 0)
                    {
                        GrabColor(2);
                    }
                }
                if (Globals.rButton == 1)
                {
                    Globals.rButton = 0;
                }
                if (GetAsyncKeyState(Keys.Pause) < 0)
                {
                    Pause();
                }
                if (GetAsyncKeyState(Keys.RControlKey) < 0)
                {
                    GrabColor(1);
                }
                if (GetAsyncKeyState(Keys.Insert) < 0)
                {
                    GrabColor(2);
                }
                if (Globals.monitorCycle < 10)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(Globals.monitorCycle);
                }
            }
        }

        private void Worker1()
        {
            while (true)
            {
                while (Globals.lButton == 1)
                {
                    Globals.rfTime = GetSleepTime(1);
                    Globals.Lock = 2;
                    Thread.Sleep(Globals.rfTime);
                    WorkSleep(1);
                }
                while (Globals.rButton == 1)
                {
                    if (Globals.RDY == 1)
                    {
                        if (Globals.H1 == 0)
                        {
                            Globals.H1 = 1;
                            Thread.Sleep(Globals.HoldT);
                        }
                        if (Globals.Lock == 0)
                        {
                            GrabColor2(1);
                            if (Globals.mode == 1 || Globals.mode == 6 || Globals.mode == 7 || Globals.mode == 8)
                            {
                                if (Globals.minColShade <= Globals.actCol
                                && Globals.maxColShade >= Globals.actCol)
                                {
                                    if (Globals.Lock == 0)
                                    {
                                        Globals.tbTime = GetSleepTime(2);
                                        Globals.Lock = 1;
                                        Thread.Sleep(Globals.tbTime);
                                    }
                                }
                            }
                            else
                            {
                                if (Globals.minColShade <= Globals.actCol
                                && Globals.maxColShade >= Globals.actCol
                                || Globals.minColShade <= Globals.actCol2
                                && Globals.maxColShade >= Globals.actCol2)
                                {
                                    if (Globals.Lock == 0)
                                    {
                                        Globals.tbTime = GetSleepTime(2);
                                        Globals.Lock = 1;
                                        Thread.Sleep(Globals.tbTime);
                                    }
                                }
                            } 
                        }
                    }
                    WorkSleep(1);
                }
                if (Globals.H1 == 1)
                {
                    Globals.H1 = 0;
                }
                WorkSleep(1);
            }
        }

        private void Worker2()
        {
            while (Globals.worker == true)
            {
                while (Globals.rButton == 1)
                {
                    if (Globals.RDY == 1)
                    {
                        if (Globals.H2 == 0)
                        {
                            Globals.H2 = 1;
                            Thread.Sleep(Globals.HoldT);
                        }
                        if (Globals.Lock == 0)
                        {
                            GrabColor2(2);
                            if (Globals.mode == 6 || Globals.mode == 7 || Globals.mode == 8)
                            {
                                if (Globals.minColShade <= Globals.actCol3
                                && Globals.maxColShade >= Globals.actCol3)
                                {
                                    if (Globals.Lock == 0)
                                    {
                                        Globals.tbTime = GetSleepTime(2);
                                        Globals.Lock = 1;
                                        Thread.Sleep(Globals.tbTime);
                                    }
                                }
                            }
                            else
                            {
                                if (Globals.minColShade <= Globals.actCol3
                                && Globals.maxColShade >= Globals.actCol3
                                || Globals.minColShade <= Globals.actCol4
                                && Globals.maxColShade >= Globals.actCol4)
                                {
                                    if (Globals.Lock == 0)
                                    {
                                        Globals.tbTime = GetSleepTime(2);
                                        Globals.Lock = 1;
                                        Thread.Sleep(Globals.tbTime);
                                    }
                                }
                            }  
                        }
                    }
                    WorkSleep(2);
                }
                if (Globals.H2 == 1)
                {
                    Globals.H2 = 0;
                }
                WorkSleep(2);
            }
        }

        static class Globals
        {
            public static Color definedC, definedCD, definedCL, actualC, actualC2;
            public static int RDY, pause, H1, H2, HoldT = 250, Lock, minColShade, maxColShade, defCol, actCol, actCol2, actCol3, actCol4, pX, pY, spX, spY, rfTime, rfTimeMin = 56, rfTimeMax = 62, tbTime, tbTimeMin = 100, tbTimeMax = 110, ctb, rf, inUse, rButton, lButton;
            public static int monitorCycle = 10, worker1Cycle = 10, worker2Cycle = 10, mode = 5, first, rfHK = 1, tbHK = 1;
            public static int[][,] XYarr = { new int[30, 30], new int[30, 30], new int[30, 30], new int[30, 30] };
            public static float subVal = (float)-0.100, addVal = (float)0.100;
            public static bool worker = true;
        }
        static Object WorkSleep(int arg)
        {
            if (arg == 1)
            {
                if (Globals.worker1Cycle < 10)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(Globals.worker1Cycle);
                }
            }
            else
            {
                if (Globals.worker2Cycle < 10)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(Globals.worker2Cycle);
                }
            }
            return 1;
        }
        static int GetSleepTime(int arg)
        {
            var rand = new Random();
            int result = 10;
            if (arg == 1)
            {
                if (Globals.rfTimeMin == Globals.rfTimeMax)
                {
                    result = Globals.rfTimeMin;
                }
                else
                {
                    result = rand.Next(Globals.rfTimeMin, Globals.rfTimeMax);
                }
            }
            else if (arg == 2)
            {
                if (Globals.tbTimeMin == Globals.tbTimeMax)
                {
                    result = Globals.tbTimeMin;
                }
                else
                {
                    result = rand.Next(Globals.tbTimeMin, Globals.tbTimeMax);
                }
            }
            return result;
        }
        static Object Pause()
        {
            if (Globals.pause == 0)
            {
                Globals.pause = 1;
                SystemSounds.Beep.Play();
            }
            else if (Globals.pause == 1)
            {
                Globals.pause = 0;
                SystemSounds.Asterisk.Play();
            }
            Thread.Sleep(500);
            return 1;
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        static Object ColToInt(Color color)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;
            string result = red.ToString() + green.ToString() + blue.ToString();
            return Convert.ToInt32(result);
        }

        public static void WriteToFile(string s, string name)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".save");
            if (Globals.first == 1)
            {
                Globals.first = 0;
                var make = new FileStream(path, FileMode.Create, FileAccess.Write);
                make.Close();
            }
            var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        private void LoadClick(object sender, MouseEventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Filename.Text + ".save");
            try
            {
                StreamReader file = new StreamReader(path);
                string line = file.ReadLine();
                Globals.RDY = Convert.ToInt32(line);
                line = file.ReadLine();
                Globals.pause = Convert.ToInt32(line);
                if (Globals.pause == 2)
                {
                    Globals.pause = 1;
                }
                else if (Globals.pause == 3)
                {
                    Globals.pause = 0;
                }
                line = file.ReadLine();
                Globals.minColShade = Convert.ToInt32(line);
                line = file.ReadLine();
                Globals.maxColShade = Convert.ToInt32(line);
                line = file.ReadLine();
                Globals.pX = Convert.ToInt32(line);
                xInput.Value = Globals.pX;
                line = file.ReadLine();
                Globals.pY = Convert.ToInt32(line);
                yInput.Value = Globals.pY;
                line = file.ReadLine();
                Globals.rfTimeMin = Convert.ToInt32(line);
                rfMinIn.Value = Globals.rfTimeMin;
                line = file.ReadLine();
                Globals.rfTimeMax = Convert.ToInt32(line);
                rfMaxIn.Value = Globals.rfTimeMax;
                line = file.ReadLine();
                Globals.tbTimeMin = Convert.ToInt32(line);
                tbMinIn.Value = Globals.tbTimeMin;
                line = file.ReadLine();
                Globals.tbTimeMax = Convert.ToInt32(line);
                tbMaxIn.Value = Globals.tbTimeMax;
                line = file.ReadLine();
                if (line == "1")
                {
                    if (ctbCheckB.Checked == false)
                    {
                        ctbCheckB.Checked = true;
                    }
                }
                else if (line == "0")
                {
                    if (ctbCheckB.Checked == true)
                    {
                        ctbCheckB.Checked = false;
                    }
                }
                line = file.ReadLine();
                if (line == "1")
                {
                    if (rfCheckB.Checked == false)
                    {
                        rfCheckB.Checked = true;
                    }
                }
                else if (line == "0")
                {
                    if (rfCheckB.Checked == true)
                    {
                        rfCheckB.Checked = false;
                    }
                }
                line = file.ReadLine();
                Globals.monitorCycle = Convert.ToInt32(line);
                MonitorCycleIn.Value = Globals.monitorCycle;
                line = file.ReadLine();
                Globals.worker1Cycle = Convert.ToInt32(line);
                Worker1CycleIn.Value = Globals.worker1Cycle;
                line = file.ReadLine();
                Globals.worker2Cycle = Convert.ToInt32(line);
                Worker2CycleIn.Value = Globals.worker2Cycle;
                line = file.ReadLine();
                Globals.HoldT = Convert.ToInt32(line);
                HoldTIn.Value = Globals.HoldT;
                line = file.ReadLine();
                Globals.mode = Convert.ToInt32(line);
                Setmode(Globals.mode);
                line = file.ReadLine();
                Globals.addVal = (float)Convert.ToDouble(line);
                Globals.subVal = 0.0f - (float)Convert.ToDouble(line);
                SetVal();
                string a = file.ReadLine();
                string r = file.ReadLine();
                string g = file.ReadLine();
                string b = file.ReadLine();
                colorLabel.BackColor = Color.FromArgb(Convert.ToInt32(a), Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
                a = file.ReadLine();
                r = file.ReadLine();
                g = file.ReadLine();
                b = file.ReadLine();
                colorLabelMax.BackColor = Color.FromArgb(Convert.ToInt32(a), Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
                a = file.ReadLine();
                r = file.ReadLine();
                g = file.ReadLine();
                b = file.ReadLine();
                colorLabelMin.BackColor = Color.FromArgb(Convert.ToInt32(a), Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
                int i = 1;
                for (int s = 0; s < 4; s++)
                {
                    for (int l = 0; l < 30; l++)
                    {
                        line = file.ReadLine();
                        Globals.XYarr[s][l, 0] = Convert.ToInt32(line);
                        line = file.ReadLine();
                        Globals.XYarr[s][0, l] = Convert.ToInt32(line);
                        line = file.ReadLine();
                        Match m = Regex.Match(line, @"A=(?<Alpha>\d+),\s*R=(?<Red>\d+),\s*G=(?<Green>\d+),\s*B=(?<Blue>\d+)");
                        if (m.Success)
                        {
                            int alpha = int.Parse(m.Groups["Alpha"].Value);
                            int red = int.Parse(m.Groups["Red"].Value);
                            int green = int.Parse(m.Groups["Green"].Value);
                            int blue = int.Parse(m.Groups["Blue"].Value);
                            Color c = Color.FromArgb(alpha, red, green, blue);
                            Globals.actualC = c;
                        }
                        GuiAccess(false, s, l, i);
                        i++;
                    }
                }
                file.Close();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            SystemSounds.Beep.Play();
        }

        private void SaveClick(object sender, MouseEventArgs e)
        {
            this.Enabled = false;
            if (Globals.pause == 1)
            {
                Globals.pause++;
            }
            else
            {
                Globals.pause = 3;
            }
            Globals.first = 1;
            string text = Globals.RDY.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.pause.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.minColShade.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.maxColShade.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.pX.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.pY.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.rfTimeMin.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.rfTimeMax.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.tbTimeMin.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.tbTimeMax.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.ctb.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.rf.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.monitorCycle.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.worker1Cycle.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.worker2Cycle.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.HoldT.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.mode.ToString();
            WriteToFile(text, Filename.Text);
            text = Globals.addVal.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabel.BackColor.A.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabel.BackColor.R.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabel.BackColor.G.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabel.BackColor.B.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMax.BackColor.A.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMax.BackColor.R.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMax.BackColor.G.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMax.BackColor.B.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMin.BackColor.A.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMin.BackColor.R.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMin.BackColor.G.ToString();
            WriteToFile(text, Filename.Text);
            text = colorLabelMin.BackColor.B.ToString();
            WriteToFile(text, Filename.Text);
            int i = 1;
            for (int r = 0; r < 4; r++)
            {
                for (int l = 0; l < 30; l++)
                {
                    text = Globals.XYarr[r][l, 0].ToString();
                    WriteToFile(text, Filename.Text);
                    text = Globals.XYarr[r][0, l].ToString();
                    WriteToFile(text, Filename.Text);
                    text = GuiAccess(true, r, l, i);
                    WriteToFile(text, Filename.Text);
                    i++;
                }
            }
            if (Globals.pause == 2)
            {
                Globals.pause = 1;
            }
            else
            {
                Globals.pause = 0;
            }
            this.Enabled = true;
            SystemSounds.Beep.Play();
        }
        static Color GetOnePixel(int x, int y)
        {
            Graphics graph;
            Bitmap bm = new Bitmap(1, 1);
            graph = Graphics.FromImage(bm as Image);
            graph.CopyFromScreen(x, y, 0, 0, bm.Size);
            Color result = bm.GetPixel(0, 0);
            bm.Dispose();
            graph.Dispose();
            return result;
        }
        
        Object GrabColor(int arg)
        {
            if (arg == 1 && Globals.inUse == 0)
            {
                Globals.inUse = 1;
                Globals.definedC = GetOnePixel(Globals.pX, Globals.pY);
                Globals.defCol = (int)ColToInt(Globals.definedC);
                colorLabel.BackColor = Globals.definedC;
                Globals.definedCD = ChangeColorBrightness(Globals.definedC, Globals.subVal);
                colorLabelMin.BackColor = Globals.definedCD;
                Globals.minColShade = (int)ColToInt(Globals.definedCD);
                Globals.definedCL = ChangeColorBrightness(Globals.definedC, Globals.addVal);
                colorLabelMax.BackColor = Globals.definedCL;
                Globals.maxColShade = (int)ColToInt(Globals.definedCL);
                SystemSounds.Beep.Play();
                if (Globals.RDY == 0)
                {
                    Globals.RDY = 1;
                }
                Globals.inUse = 0;
                Thread.Sleep(500);
            }
            else if (arg == 2 && Globals.inUse == 0)
            {
                Globals.inUse = 1;
                int x = Globals.spX;
                int y = Globals.spY;
                int i = 0;
                Graphics CrossAreaG;
                Bitmap CrossArea = new Bitmap(4, 30);
                CrossAreaG = Graphics.FromImage(CrossArea as Image);
                CrossAreaG.CopyFromScreen(Globals.spX + 1, Globals.spY + 1, 0, 0, CrossArea.Size);
                for (int r = 0; r < 4; r++)
                {
                    x++;
                    if (y != Globals.spY)
                    {
                        y = Globals.spY;
                    }
                    for (int l = 0; l < 30; l++)
                    {
                        y++;
                        i++;
                        Globals.XYarr[r][l, 0] = x;
                        Globals.XYarr[r][0, l] = y;
                        Globals.actualC = CrossArea.GetPixel(r, l);
                        Globals.actCol = (int)ColToInt(Globals.actualC);
                        _ = (string)Invoke((Func<bool, int, int, int, string>)GuiAccess, false, r, l, i);
                    }
                }
                CrossArea.Dispose();
                CrossAreaG.Dispose();
                SystemSounds.Beep.Play();
                Globals.inUse = 0;
            }
            return 1;
        }
        bool PSaccess(bool b)
        {
            pixelSelectionBox.Enabled = b;
            return b;
        }

        static Object GrabColor2(int arg)
        {
            if (arg == 1 && Globals.inUse == 0)
            {
                Globals.inUse = 1;
                int x = Globals.pX;
                int y = Globals.pY;
                if (Globals.mode == 1 || Globals.mode == 6 || Globals.mode == 7 || Globals.mode == 8)
                {
                    Globals.actualC = GetOnePixel(x, y);
                    Globals.actCol = (int)ColToInt(Globals.actualC);
                    Globals.inUse = 0;
                    return 1;
                }
                else if (Globals.mode == 2 || Globals.mode == 4)
                {
                    Globals.actualC = GetOnePixel(x, y);
                    Globals.actCol = (int)ColToInt(Globals.actualC);
                    y++;
                }
                else if (Globals.mode == 3 || Globals.mode == 5)
                {
                    Globals.actualC = GetOnePixel(x, y);
                    Globals.actCol = (int)ColToInt(Globals.actualC);
                    x++;
                }
                Globals.actualC = GetOnePixel(x, y);
                Globals.actCol2 = (int)ColToInt(Globals.actualC);
                Globals.inUse = 0;
            }
            else if (Globals.inUse == 0)
            {
                Globals.inUse = 1;
                int x = Globals.pX;
                int y = Globals.pY;
                if (Globals.mode == 8)
                {
                    Globals.actualC2 = GetOnePixel(x, y);
                    Globals.actCol3 = (int)ColToInt(Globals.actualC2);
                    Globals.inUse = 0;
                    return 1;
                }
                else if (Globals.mode == 4)
                {
                    y++;
                    Globals.actualC2 = GetOnePixel(x, y);
                    Globals.actCol3 = (int)ColToInt(Globals.actualC2);
                    y++;
                }
                else if (Globals.mode == 5)
                {
                    y++;
                    Globals.actualC2 = GetOnePixel(x, y);
                    Globals.actCol3 = (int)ColToInt(Globals.actualC2);
                    x++;
                }
                else if (Globals.mode == 6)
                {
                    y++;
                    Globals.actualC2 = GetOnePixel(x, y);
                    Globals.actCol3 = (int)ColToInt(Globals.actualC2);
                    Globals.inUse = 0;
                    return 1;
                }
                else if (Globals.mode == 7)
                {
                    x++;
                    Globals.actualC2 = GetOnePixel(x, y);
                    Globals.actCol3 = (int)ColToInt(Globals.actualC2);
                    Globals.inUse = 0;
                    return 1;
                }
                Globals.actualC2 = GetOnePixel(x, y);
                Globals.actCol4 = (int)ColToInt(Globals.actualC2);
                Globals.inUse = 0;
            }
            return 1;

        }

        private void PixelLabel(object sender, MouseEventArgs e)
        {
            LinkLabel me = sender as LinkLabel;
            int r = (int)me.Tag;
            int l = Convert.ToInt32(me.Text);
            xInput.Value = Globals.XYarr[r][l, 0];
            yInput.Value = Globals.XYarr[r][0, l];
        }

        private void Xchange(object sender, EventArgs e)
        {
            Globals.pX = (int)xInput.Value;
        }

        private void Ychange(object sender, EventArgs e)
        {
            Globals.pY = (int)yInput.Value;
        }

        private void GUI_Exit(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void TbMinIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
            }
            else if (me.Value > tbMaxIn.Value)
            {
                tbMaxIn.Value = me.Value;
            }
            Globals.tbTimeMin = (int)me.Value;
        }

        private void TbMaxIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
            }
            else if (me.Value < tbMinIn.Value)
            {
                tbMinIn.Value = me.Value;
            }
            Globals.tbTimeMax = (int)me.Value;
        }

        private void RfMinIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
            }
            else if (me.Value > rfMaxIn.Value)
            {
                rfMaxIn.Value = me.Value;
            }
            Globals.rfTimeMin = (int)me.Value;
        }

        private void RfMaxIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
            }
            else if (me.Value < rfMinIn.Value)
            {
                rfMinIn.Value = me.Value;
            }
            Globals.rfTimeMax = (int)me.Value;
        }

        private void CTBcheckChange(object sender, EventArgs e)
        {
            CheckBox me = sender as CheckBox;
            if (Globals.ctb == 0)
            {
                Globals.ctb = 1;
                me.Text = "CTB - is On";
            }
            else
            {
                Globals.ctb = 0;
                me.Text = "CTB - is Off";
            }
        }

        private void RFcheckChange(object sender, EventArgs e)
        {
            CheckBox me = sender as CheckBox;
            if (Globals.rf == 0)
            {
                Globals.rf = 1;
                me.Text = "RF - is On";
            }
            else
            {
                Globals.rf = 0;
                me.Text = "RF - is Off";
            }
        }

        private void SetVal()
        {
            string s = Globals.addVal.ToString();
            if (s == "0,005")
            {
                VFIn.Value = 0;
            }
            else if (s == "1")
            {
                VFIn.Value = 100;
            }
            else
            {
                string firstThreeChar = new string(s.Take(3).ToArray());
                if (firstThreeChar == "0,0")
                {
                    VFIn.Value = Convert.ToInt32(s.Substring(3, s.Length - 3));
                }
                else
                {
                    VFIn.Value = Convert.ToInt32(s.Substring(2, s.Length - 2));
                    if (VFIn.Value < 10)
                    {
                        VFIn.Value *= 10;
                    }
                }
            }
        }

        private void VarianceFactor(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value == 0)
            {
                Globals.subVal = -0.005f;
                Globals.addVal = 0.005f;
            }
            else if (me.Value < 10)
            {
                string val = "0,0" + me.Value + "0";
                Globals.subVal = 0f - (float)Convert.ToDecimal(val);
                Globals.addVal = (float)Convert.ToDecimal(val);
            }
            else if (me.Value > 9 && me.Value < 100)
            {
                string val = "0," + me.Value + "0";
                Globals.subVal = 0f - (float)Convert.ToDecimal(val);
                Globals.addVal = (float)Convert.ToDecimal(val);
            }
            else if (me.Value > 99)
            {
                Globals.subVal = -1.000f;
                Globals.addVal = 1.000f;
            }
        }

        private void MonitorCycleIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
                Globals.monitorCycle = 10;
            }
            else
            {
                Globals.monitorCycle = (int)me.Value;
            }
        }
        private void Worker1CycleIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
                Globals.worker1Cycle = 10;
            }
            else
            {
                Globals.worker1Cycle = (int)me.Value;
            }
        }

        private void Worker2CycleIn_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            if (me.Value < 10)
            {
                me.Value = 10;
                Globals.worker2Cycle = 10;
            }
            else
            {
                Globals.worker2Cycle = (int)me.Value;
            }
        }

        private void Setmode(int mode)
        {
            TreeView me = ScanModeView as TreeView;
            if (mode == 1)
            {
                me.Nodes[0].Text = "Mode-Single Pixel";
            }
            else if (mode == 2)
            {
                me.Nodes[0].Text = "Mode-ST Vertical";
            }
            else if (mode == 3)
            {
                me.Nodes[0].Text = "Mode-ST Horizontal";
            }
            else if (mode == 4)
            {
                me.Nodes[0].Text = "Mode-DTQP Vertical";
            }
            else if (mode == 5)
            {
                me.Nodes[0].Text = "Mode-DTQP Square";
            }
            else if (mode == 6)
            {
                me.Nodes[0].Text = "Mode-DTDP Vertical";
            }
            else if (mode == 7)
            {
                me.Nodes[0].Text = "Mode-DTDP Horizontal";
            }
            else if (mode == 8)
            {
                me.Nodes[0].Text = "Mode-DT Single Pixel";
            }
            if (Globals.mode < 4)
            {
                if (Globals.worker == true)
                {
                    Globals.worker = false;
                }
            }
            else
            {
                if (Globals.worker == false)
                {
                    Globals.worker = true;
                    Thread Start2 = new Thread(Worker2) { IsBackground = true };
                    Start2.Start();
                }
            }
        }

        private void ScanModeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView me = sender as TreeView;
            int tag = Convert.ToInt32(me.SelectedNode.Tag);
            if (tag == 1)
            {
                if (Globals.mode == 1)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 1;
                    me.Nodes[0].Text = "Mode-Single Pixel";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 2)
            {
                if (Globals.mode == 2)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 2;
                    me.Nodes[0].Text = "Mode-ST Vertical";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 3)
            {
                if (Globals.mode == 3)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 3;
                    me.Nodes[0].Text = "Mode-ST Horizontal";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 4)
            {
                if (Globals.mode == 4)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 4;
                    me.Nodes[0].Text = "Mode-DTQP Vertical";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 5)
            {
                if (Globals.mode == 5)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 5;
                    me.Nodes[0].Text = "Mode-DTQP Square";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 6)
            {
                if (Globals.mode == 6)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 6;
                    me.Nodes[0].Text = "Mode-DTDP Vertical";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 7)
            {
                if (Globals.mode == 7)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 7;
                    me.Nodes[0].Text = "Mode-DTDP Horizontal";
                    SystemSounds.Beep.Play();
                }
            }
            else if (tag == 8)
            {
                if (Globals.mode == 8)
                {
                    _ = MessageBox.Show("This Mode is already active!");
                }
                else
                {
                    Globals.mode = 8;
                    me.Nodes[0].Text = "Mode-DT Single Pixel";
                    SystemSounds.Beep.Play();
                }
            }
            if (Globals.mode < 4)
            {
                if (Globals.worker == true)
                {
                    Globals.worker = false;
                }
            }
            else
            {
                if (Globals.worker == false)
                {
                    Globals.worker = true;
                    Thread Start2 = new Thread(Worker2) { IsBackground = true };
                    Start2.Start();
                }
            }
        }

        private void HoldTIn_Changed(object sender, EventArgs e)
        {
            NumericUpDown me = sender as NumericUpDown;
            Globals.HoldT = (int)me.Value;
        }

        string GuiAccess(bool Save, int r, int l, int i)
        {
            string s = " ";
            if (i == 1)
            {
                PSaccess(false);
                if (Save == true) { s = linkLabel1.BackColor.ToString(); } else { linkLabel1.BackColor = Globals.actualC; linkLabel1.Tag = r; linkLabel1.Text = l.ToString(); }
            }
            else if (i == 2)
            {
                if (Save == true) { s = linkLabel2.BackColor.ToString(); } else { linkLabel2.BackColor = Globals.actualC; linkLabel2.Tag = r; linkLabel2.Text = l.ToString(); }
            }
            else if (i == 3)
            {
                if (Save == true) { s = linkLabel3.BackColor.ToString(); } else { linkLabel3.BackColor = Globals.actualC; linkLabel3.Tag = r; linkLabel3.Text = l.ToString(); }
            }
            else if (i == 4)
            {
                if (Save == true) { s = linkLabel4.BackColor.ToString(); } else { linkLabel4.BackColor = Globals.actualC; linkLabel4.Tag = r; linkLabel4.Text = l.ToString(); }
            }
            else if (i == 5)
            {
                if (Save == true) { s = linkLabel5.BackColor.ToString(); } else { linkLabel5.BackColor = Globals.actualC; linkLabel5.Tag = r; linkLabel5.Text = l.ToString(); }
            }
            else if (i == 6)
            {
                if (Save == true) { s = linkLabel6.BackColor.ToString(); } else { linkLabel6.BackColor = Globals.actualC; linkLabel6.Tag = r; linkLabel6.Text = l.ToString(); }
            }
            else if (i == 7)
            {
                if (Save == true) { s = linkLabel7.BackColor.ToString(); } else { linkLabel7.BackColor = Globals.actualC; linkLabel7.Tag = r; linkLabel7.Text = l.ToString(); }
            }
            else if (i == 8)
            {
                if (Save == true) { s = linkLabel8.BackColor.ToString(); } else { linkLabel8.BackColor = Globals.actualC; linkLabel8.Tag = r; linkLabel8.Text = l.ToString(); }
            }
            else if (i == 9)
            {
                if (Save == true) { s = linkLabel9.BackColor.ToString(); } else { linkLabel9.BackColor = Globals.actualC; linkLabel9.Tag = r; linkLabel9.Text = l.ToString(); }
            }
            else if (i == 10)
            {

                if (Save == true) { s = linkLabel10.BackColor.ToString(); } else { linkLabel10.BackColor = Globals.actualC; linkLabel10.Tag = r; linkLabel10.Text = l.ToString(); }
            }
            else if (i == 11)
            {

                if (Save == true) { s = linkLabel11.BackColor.ToString(); } else { linkLabel11.BackColor = Globals.actualC; linkLabel11.Tag = r; linkLabel11.Text = l.ToString(); }
            }
            else if (i == 12)
            {

                if (Save == true) { s = linkLabel12.BackColor.ToString(); } else { linkLabel12.BackColor = Globals.actualC; linkLabel12.Tag = r; linkLabel12.Text = l.ToString(); }
            }
            else if (i == 13)
            {

                if (Save == true) { s = linkLabel13.BackColor.ToString(); } else { linkLabel13.BackColor = Globals.actualC; linkLabel13.Tag = r; linkLabel13.Text = l.ToString(); }
            }
            else if (i == 14)
            {

                if (Save == true) { s = linkLabel14.BackColor.ToString(); } else { linkLabel14.BackColor = Globals.actualC; linkLabel14.Tag = r; linkLabel14.Text = l.ToString(); }
            }
            else if (i == 15)
            {

                if (Save == true) { s = linkLabel15.BackColor.ToString(); } else { linkLabel15.BackColor = Globals.actualC; linkLabel15.Tag = r; linkLabel15.Text = l.ToString(); }
            }
            else if (i == 16)
            {

                if (Save == true) { s = linkLabel16.BackColor.ToString(); } else { linkLabel16.BackColor = Globals.actualC; linkLabel16.Tag = r; linkLabel16.Text = l.ToString(); }
            }
            else if (i == 17)
            {

                if (Save == true) { s = linkLabel17.BackColor.ToString(); } else { linkLabel17.BackColor = Globals.actualC; linkLabel17.Tag = r; linkLabel17.Text = l.ToString(); }
            }
            else if (i == 18)
            {

                if (Save == true) { s = linkLabel18.BackColor.ToString(); } else { linkLabel18.BackColor = Globals.actualC; linkLabel18.Tag = r; linkLabel18.Text = l.ToString(); }
            }
            else if (i == 19)
            {

                if (Save == true) { s = linkLabel19.BackColor.ToString(); } else { linkLabel19.BackColor = Globals.actualC; linkLabel19.Tag = r; linkLabel19.Text = l.ToString(); }
            }
            else if (i == 20)
            {

                if (Save == true) { s = linkLabel20.BackColor.ToString(); } else { linkLabel20.BackColor = Globals.actualC; linkLabel20.Tag = r; linkLabel20.Text = l.ToString(); }
            }
            else if (i == 21)
            {

                if (Save == true) { s = linkLabel21.BackColor.ToString(); } else { linkLabel21.BackColor = Globals.actualC; linkLabel21.Tag = r; linkLabel21.Text = l.ToString(); }
            }
            else if (i == 22)
            {

                if (Save == true) { s = linkLabel22.BackColor.ToString(); } else { linkLabel22.BackColor = Globals.actualC; linkLabel22.Tag = r; linkLabel22.Text = l.ToString(); }
            }
            else if (i == 23)
            {

                if (Save == true) { s = linkLabel23.BackColor.ToString(); } else { linkLabel23.BackColor = Globals.actualC; linkLabel23.Tag = r; linkLabel23.Text = l.ToString(); }
            }
            else if (i == 24)
            {

                if (Save == true) { s = linkLabel24.BackColor.ToString(); } else { linkLabel24.BackColor = Globals.actualC; linkLabel24.Tag = r; linkLabel24.Text = l.ToString(); }
            }
            else if (i == 25)
            {

                if (Save == true) { s = linkLabel25.BackColor.ToString(); } else { linkLabel25.BackColor = Globals.actualC; linkLabel25.Tag = r; linkLabel25.Text = l.ToString(); }
            }
            else if (i == 26)
            {

                if (Save == true) { s = linkLabel26.BackColor.ToString(); } else { linkLabel26.BackColor = Globals.actualC; linkLabel26.Tag = r; linkLabel26.Text = l.ToString(); }
            }
            else if (i == 27)
            {

                if (Save == true) { s = linkLabel27.BackColor.ToString(); } else { linkLabel27.BackColor = Globals.actualC; linkLabel27.Tag = r; linkLabel27.Text = l.ToString(); }
            }
            else if (i == 28)
            {

                if (Save == true) { s = linkLabel28.BackColor.ToString(); } else { linkLabel28.BackColor = Globals.actualC; linkLabel28.Tag = r; linkLabel28.Text = l.ToString(); }
            }
            else if (i == 29)
            {

                if (Save == true) { s = linkLabel29.BackColor.ToString(); } else { linkLabel29.BackColor = Globals.actualC; linkLabel29.Tag = r; linkLabel29.Text = l.ToString(); }
            }
            else if (i == 30)
            {

                if (Save == true) { s = linkLabel30.BackColor.ToString(); } else { linkLabel30.BackColor = Globals.actualC; linkLabel30.Tag = r; linkLabel30.Text = l.ToString(); }
            }
            else if (i == 31)
            {

                if (Save == true) { s = linkLabel31.BackColor.ToString(); } else { linkLabel31.BackColor = Globals.actualC; linkLabel31.Tag = r; linkLabel31.Text = l.ToString(); }
            }
            else if (i == 32)
            {

                if (Save == true) { s = linkLabel32.BackColor.ToString(); } else { linkLabel32.BackColor = Globals.actualC; linkLabel32.Tag = r; linkLabel32.Text = l.ToString(); }
            }
            else if (i == 33)
            {

                if (Save == true) { s = linkLabel33.BackColor.ToString(); } else { linkLabel33.BackColor = Globals.actualC; linkLabel33.Tag = r; linkLabel33.Text = l.ToString(); }
            }
            else if (i == 34)
            {

                if (Save == true) { s = linkLabel34.BackColor.ToString(); } else { linkLabel34.BackColor = Globals.actualC; linkLabel34.Tag = r; linkLabel34.Text = l.ToString(); }
            }
            else if (i == 35)
            {

                if (Save == true) { s = linkLabel35.BackColor.ToString(); } else { linkLabel35.BackColor = Globals.actualC; linkLabel35.Tag = r; linkLabel35.Text = l.ToString(); }
            }
            else if (i == 36)
            {

                if (Save == true) { s = linkLabel36.BackColor.ToString(); } else { linkLabel36.BackColor = Globals.actualC; linkLabel36.Tag = r; linkLabel36.Text = l.ToString(); }
            }
            else if (i == 37)
            {

                if (Save == true) { s = linkLabel37.BackColor.ToString(); } else { linkLabel37.BackColor = Globals.actualC; linkLabel37.Tag = r; linkLabel37.Text = l.ToString(); }
            }
            else if (i == 38)
            {

                if (Save == true) { s = linkLabel38.BackColor.ToString(); } else { linkLabel38.BackColor = Globals.actualC; linkLabel38.Tag = r; linkLabel38.Text = l.ToString(); }
            }
            else if (i == 39)
            {

                if (Save == true) { s = linkLabel39.BackColor.ToString(); } else { linkLabel39.BackColor = Globals.actualC; linkLabel39.Tag = r; linkLabel39.Text = l.ToString(); }
            }
            else if (i == 40)
            {

                if (Save == true) { s = linkLabel40.BackColor.ToString(); } else { linkLabel40.BackColor = Globals.actualC; linkLabel40.Tag = r; linkLabel40.Text = l.ToString(); }
            }
            else if (i == 41)
            {

                if (Save == true) { s = linkLabel41.BackColor.ToString(); } else { linkLabel41.BackColor = Globals.actualC; linkLabel41.Tag = r; linkLabel41.Text = l.ToString(); }
            }
            else if (i == 42)
            {

                if (Save == true) { s = linkLabel42.BackColor.ToString(); } else { linkLabel42.BackColor = Globals.actualC; linkLabel42.Tag = r; linkLabel42.Text = l.ToString(); }
            }
            else if (i == 43)
            {

                if (Save == true) { s = linkLabel43.BackColor.ToString(); } else { linkLabel43.BackColor = Globals.actualC; linkLabel43.Tag = r; linkLabel43.Text = l.ToString(); }
            }
            else if (i == 44)
            {

                if (Save == true) { s = linkLabel44.BackColor.ToString(); } else { linkLabel44.BackColor = Globals.actualC; linkLabel44.Tag = r; linkLabel44.Text = l.ToString(); }
            }
            else if (i == 45)
            {

                if (Save == true) { s = linkLabel45.BackColor.ToString(); } else { linkLabel45.BackColor = Globals.actualC; linkLabel45.Tag = r; linkLabel45.Text = l.ToString(); }
            }
            else if (i == 46)
            {

                if (Save == true) { s = linkLabel46.BackColor.ToString(); } else { linkLabel46.BackColor = Globals.actualC; linkLabel46.Tag = r; linkLabel46.Text = l.ToString(); }
            }
            else if (i == 47)
            {

                if (Save == true) { s = linkLabel47.BackColor.ToString(); } else { linkLabel47.BackColor = Globals.actualC; linkLabel47.Tag = r; linkLabel47.Text = l.ToString(); }
            }
            else if (i == 48)
            {

                if (Save == true) { s = linkLabel48.BackColor.ToString(); } else { linkLabel48.BackColor = Globals.actualC; linkLabel48.Tag = r; linkLabel48.Text = l.ToString(); }
            }
            else if (i == 49)
            {

                if (Save == true) { s = linkLabel49.BackColor.ToString(); } else { linkLabel49.BackColor = Globals.actualC; linkLabel49.Tag = r; linkLabel49.Text = l.ToString(); }
            }
            else if (i == 50)
            {

                if (Save == true) { s = linkLabel50.BackColor.ToString(); } else { linkLabel50.BackColor = Globals.actualC; linkLabel50.Tag = r; linkLabel50.Text = l.ToString(); }
            }
            else if (i == 51)
            {

                if (Save == true) { s = linkLabel51.BackColor.ToString(); } else { linkLabel51.BackColor = Globals.actualC; linkLabel51.Tag = r; linkLabel51.Text = l.ToString(); }
            }
            else if (i == 52)
            {

                if (Save == true) { s = linkLabel52.BackColor.ToString(); } else { linkLabel52.BackColor = Globals.actualC; linkLabel52.Tag = r; linkLabel52.Text = l.ToString(); }
            }
            else if (i == 53)
            {

                if (Save == true) { s = linkLabel53.BackColor.ToString(); } else { linkLabel53.BackColor = Globals.actualC; linkLabel53.Tag = r; linkLabel53.Text = l.ToString(); }
            }
            else if (i == 54)
            {

                if (Save == true) { s = linkLabel54.BackColor.ToString(); } else { linkLabel54.BackColor = Globals.actualC; linkLabel54.Tag = r; linkLabel54.Text = l.ToString(); }
            }
            else if (i == 55)
            {

                if (Save == true) { s = linkLabel55.BackColor.ToString(); } else { linkLabel55.BackColor = Globals.actualC; linkLabel55.Tag = r; linkLabel55.Text = l.ToString(); }
            }
            else if (i == 56)
            {

                if (Save == true) { s = linkLabel56.BackColor.ToString(); } else { linkLabel56.BackColor = Globals.actualC; linkLabel56.Tag = r; linkLabel56.Text = l.ToString(); }
            }
            else if (i == 57)
            {

                if (Save == true) { s = linkLabel57.BackColor.ToString(); } else { linkLabel57.BackColor = Globals.actualC; linkLabel57.Tag = r; linkLabel57.Text = l.ToString(); }
            }
            else if (i == 58)
            {

                if (Save == true) { s = linkLabel58.BackColor.ToString(); } else { linkLabel58.BackColor = Globals.actualC; linkLabel58.Tag = r; linkLabel58.Text = l.ToString(); }
            }
            else if (i == 59)
            {

                if (Save == true) { s = linkLabel59.BackColor.ToString(); } else { linkLabel59.BackColor = Globals.actualC; linkLabel59.Tag = r; linkLabel59.Text = l.ToString(); }
            }
            else if (i == 60)
            {

                if (Save == true) { s = linkLabel60.BackColor.ToString(); } else { linkLabel60.BackColor = Globals.actualC; linkLabel60.Tag = r; linkLabel60.Text = l.ToString(); }
            }
            else if (i == 61)
            {

                if (Save == true) { s = linkLabel61.BackColor.ToString(); } else { linkLabel61.BackColor = Globals.actualC; linkLabel61.Tag = r; linkLabel61.Text = l.ToString(); }
            }
            else if (i == 62)
            {

                if (Save == true) { s = linkLabel62.BackColor.ToString(); } else { linkLabel62.BackColor = Globals.actualC; linkLabel62.Tag = r; linkLabel62.Text = l.ToString(); }
            }
            else if (i == 63)
            {

                if (Save == true) { s = linkLabel63.BackColor.ToString(); } else { linkLabel63.BackColor = Globals.actualC; linkLabel63.Tag = r; linkLabel63.Text = l.ToString(); }
            }
            else if (i == 64)
            {

                if (Save == true) { s = linkLabel64.BackColor.ToString(); } else { linkLabel64.BackColor = Globals.actualC; linkLabel64.Tag = r; linkLabel64.Text = l.ToString(); }
            }
            else if (i == 65)
            {

                if (Save == true) { s = linkLabel65.BackColor.ToString(); } else { linkLabel65.BackColor = Globals.actualC; linkLabel65.Tag = r; linkLabel65.Text = l.ToString(); }
            }
            else if (i == 66)
            {

                if (Save == true) { s = linkLabel66.BackColor.ToString(); } else { linkLabel66.BackColor = Globals.actualC; linkLabel66.Tag = r; linkLabel66.Text = l.ToString(); }
            }
            else if (i == 67)
            {

                if (Save == true) { s = linkLabel67.BackColor.ToString(); } else { linkLabel67.BackColor = Globals.actualC; linkLabel67.Tag = r; linkLabel67.Text = l.ToString(); }
            }
            else if (i == 68)
            {

                if (Save == true) { s = linkLabel68.BackColor.ToString(); } else { linkLabel68.BackColor = Globals.actualC; linkLabel68.Tag = r; linkLabel68.Text = l.ToString(); }
            }
            else if (i == 69)
            {

                if (Save == true) { s = linkLabel69.BackColor.ToString(); } else { linkLabel69.BackColor = Globals.actualC; linkLabel69.Tag = r; linkLabel69.Text = l.ToString(); }
            }
            else if (i == 70)
            {

                if (Save == true) { s = linkLabel70.BackColor.ToString(); } else { linkLabel70.BackColor = Globals.actualC; linkLabel70.Tag = r; linkLabel70.Text = l.ToString(); }
            }
            else if (i == 71)
            {

                if (Save == true) { s = linkLabel71.BackColor.ToString(); } else { linkLabel71.BackColor = Globals.actualC; linkLabel71.Tag = r; linkLabel71.Text = l.ToString(); }
            }
            else if (i == 72)
            {

                if (Save == true) { s = linkLabel72.BackColor.ToString(); } else { linkLabel72.BackColor = Globals.actualC; linkLabel72.Tag = r; linkLabel72.Text = l.ToString(); }
            }
            else if (i == 73)
            {

                if (Save == true) { s = linkLabel73.BackColor.ToString(); } else { linkLabel73.BackColor = Globals.actualC; linkLabel73.Tag = r; linkLabel73.Text = l.ToString(); }
            }
            else if (i == 74)
            {

                if (Save == true) { s = linkLabel74.BackColor.ToString(); } else { linkLabel74.BackColor = Globals.actualC; linkLabel74.Tag = r; linkLabel74.Text = l.ToString(); }
            }
            else if (i == 75)
            {

                if (Save == true) { s = linkLabel75.BackColor.ToString(); } else { linkLabel75.BackColor = Globals.actualC; linkLabel75.Tag = r; linkLabel75.Text = l.ToString(); }
            }
            else if (i == 76)
            {

                if (Save == true) { s = linkLabel76.BackColor.ToString(); } else { linkLabel76.BackColor = Globals.actualC; linkLabel76.Tag = r; linkLabel76.Text = l.ToString(); }
            }
            else if (i == 77)
            {

                if (Save == true) { s = linkLabel77.BackColor.ToString(); } else { linkLabel77.BackColor = Globals.actualC; linkLabel77.Tag = r; linkLabel77.Text = l.ToString(); }
            }
            else if (i == 78)
            {

                if (Save == true) { s = linkLabel78.BackColor.ToString(); } else { linkLabel78.BackColor = Globals.actualC; linkLabel78.Tag = r; linkLabel78.Text = l.ToString(); }
            }
            else if (i == 79)
            {

                if (Save == true) { s = linkLabel79.BackColor.ToString(); } else { linkLabel79.BackColor = Globals.actualC; linkLabel79.Tag = r; linkLabel79.Text = l.ToString(); }
            }
            else if (i == 80)
            {

                if (Save == true) { s = linkLabel80.BackColor.ToString(); } else { linkLabel80.BackColor = Globals.actualC; linkLabel80.Tag = r; linkLabel80.Text = l.ToString(); }
            }
            else if (i == 81)
            {

                if (Save == true) { s = linkLabel81.BackColor.ToString(); } else { linkLabel81.BackColor = Globals.actualC; linkLabel81.Tag = r; linkLabel81.Text = l.ToString(); }
            }
            else if (i == 82)
            {

                if (Save == true) { s = linkLabel82.BackColor.ToString(); } else { linkLabel82.BackColor = Globals.actualC; linkLabel82.Tag = r; linkLabel82.Text = l.ToString(); }
            }
            else if (i == 83)
            {

                if (Save == true) { s = linkLabel83.BackColor.ToString(); } else { linkLabel83.BackColor = Globals.actualC; linkLabel83.Tag = r; linkLabel83.Text = l.ToString(); }
            }
            else if (i == 84)
            {

                if (Save == true) { s = linkLabel84.BackColor.ToString(); } else { linkLabel84.BackColor = Globals.actualC; linkLabel84.Tag = r; linkLabel84.Text = l.ToString(); }
            }
            else if (i == 85)
            {

                if (Save == true) { s = linkLabel85.BackColor.ToString(); } else { linkLabel85.BackColor = Globals.actualC; linkLabel85.Tag = r; linkLabel85.Text = l.ToString(); }
            }
            else if (i == 86)
            {

                if (Save == true) { s = linkLabel86.BackColor.ToString(); } else { linkLabel86.BackColor = Globals.actualC; linkLabel86.Tag = r; linkLabel86.Text = l.ToString(); }
            }
            else if (i == 87)
            {

                if (Save == true) { s = linkLabel87.BackColor.ToString(); } else { linkLabel87.BackColor = Globals.actualC; linkLabel87.Tag = r; linkLabel87.Text = l.ToString(); }
            }
            else if (i == 88)
            {

                if (Save == true) { s = linkLabel88.BackColor.ToString(); } else { linkLabel88.BackColor = Globals.actualC; linkLabel88.Tag = r; linkLabel88.Text = l.ToString(); }
            }
            else if (i == 89)
            {

                if (Save == true) { s = linkLabel89.BackColor.ToString(); } else { linkLabel89.BackColor = Globals.actualC; linkLabel89.Tag = r; linkLabel89.Text = l.ToString(); }
            }
            else if (i == 90)
            {

                if (Save == true) { s = linkLabel90.BackColor.ToString(); } else { linkLabel90.BackColor = Globals.actualC; linkLabel90.Tag = r; linkLabel90.Text = l.ToString(); }
            }
            else if (i == 91)
            {

                if (Save == true) { s = linkLabel91.BackColor.ToString(); } else { linkLabel91.BackColor = Globals.actualC; linkLabel91.Tag = r; linkLabel91.Text = l.ToString(); }
            }
            else if (i == 92)
            {

                if (Save == true) { s = linkLabel92.BackColor.ToString(); } else { linkLabel92.BackColor = Globals.actualC; linkLabel92.Tag = r; linkLabel92.Text = l.ToString(); }
            }
            else if (i == 93)
            {

                if (Save == true) { s = linkLabel93.BackColor.ToString(); } else { linkLabel93.BackColor = Globals.actualC; linkLabel93.Tag = r; linkLabel93.Text = l.ToString(); }
            }
            else if (i == 94)
            {

                if (Save == true) { s = linkLabel94.BackColor.ToString(); } else { linkLabel94.BackColor = Globals.actualC; linkLabel94.Tag = r; linkLabel94.Text = l.ToString(); }
            }
            else if (i == 95)
            {

                if (Save == true) { s = linkLabel95.BackColor.ToString(); } else { linkLabel95.BackColor = Globals.actualC; linkLabel95.Tag = r; linkLabel95.Text = l.ToString(); }
            }
            else if (i == 96)
            {

                if (Save == true) { s = linkLabel96.BackColor.ToString(); } else { linkLabel96.BackColor = Globals.actualC; linkLabel96.Tag = r; linkLabel96.Text = l.ToString(); }
            }
            else if (i == 97)
            {

                if (Save == true) { s = linkLabel97.BackColor.ToString(); } else { linkLabel97.BackColor = Globals.actualC; linkLabel97.Tag = r; linkLabel97.Text = l.ToString(); }
            }
            else if (i == 98)
            {

                if (Save == true) { s = linkLabel98.BackColor.ToString(); } else { linkLabel98.BackColor = Globals.actualC; linkLabel98.Tag = r; linkLabel98.Text = l.ToString(); }
            }
            else if (i == 99)
            {

                if (Save == true) { s = linkLabel99.BackColor.ToString(); } else { linkLabel99.BackColor = Globals.actualC; linkLabel99.Tag = r; linkLabel99.Text = l.ToString(); }
            }
            else if (i == 100)
            {

                if (Save == true) { s = linkLabel100.BackColor.ToString(); } else { linkLabel100.BackColor = Globals.actualC; linkLabel100.Tag = r; linkLabel100.Text = l.ToString(); }
            }
            else if (i == 101)
            {

                if (Save == true) { s = linkLabel101.BackColor.ToString(); } else { linkLabel101.BackColor = Globals.actualC; linkLabel101.Tag = r; linkLabel101.Text = l.ToString(); }
            }
            else if (i == 102)
            {

                if (Save == true) { s = linkLabel102.BackColor.ToString(); } else { linkLabel102.BackColor = Globals.actualC; linkLabel102.Tag = r; linkLabel102.Text = l.ToString(); }
            }
            else if (i == 103)
            {

                if (Save == true) { s = linkLabel103.BackColor.ToString(); } else { linkLabel103.BackColor = Globals.actualC; linkLabel103.Tag = r; linkLabel103.Text = l.ToString(); }
            }
            else if (i == 104)
            {

                if (Save == true) { s = linkLabel104.BackColor.ToString(); } else { linkLabel104.BackColor = Globals.actualC; linkLabel104.Tag = r; linkLabel104.Text = l.ToString(); }
            }
            else if (i == 105)
            {

                if (Save == true) { s = linkLabel105.BackColor.ToString(); } else { linkLabel105.BackColor = Globals.actualC; linkLabel105.Tag = r; linkLabel105.Text = l.ToString(); }
            }
            else if (i == 106)
            {

                if (Save == true) { s = linkLabel106.BackColor.ToString(); } else { linkLabel106.BackColor = Globals.actualC; linkLabel106.Tag = r; linkLabel106.Text = l.ToString(); }
            }
            else if (i == 107)
            {

                if (Save == true) { s = linkLabel107.BackColor.ToString(); } else { linkLabel107.BackColor = Globals.actualC; linkLabel107.Tag = r; linkLabel107.Text = l.ToString(); }
            }
            else if (i == 108)
            {

                if (Save == true) { s = linkLabel108.BackColor.ToString(); } else { linkLabel108.BackColor = Globals.actualC; linkLabel108.Tag = r; linkLabel108.Text = l.ToString(); }
            }
            else if (i == 109)
            {

                if (Save == true) { s = linkLabel109.BackColor.ToString(); } else { linkLabel109.BackColor = Globals.actualC; linkLabel109.Tag = r; linkLabel109.Text = l.ToString(); }
            }
            else if (i == 110)
            {

                if (Save == true) { s = linkLabel110.BackColor.ToString(); } else { linkLabel110.BackColor = Globals.actualC; linkLabel110.Tag = r; linkLabel110.Text = l.ToString(); }
            }
            else if (i == 111)
            {

                if (Save == true) { s = linkLabel111.BackColor.ToString(); } else { linkLabel111.BackColor = Globals.actualC; linkLabel111.Tag = r; linkLabel111.Text = l.ToString(); }
            }
            else if (i == 112)
            {

                if (Save == true) { s = linkLabel112.BackColor.ToString(); } else { linkLabel112.BackColor = Globals.actualC; linkLabel112.Tag = r; linkLabel112.Text = l.ToString(); }
            }
            else if (i == 113)
            {

                if (Save == true) { s = linkLabel113.BackColor.ToString(); } else { linkLabel113.BackColor = Globals.actualC; linkLabel113.Tag = r; linkLabel113.Text = l.ToString(); }
            }
            else if (i == 114)
            {

                if (Save == true) { s = linkLabel114.BackColor.ToString(); } else { linkLabel114.BackColor = Globals.actualC; linkLabel114.Tag = r; linkLabel114.Text = l.ToString(); }
            }
            else if (i == 115)
            {

                if (Save == true) { s = linkLabel115.BackColor.ToString(); } else { linkLabel115.BackColor = Globals.actualC; linkLabel115.Tag = r; linkLabel115.Text = l.ToString(); }
            }
            else if (i == 116)
            {

                if (Save == true) { s = linkLabel116.BackColor.ToString(); } else { linkLabel116.BackColor = Globals.actualC; linkLabel116.Tag = r; linkLabel116.Text = l.ToString(); }
            }
            else if (i == 117)
            {

                if (Save == true) { s = linkLabel117.BackColor.ToString(); } else { linkLabel117.BackColor = Globals.actualC; linkLabel117.Tag = r; linkLabel117.Text = l.ToString(); }
            }
            else if (i == 118)
            {

                if (Save == true) { s = linkLabel118.BackColor.ToString(); } else { linkLabel118.BackColor = Globals.actualC; linkLabel118.Tag = r; linkLabel118.Text = l.ToString(); }
            }
            else if (i == 119)
            {

                if (Save == true) { s = linkLabel119.BackColor.ToString(); } else { linkLabel119.BackColor = Globals.actualC; linkLabel119.Tag = r; linkLabel119.Text = l.ToString(); }
            }
            else if (i == 120)
            {
                PSaccess(true);
                if (Save == true) { s = linkLabel120.BackColor.ToString(); } else { linkLabel120.BackColor = Globals.actualC; linkLabel120.Tag = r; linkLabel120.Text = l.ToString(); }
            }
            return s;
        }

        private void RF_MLB_Click(object sender, MouseEventArgs e)
        {
            Globals.rfHK = 1;
            if(RF_AK.Checked == true)
            {
                RF_AK.Checked = false;
            }
            else if (RF_MX1.Checked == true)
            {
                RF_MX1.Checked = false;
            }
            else if (RF_MX2.Checked == true)
            {
                RF_MX2.Checked = false;
            }
        }

        private void CTB_MRB_Click(object sender, MouseEventArgs e)
        {
            Globals.tbHK = 1;
            if (CTB_AK.Checked == true)
            {
                CTB_AK.Checked = false;
            }
            else if (CTB_MX1.Checked == true)
            {
                CTB_MX1.Checked = false;
            }
            else if (CTB_MX2.Checked == true)
            {
                CTB_MX2.Checked = false;
            }
        }

        private void RF_AK_Click(object sender, MouseEventArgs e)
        {
            if (CTB_AK.Checked == true)
            {
                Globals.tbHK = 1;
                CTB_MRB.Checked = true;
            }
            Globals.rfHK = 4;
            if (RF_MLB.Checked == true)
            {
                RF_MLB.Checked = false;
            }
            else if (RF_MX1.Checked == true)
            {
                RF_MX1.Checked = false;
            }
            else if (RF_MX2.Checked == true)
            {
                RF_MX2.Checked = false;
            }
        }

        private void CTB_AK_Click(object sender, MouseEventArgs e)
        {
            if (RF_AK.Checked == true)
            {
                Globals.rfHK = 1;
                RF_MLB.Checked = true;
            }
            Globals.tbHK = 4;
            if (CTB_MRB.Checked == true)
            {
                CTB_MRB.Checked = false;
            }
            else if (CTB_MX1.Checked == true)
            {
                CTB_MX1.Checked = false;
            }
            else if (CTB_MX2.Checked == true)
            {
                CTB_MX2.Checked = false;
            }
        }

        private void RF_MX2_Click(object sender, MouseEventArgs e)
        {
            if (CTB_MX2.Checked == true)
            {
                Globals.tbHK = 1;
                CTB_MRB.Checked = true;
            }
            Globals.rfHK = 3;
            if (RF_MLB.Checked == true)
            {
                RF_MLB.Checked = false;
            }
            else if (RF_MX1.Checked == true)
            {
                RF_MX1.Checked = false;
            }
            else if (RF_AK.Checked == true)
            {
                RF_AK.Checked = false;
            }
        }

        private void CTB_MX2_Click(object sender, MouseEventArgs e)
        {
            if (RF_MX2.Checked == true)
            {
                Globals.rfHK = 1;
                RF_MLB.Checked = true;
            }
            Globals.tbHK = 3;
            if (CTB_MRB.Checked == true)
            {
                CTB_MRB.Checked = false;
            }
            else if (CTB_MX1.Checked == true)
            {
                CTB_MX1.Checked = false;
            }
            else if (CTB_AK.Checked == true)
            {
                CTB_AK.Checked = false;
            }
        }

        private void RF_MX1_Click(object sender, MouseEventArgs e)
        {
            if (CTB_MX1.Checked == true)
            {
                Globals.tbHK = 1;
                CTB_MRB.Checked = true;
            }
            Globals.rfHK = 2;
            if (RF_MLB.Checked == true)
            {
                RF_MLB.Checked = false;
            }
            else if (RF_MX2.Checked == true)
            {
                RF_MX2.Checked = false;
            }
            else if (RF_AK.Checked == true)
            {
                RF_AK.Checked = false;
            }
        }

        private void CTB_MX1_Click(object sender, MouseEventArgs e)
        {
            if (RF_MX1.Checked == true)
            {
                Globals.rfHK = 1;
                RF_MLB.Checked = true;
            }
            Globals.tbHK = 2;
            if (CTB_MRB.Checked == true)
            {
                CTB_MRB.Checked = false;
            }
            else if (CTB_MX2.Checked == true)
            {
                CTB_MX2.Checked = false;
            }
            else if (CTB_AK.Checked == true)
            {
                CTB_AK.Checked = false;
            }
        }
    }
}
