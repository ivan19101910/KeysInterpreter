using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Interpreter;
using System.Runtime.InteropServices;
using Hooks;
using System.IO;

namespace MAPZ1_Interpreter
{
    public delegate void EventHandler(string message);
    
    public partial class Form1 : Form
    {       
        public delegate void MethodContainer();
        public delegate void KeyPressEvent(int key);

        public event MethodContainer onCount;
        public event MethodContainer StartPlayback;
        public event MethodContainer StartRecord;
        public event KeyPressEvent KeyPress;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        Executor executor;
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            executor = new Executor(this);
            onCount += executor.FlagChange;
            StartPlayback += executor.PlaybackInNewThread;
            StartRecord += executor.RecordInNewThread;
            KeyPress += executor.KeyPress;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string inputStr;
        string[] splittedStr;
        int STOP_RECORD_KEY_CODE, START_RECORD_KEY_CODE;
        int STOP_PLAYBACK_KEY_CODE, START_PLAYBACK_KEY_CODE;
        int errorCounter = 0;
  
        List<Token[]> tokens = new List<Token[]>();
        List<Interpreter.TreeNode> trees = new List<Interpreter.TreeNode>();
        public static bool signal = false;
        bool smth = true;
        KeysConverter conv = new KeysConverter();

        Regex regex = new Regex(@"\s+");
        Regex regex2 = new Regex(@"^\s+");

        ILexer lexer = new Lexer();
        Parser parser = new Parser();
        

