using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // Native hotkey API
        [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const uint MOD_CTRL = 0x0002;
        const int WM_HOTKEY = 0x0312;

        const int HOTKEY_ID_D1 = 1;
        const int HOTKEY_ID_D2 = 2;
        const int HOTKEY_ID_D3 = 3;
        const int HOTKEY_ID_D5 = 4;
        const int HOTKEY_ID_SPACE = 5;
        const int HOTKEY_ID_OemOpenBrackets = 6;

        string backup_clipboard, temp_clipboard;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, HOTKEY_ID_D1, MOD_CTRL, (uint)Keys.D1);

            label4.Text = "Hotkeys registered.";

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open");
            openMenuItem.Click += (s, args) => NotifyIcon1_Click(s, args);
            contextMenuStrip.Items.Add(openMenuItem);
            notifyIcon1.ContextMenuStrip = contextMenuStrip;

            notifyIcon1.Click += NotifyIcon1_Click;

            read_config();
        }

        private void read_config()
        {
            try
            {
                StreamReader sr = new StreamReader("C:\\Paste\\paste_config.txt");
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Contains("StartInTray=yes"))
                    {
                        ShowInTaskbar = false;
                        TrayButton_Click(null, null);
                    }
                    if (line.Contains("EnableSecondClipboard=yes"))
                    {
                        checkBox1.Checked = true;
                        manage_backup_clipboard();
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e) { Console.WriteLine("Exception: " + e.Message); }
            finally { Console.WriteLine("Done."); }
        }

        private void paste_text_from_clipboard(string by_hotkey, string text)
        {
            if (by_hotkey == "by_button") SendKeys.Send("%{TAB}");
            System.Threading.Thread.Sleep(400);

            if (text != null)
                foreach (char c in text)
                {
                    float delay = float.Parse(textBox1.Text);
                    System.Threading.Thread.Sleep((int)(delay));
                    SendKeys.Send(GetSafeKey(c));
                }
        }

        private string GetSafeKey(char c)
        {
            switch (c)
            {
                case '(': return "{(}";
                case ')': return "{)}";
                case '+': return "{+}";
                case '^': return "{^}";
                case '%': return "{%}";
                case '~': return "{~}";
                case '{': return "{{}";
                case '}': return "{}}";
                default: return c.ToString();
            }
        }


        private void clipboard2_add()
        {
            temp_clipboard = Clipboard.GetText(TextDataFormat.Text);
            SendKeys.Send("^c");
            backup_clipboard = Clipboard.GetText(TextDataFormat.Text);
            Clipboard.SetText(temp_clipboard);
        }

        private void clipboard2_paste()
        {
            paste_text_from_clipboard("by_hotkey", backup_clipboard);
        }

        private void HandleHotkey()
        {
            paste_text_from_clipboard("by_hotkey", Clipboard.GetText(TextDataFormat.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paste_text_from_clipboard("by_button", Clipboard.GetText(TextDataFormat.Text));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                switch (id)
                {
                    case HOTKEY_ID_D1: HandleHotkey(); break;
                    case HOTKEY_ID_D2: clipboard2_add(); break;
                    case HOTKEY_ID_D3: clipboard2_paste(); break;
                    case HOTKEY_ID_D5: HandleHotkey(); break;
                    case HOTKEY_ID_SPACE: HandleHotkey(); break;
                    case HOTKEY_ID_OemOpenBrackets: HandleHotkey(); break;
                }
            }
            base.WndProc(ref m);
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            this.Opacity = 1;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void TrayButton_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);
            this.Opacity = 0.0f;
        }

        private void UnregisterPrimaryHotkeys()
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_D1);
            UnregisterHotKey(this.Handle, HOTKEY_ID_D5);
            UnregisterHotKey(this.Handle, HOTKEY_ID_SPACE);
            UnregisterHotKey(this.Handle, HOTKEY_ID_OemOpenBrackets);
        }

        private void UnregisterSecondaryHotkeys()
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_D2);
            UnregisterHotKey(this.Handle, HOTKEY_ID_D3);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_D1);
            UnregisterHotKey(this.Handle, HOTKEY_ID_D2);
            UnregisterHotKey(this.Handle, HOTKEY_ID_D3);
            UnregisterHotKey(this.Handle, HOTKEY_ID_D5);
            UnregisterHotKey(this.Handle, HOTKEY_ID_SPACE);
            UnregisterHotKey(this.Handle, HOTKEY_ID_OemOpenBrackets);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnregisterPrimaryHotkeys();
            if (RegisterHotKey(this.Handle, HOTKEY_ID_D1, MOD_CTRL, (uint)Keys.D1))
            {
                label4.Text = "Hotkey CTRL+1 registered.";
            }
            else
            {
                label4.Text = "Failed to register hotkey CTRL+1. It may already be in use.";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UnregisterPrimaryHotkeys();
            if (RegisterHotKey(this.Handle, HOTKEY_ID_D5, MOD_CTRL, (uint)Keys.D5))
            {
                label4.Text = "Hotkey CTRL+5 registered.";
            }
            else
            {
                label4.Text = "Failed to register CTRL+5.\nIt may already be in use.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UnregisterPrimaryHotkeys();
            if (RegisterHotKey(this.Handle, HOTKEY_ID_OemOpenBrackets, MOD_CTRL, (uint)Keys.OemOpenBrackets))
            {
                label4.Text = "Hotkey CTRL+[ registered.";
            }
            else
            {
                label4.Text = "Failed to register CTRL+[.\nIt may already be in use.";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UnregisterPrimaryHotkeys();
            if (RegisterHotKey(this.Handle, HOTKEY_ID_SPACE, MOD_CTRL, (uint)Keys.Space))
            {
                label4.Text = "Hotkey CTRL+space registered.";
            }
            else
            {
                label4.Text = "Failed to register CTRL+space.\nIt may already be in use.";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jarekj9/paste");
        }

        private void manage_backup_clipboard()
        {
            UnregisterSecondaryHotkeys();
            if (checkBox1.Checked)
            {
                if (RegisterHotKey(this.Handle, HOTKEY_ID_D2, MOD_CTRL, (uint)Keys.D2) && RegisterHotKey(this.Handle, HOTKEY_ID_D3, MOD_CTRL, (uint)Keys.D3))
                {
                    SecClipLabel.Text = "Hotkeys CTRL+2 and CTRL+3 registered\nfor second clipboard.";
                }
                else
                {
                    SecClipLabel.Text = "Failed to register CTRL+2 and CTRL+3.\nThey may already be in use.";
                }
            }
            else
            {
                SecClipLabel.Text = "CTRL+2 and CTRL+3 not registered";
            }
        }

        private void checkBox1_Clicked(object sender, EventArgs e)
        {
            manage_backup_clipboard();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("- Use at your own risk\n");
            sb.AppendLine("- Application registers hotkey(ctrl + 1 or other) - only If this hotkey has not been already taken by other application.\n");
            sb.AppendLine("- Pressing hotkey or pressing 'PASTE' button will take text from your clipboard and simulate keyboard keypresses to type this text. Effect is the same like typing on keyboard.\n");
            sb.AppendLine("- With 'Second clipboard' enabled, use: Ctrl+2 to copy, Ctrl+3 to paste\n");
            sb.AppendLine("- Uninstall old version from control panel before installing new.\n");
            sb.AppendLine("- You may change default settings if you put paste_config.txt file in folder C:\\Paste\\\n");
            sb.AppendLine("- Attention1: Sometimes some letter may be lost due to system / network performance.\n");
            sb.AppendLine("- Attention2: Sometimes the destination, where you type, may recognize certain quick keypresses as its own shortcuts / hotkeys. (for example : better disable 'autocomplete' in notepad++ if you paste into notepad++ ).\n");
            sb.AppendLine();
            sb.AppendLine("Version information:");
            sb.AppendLine("Version 2.0, 23.06.2025 - upgraded project from .net framework to .net8");
            sb.AppendLine("Version 1.62 - restored option to hide in Tray with config file (but there is a bug in Windows7)");
            sb.AppendLine("Version 1.61 - changed behaviour of backup clipboard");
            sb.AppendLine("Version 1.6 - added Second Clipboard and config file\n");
            sb.AppendLine("In case of questions contact me at jaroslaw.jankun@gmail.com\n");
            sb.AppendLine("Copyright (C) 2018\n");
            sb.AppendLine("This program is free software: you can redistribute it and/or modify");
            sb.AppendLine("it under the terms of the GNU General Public License as published by");
            sb.AppendLine("the Free Software Foundation, either version 3 of the License, or");
            sb.AppendLine("(at your option) any later version.\n");
            sb.AppendLine("This program is distributed in the hope that it will be useful,");
            sb.AppendLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
            sb.AppendLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the");
            sb.AppendLine("GNU General Public License for more details.\n");
            sb.AppendLine("You should have received a copy of the GNU General Public License");
            sb.AppendLine("along with this program.  If not, see http://www.gnu.org/licenses/");
            MessageBox.Show(sb.ToString());
        }
    }
}
