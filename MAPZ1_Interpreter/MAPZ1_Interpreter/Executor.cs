using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using Hooks;
using MAPZ1_Interpreter;
using System.Text.RegularExpressions;
using System.IO;


namespace Interpreter
{
    public delegate string AccountHandler(string message);
    class Executor
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern long SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point point);

        [DllImport("User32.dll")]
        static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        double xScaling;
        double yScaling;

        [Flags]
        enum MouseFlags
        {
            Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, Absolute = 0x8000
        };
        Form1 frm;
        public Executor(Form1 form)
        {
            MouseHook.LocalHook = false;
            MouseHook.InstallHook();
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            xScaling = (double)65535 / resolution.Width;
            yScaling = (double)65535 / resolution.Height;
            frm = form;
            
        }

        List<object> list = new List<object>();
        object[] UserActions;

        public bool stopFlag = false;
        bool active = false;

        

        public void FlagChange()
        {
            stopFlag = true;           
        }

        public void Flg()
        {
            stopFlag = true;
        }
        
        public delegate void SetKey(int key);
        public delegate void SetModifiedKey(int key, int modifier);
        public delegate void MyEvent();
        public delegate void ExceptionThrower(ArgumentException exception);
        public event SetKey SetStopRecordKey;
        public event SetKey SetStopPlaybackKey;
        public event SetKey SetStartPlaybackKey;
        public event SetKey SetStartRecordKey;
        public event SetModifiedKey SetModifiedStopRecordKey;
        public event SetModifiedKey SetModifiedStopPlaybackKey;
        public event SetModifiedKey SetModifiedStartPlaybackKey;
        public event SetModifiedKey SetModifiedStartRecordKey;

        Regex reg = new Regex("\"");
        string Key, Key2;
        string key, key2;
        static KeysConverter k = new KeysConverter();

        public void Execute(TreeNode node)
        {
            FunctionTreeNode Node;
            CycleTreeNode cycleNode;
            
            SetStopRecordKey += frm.SetStopRecordKey;
            SetStopPlaybackKey += frm.SetStopPlaybackKey;
            SetStartPlaybackKey += frm.SetStartPlaybackKey;
            SetStartRecordKey += frm.SetStartRecordKey;

            SetModifiedStopRecordKey += frm.SetStopRecordKey;
            SetModifiedStopPlaybackKey += frm.SetStopPlaybackKey;
            SetModifiedStartPlaybackKey += frm.SetStartPlaybackKey;
            SetModifiedStartRecordKey += frm.SetStartRecordKey;

            if (node is FunctionTreeNode)
            {
                Node = (FunctionTreeNode)node;
                switch (Node.Name)
                {
                    case "LeftClick":
                        switch (Node.Parameters.Length) 
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 2:
                                SetCursorPos(int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()));
                                mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                break;
                            default:
                                throw new ArgumentException($"Too much arguments in {Node.Name}");
                        }
                        break;

                    case "RightClick":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                
                                break;
                            case 2:
                                SetCursorPos(int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()));
                                mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                break;

                            default:
                                throw new ArgumentException($"Too much arguments in {Node.Name}");
                        }
                        break;

                    case "Start":
                        switch (Node.Parameters.Length)
                        {
                            case 0: 
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                Process.Start(Node.Parameters[0].ToString());
                                break;

                            default:
                                throw new ArgumentException($"Too much arguments in {Node.Name}");
                        }                       
                        break;

                    case "MoveCursorTo":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 2:
                                SetCursorPos(int.Parse(Node.Parameters[0].ToString()), int.Parse(Node.Parameters[1].ToString()));
                                break;

                            default:
                                throw new ArgumentException($"Too much arguments in {Node.Name}");
                        }
                        
                        break;

                    case "PressKey":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                string key = Node.Parameters[0].ToString();
                                Regex rg = new Regex(" \" ");
                                string str = key.Trim('"');
                                int ff = (int)(Keys)k.ConvertFromString(str);
                                KeyboardSend.KeyDown((Keys)k.ConvertFromString(str));
                                KeyboardSend.KeyUp((Keys)k.ConvertFromString(str));
                                break;

                            default:
                                throw new ArgumentException($"Too much arguments in {Node.Name}");
                        }
                        break;

                    case "StartRecord":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                SetStartRecordKey((int)k.ConvertFromString(key));
                                break;

                            case 2:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                Key2 = Node.Parameters[1].ToString();
                                key2 = reg.Replace(Key2, "");
                                SetModifiedStartRecordKey((int)k.ConvertFromString(key), (int)k.ConvertFromString(key2));                              
                                break;
                        }                      
                        break;

                    case "StopRecord":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                SetStopRecordKey((int)k.ConvertFromString(key));
                                break;

                            case 2:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                Key2 = Node.Parameters[1].ToString();
                                key2 = reg.Replace(Key2, "");
                                SetModifiedStopRecordKey((int)k.ConvertFromString(key), (int)k.ConvertFromString(key2));
                                break;
                        }                 
                        break;

                    case "StartPlayback":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                SetStartPlaybackKey((int)k.ConvertFromString(key));
                                break;

                            case 2:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                Key2 = Node.Parameters[1].ToString();
                                key2 = reg.Replace(Key2, "");
                                SetModifiedStartPlaybackKey((int)k.ConvertFromString(key), (int)k.ConvertFromString(key2));
                                break;
                        }                                 
                        break;

                    case "StopPlayback":
                        switch (Node.Parameters.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Too few arguments in {Node.Name}");

                            case 1:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                SetStopPlaybackKey((int)k.ConvertFromString(key));
                                break;

                            case 2:
                                Key = Node.Parameters[0].ToString();
                                key = reg.Replace(Key, "");
                                Key2 = Node.Parameters[1].ToString();
                                key2 = reg.Replace(Key2, "");
                                SetModifiedStopPlaybackKey((int)k.ConvertFromString(key), (int)k.ConvertFromString(key2));
                                break;
                        }
                        break;

                    default:
                        throw new ArgumentException($"Undefined function \"{Node.Name}\"");                       
                }
            }
            else if (node is CycleTreeNode)
            {
                cycleNode = (node as CycleTreeNode);
                if(cycleNode.Name == "Repeat")
                {
                    try
                    {
                        switch (cycleNode.Arguments.Length)
                        {
                            case 0:
                                throw new ArgumentException($"Error: Too few arguments in Repeat cycle(must be 1 or 2)");

                            case 1:
                                new Thread(() =>
                                {
                                    try
                                    {
                                        CycleInNewThread(int.Parse(cycleNode.Arguments[0].ToString()), cycleNode.Expressions);
                                    }
                                    catch (ArgumentException exception)
                                    {
                                        frm.richTextBox1.Text += $"{exception.Message}\n";
                                    }
                                }).Start();
                                break;

                            case 2:
                                new Thread(() => 
                                {
                                    try
                                    {
                                        if (cycleNode.Expressions.Length == 0) { }
                                        else CycleInNewThread(int.Parse(cycleNode.Arguments[0].ToString()), int.Parse(cycleNode.Arguments[1].ToString()), cycleNode.Expressions);
                                    }
                                    catch (ArgumentException exception)
                                    {
                                        frm.richTextBox1.Text += $"{exception.Message}\n";
                                    }
                                }).Start();
                                break;

                            default:
                                throw new ArgumentException($"Error: Too much arguments in Repeat cycle(must be 1 or 2)");
                        }
                    }
                    catch(ArgumentException exception)
                    {
                        frm.richTextBox1.Text += exception.Message;
                    }
                }
                else
                {
                    throw new ArgumentException($"Error: Unknown cycle \"{cycleNode.Name}\"(Maybe, your mean \"Repeat\"?)");
                }
            }
            
        }

        public void CycleInNewThread(int numOfIterations, TreeNode[] expressions)
        {
            for (int i = 0; i < numOfIterations; ++i)
            {
                foreach (FunctionTreeNode expression in expressions)
                {                 
                    switch (expression.Name)
                    {
                        case "LeftClick":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                    mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                    break;
                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "RightClick":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                    mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "Start":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    Process.Start(expression.Parameters[0].ToString());
                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "MoveCursorTo":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    break;//todo

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }

                            break;

                        case "PressKey":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    string key = expression.Parameters[0].ToString();
                                    KeyboardSend.KeyDown((Keys)k.ConvertFromString(key));
                                    KeyboardSend.KeyUp((Keys)k.ConvertFromString(key));

                                    
                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;
                    }
                }
            }

        }

        public void CycleInNewThread(int numOfIterations, int delay, TreeNode[] expressions)
        {
            for (int i = 0; i < numOfIterations; ++i)
            {
                foreach (FunctionTreeNode expression in expressions)
                {
                    switch (expression.Name)
                    {
                        case "LeftClick":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                    mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                    break;
                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "RightClick":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:

                                    break;
                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);
                                    mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()), 0, UIntPtr.Zero);// todo
                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "Start":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    Process.Start(expression.Parameters[0].ToString());
                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;

                        case "MoveCursorTo":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 2:
                                    SetCursorPos(int.Parse(expression.Parameters[0].ToString()), int.Parse(expression.Parameters[1].ToString()));
                                    break;//todo

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }

                            break;

                        case "PressKey":
                            switch (expression.Parameters.Length)
                            {
                                case 0:
                                    throw new ArgumentException($"Too few arguments in {expression.Name}");

                                case 1:
                                    string key = expression.Parameters[0].ToString();
                                    Regex rg = new Regex(" \" ");
                                    string str = key.Trim('"');
                                    int ff = (int)(Keys)k.ConvertFromString(str);
                                    KeyboardSend.KeyDown((Keys)k.ConvertFromString(str));
                                    KeyboardSend.KeyUp((Keys)k.ConvertFromString(str));

                                    break;

                                default:
                                    throw new ArgumentException($"Too much arguments in {expression.Name}");
                            }
                            break;
                    }
                }

                #if DEBUG
                frm.label1.Text += "f ";
                #endif

                Thread.Sleep(delay);//delay in cycle
            }
        }

        public void PlaybackInNewThread()
        {
            new Thread(() => playbackMouseMovement()).Start();
        }

        public void RecordInNewThread()
        {
            MouseHook.MouseDown += new MouseEventHandler(MouseHook_MouseDown);//subscribe on event
            MouseHook.MouseUp += new MouseEventHandler(MouseHook_MouseUp);//subscribe on event
            new Thread(() => RecordMouseMovement()).Start();
        }

        public void RecordMouseMovement()
        {

            for (; ; )
            {
                list.Add(new MousePoint(Cursor.Position.X, Cursor.Position.Y));                                                                           
                Thread.Sleep(25);
                if (stopFlag == true)
                {
                    stopFlag = false;
                    break;
                }
            }
            UserActions = list.ToArray();
            MouseHook.UnInstallHook();
            
            MouseHook.MouseDown -= new MouseEventHandler(MouseHook_MouseDown);//unsubscribe event
            MouseHook.MouseUp -= new MouseEventHandler(MouseHook_MouseUp);//unsubscribe event

        }

        public void playbackMouseMovement()
        {
            bool leftMouseDown, leftMouseUp, rightMouseUp, rightMouseDown;

            foreach(object obj in UserActions)
            {
                Thread.Sleep(25);
                if (obj is MousePoint)
                {
                    SetCursorPos((obj as MousePoint).x, (obj as MousePoint).y);
                    leftMouseDown = (obj as MousePoint).leftDown;
                    leftMouseUp = (obj as MousePoint).leftUp;
                    rightMouseDown = (obj as MousePoint).rightDown;
                    rightMouseUp = (obj as MousePoint).rightUp;

                    if (leftMouseUp && active)
                    {
                        mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, 0, 0, 0, UIntPtr.Zero);
                        active = false;
                    }
                    else if (leftMouseDown && !active)
                    {
                        mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, 0, 0, 0, UIntPtr.Zero);
                        active = true;
                    }

                    else if (rightMouseUp && active)
                    {
                        mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, 0, 0, 0, UIntPtr.Zero);
                        active = false;
                    }
                    else if (rightMouseDown && !active)
                    {
                        mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, 0, 0, 0, UIntPtr.Zero);
                        active = true;
                    }

                    if (stopFlag == true)
                    {
                        stopFlag = false;
                        break;
                    }
                }
                else if(obj is PressedKey)
                {
                    KeyboardSend.KeyDown((Keys)((obj as PressedKey).code));
                    KeyboardSend.KeyUp((Keys)(obj as PressedKey).code);
                }
            }
        }

        public class MousePoint
        {
            public int x;
            public int y;
            public bool leftDown = false;
            public bool leftUp = false;
            public bool rightDown = false;
            public bool rightUp = false;

            public MousePoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public MousePoint(int x, int y, bool leftDown, bool leftUp, bool rightDown, bool rightUp)
            {
                this.x = x;
                this.y = y;
                this.leftDown = leftDown;
                this.leftUp = leftUp;
                this.rightDown = rightDown;
                this.rightUp = rightUp;
            }

            public MousePoint() { }          
        }

        public class PressedKey
        {
            public string name;
            public int code;
            
            public PressedKey(int code)
            {
                this.code = code;
                name = k.ConvertToString(code);
            }
        }

        void MouseHook_MouseUp(object sender, MouseEventArgs e)//registration of mouse up`s
        {
            if (e.Button == MouseButtons.Left)
                list.Add(new MousePoint(Cursor.Position.X, Cursor.Position.Y, false, true, false, false));
            else if (e.Button == MouseButtons.Right)
                list.Add(new MousePoint(Cursor.Position.X, Cursor.Position.Y, false, false, false, true));
        }

        void MouseHook_MouseDown(object sender, MouseEventArgs e)//registration of mouse down`s
        {
            if (e.Button == MouseButtons.Left)
                list.Add(new MousePoint(Cursor.Position.X, Cursor.Position.Y, true, false, false, false));
            else if (e.Button == MouseButtons.Right)
                list.Add(new MousePoint(Cursor.Position.X, Cursor.Position.Y, false, false, true, false));
        }

        public void KeyPress(int key)
        {
            list.Add(new PressedKey(key));
        }

    }
}