        protected override void WndProc(ref Message m)//method for react on key pressing
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                if ((int)GetKey(m.LParam) == STOP_RECORD_KEY_CODE)
                {
                    onCount();
                    UnregisterAllKeys();
                }
                else if ((int)GetKey(m.LParam) == STOP_PLAYBACK_KEY_CODE)
                {
                    onCount();
                }
                else if ((int)GetKey(m.LParam) == START_PLAYBACK_KEY_CODE)
                {
                    StartPlayback();
                }
                else if ((int)GetKey(m.LParam) == START_RECORD_KEY_CODE)
                {
                    RegisterAllKeys();
                    StartRecord();
                }
                else if(smth)
                {
                    KeyPress((int)GetKey(m.LParam));//call keypress event for record user actions(Executor react on this)

                    UnregisterHotKey(this.Handle, (int)GetKey(m.LParam));//probably can cause error?
                    KeyboardSend.KeyDown(GetKey(m.LParam));//probably can cause error?
                    KeyboardSend.KeyUp(GetKey(m.LParam));//probably can cause error?
                    RegisterHotKey(this.Handle, (int)GetKey(m.LParam), 0, (int)GetKey(m.LParam));//probably can cause error?

                    #if DEBUG
                    label1.Text += GetKey(m.LParam);
                    #endif
                }
            }
            base.WndProc(ref m);
        }

        private Keys GetKey(IntPtr LParam)
        {
            return (Keys)((LParam.ToInt32()) >> 16); // not all of the parenthesis are needed, I just found it easier to see what's happening
        }

        public void SetStopRecordKey(int key)
        {
            RegisterHotKey(this.Handle, key, 0, key);
            STOP_RECORD_KEY_CODE = key;
        }

        public void SetStopRecordKey(int key, int modifier)
        {
            RegisterHotKey(this.Handle, key, ConvertModifierKey((Keys)modifier), key);
            STOP_RECORD_KEY_CODE = key;
        }

        public void SetStopPlaybackKey(int key)
        {
            RegisterHotKey(this.Handle, key, 0, key);
            STOP_PLAYBACK_KEY_CODE = key;
        }

        public void SetStopPlaybackKey(int key, int modifier)
        {
            RegisterHotKey(this.Handle, key, ConvertModifierKey((Keys)modifier), key);
            STOP_PLAYBACK_KEY_CODE = key;
        }

        public void SetStartPlaybackKey(int key)
        {
            RegisterHotKey(this.Handle, key, 0, key);
            START_PLAYBACK_KEY_CODE = key;
        }

        public void SetStartPlaybackKey(int key, int modifier)
        {
            RegisterHotKey(this.Handle, key, ConvertModifierKey((Keys)modifier), key);
            START_PLAYBACK_KEY_CODE = key;
        }

        public void SetStartRecordKey(int key)
        {
            RegisterHotKey(this.Handle, key, 0, key);
            START_RECORD_KEY_CODE = key;
        }

        public void SetStartRecordKey(int key, int modifier)
        {
            RegisterHotKey(this.Handle, key, ConvertModifierKey((Keys)modifier), key);
            START_RECORD_KEY_CODE = key;
        }

        public void RegisterAllKeys()
        {
            foreach (Keys el in Enum.GetValues(typeof(Keys)))
            {
                if ((int)el != STOP_RECORD_KEY_CODE && (int)el != START_RECORD_KEY_CODE && (int)el != STOP_PLAYBACK_KEY_CODE && (int)el != START_PLAYBACK_KEY_CODE)// if dont do this, modifier keys dont work
                    RegisterHotKey(this.Handle, (int)el, 0, (int)el);//3 argument is modifier(ctrl, shift ..)
            }
        }

        public void UnregisterAllKeys()
        {
            foreach (Keys el in Enum.GetValues(typeof(Keys)))
            {
                if ((int)el != STOP_RECORD_KEY_CODE && (int)el != START_RECORD_KEY_CODE && (int)el != STOP_PLAYBACK_KEY_CODE && (int)el != START_PLAYBACK_KEY_CODE) 
                    UnregisterHotKey(this.Handle, (int)el);             
            }
            smth = false;
        }

        public int ConvertModifierKey(Keys key) 
        {
            switch (key)
            {
                case Keys.Shift:
                    return Constants.SHIFT;

                case Keys.Control:
                    return Constants.CTRL;

                case Keys.Alt:
                    return Constants.ALT;

                case Keys.LWin:
                    return Constants.WIN;

                case Keys.RWin:
                    return Constants.WIN;

                default:
                    return 0;
            }
        }

        List<Token[]> TokensForPrint = new List<Token[]>();
        List<Interpreter.TreeNode> TreeNodesForPrint = new List<Interpreter.TreeNode>();


        private void run_btn_Click(object sender, EventArgs e)
        {
            inputStr = CodeBox.Text;
            inputStr = regex.Replace(inputStr, @" ");
            splittedStr = inputStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            tokens.Clear();
            trees.Clear();

            try
            {
                for (int i = 0; i < splittedStr.Length; ++i)//partitioning(lexical and syntactic analysis)
                {
                    splittedStr[i] = regex2.Replace(splittedStr[i], @"");

                    tokens.Add(lexer.Tokenize(splittedStr[i]));
                    trees.Add(parser.Construct(tokens[i]));

                    TokensForPrint.Add(lexer.Tokenize(splittedStr[i]));
                    TreeNodesForPrint.Add(parser.Construct(tokens[i]));
                }
            }
            catch(ArgumentException exp)
            {
                PrintError(richTextBox1, exp.Message);
            }
            catch (InvalidOperationException exp)
            {
                PrintError(richTextBox1, exp.Message);
            }

            foreach (Interpreter.TreeNode node in trees)//running code
            {
                try
                {
                    executor.Execute(node);
                }
                catch(ArgumentException exp)
                {
                    PrintError(richTextBox1, exp.Message);
                }
                catch(InvalidOperationException exp)
                {
                    PrintError(richTextBox1, exp.Message);
                }

            }
            
        }

        private void display_tokens_btn_Click(object sender, EventArgs e)
        {
            foreach (Token[] token in tokens)
            {
                for (int i = 0; i < token.Length; ++i)
                {
                    richTextBox2.Text += token[i].ToString() + "    ";
                }
                richTextBox2.Text += "\n";
            }
        }

        private void display_btn_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            foreach (Interpreter.TreeNode el in trees)
            {
                switch (el)
                {
                    case FunctionTreeNode funcTree:
                        richTextBox2.Text += $"-{funcTree.Name}\n";
                        foreach (Interpreter.TreeNode param in funcTree.Parameters)
                        {
                            richTextBox2.Text += $"--{param}\n";
                        }
                        richTextBox2.Text += "\n";
                        break;

                    case CycleTreeNode cycleTree:
                        richTextBox2.Text += $"-{cycleTree.Name}\n";
                        foreach (Interpreter.TreeNode param in cycleTree.Arguments)
                        {
                            richTextBox2.Text += $"--{param}\n";
                        }
                        foreach (Interpreter.TreeNode func in cycleTree.Expressions)
                        {
                            richTextBox2.Text += $"--{(func as FunctionTreeNode).Name}\n";
                            foreach (Interpreter.TreeNode param in (func as FunctionTreeNode).Parameters)
                            {
                                richTextBox2.Text += $"---{param}\n";
                            }
                        }
                        break;
                }
            }
        }

        private void print_functions_btn_Click(object sender, EventArgs e)
        {
            richTextBox2.Text += "StartRecord(\"...\", \"...\") or StartRecord(\"...\")\n"; 
            richTextBox2.Text += "StopRecord(\"...\", \"...\") or StopRecord(\"...\")\n";
            richTextBox2.Text += "StartPlayback(\"...\", \"...\") or StartPlayback(\"...\")\n";
            richTextBox2.Text += "StopPlayback(\"...\", \"...\") or StopPlayback(\"...\")\n";
            richTextBox2.Text += "Start(\"path to file\")\n";
            richTextBox2.Text += "LeftClick(x, y), x & y - coordinates on monitor\n";
            richTextBox2.Text += "RightClick(x, y), x & y - coordinates on monitor\n";
            richTextBox2.Text += "MoveCursorTo(x, y), x & y - coordinates on monitor\n";
            richTextBox2.Text += "PressKey(\"...\")\n";
            richTextBox2.Text += "Repeat(..,..){expression} or Repeat(..){expression}\n";
        }

        public static class Constants
        {
            public const int NOMOD = 0x0000;
            public const int ALT = 1;
            public const int CTRL = 2;
            public const int SHIFT = 4;
            public const int WIN = 8;
            public const int WM_HOTKEY_MSG_ID = 0x0312;
        }

        public void PrintError(RichTextBox text, string error)
        {
            ++errorCounter;
            text.Text += $"{errorCounter}.{error}\n";
        }

    }
}
