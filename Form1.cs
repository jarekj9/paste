//Paste V1.5

using System;
using System.Windows.Forms;
using Hotkeys;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private void paste_text_from_clipboard(String by_hotkey)
        { 
            string text1 = Clipboard.GetText(TextDataFormat.Text);
            if(by_hotkey=="by_hotkey") SendKeys.Send("%{TAB}");                                    //alt tab
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.4));       //pause 0.4sec, because some chars have been cut

            for (int i = 0; i < text1.Length; i++)          //types char by char, while treating special chars in other way
            {
                char C = text1[i];
                float DELAY = float.Parse(textBox1.Text);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(DELAY / 1000));  //DELAY defined to pause between chars  
                if (C == '(') { SendKeys.Send("{(}"); continue; }
                if (C == ')') { SendKeys.Send("{)}"); continue; }
                if (C == '+') { SendKeys.Send("{+}"); continue; }
                if (C == '^') { SendKeys.Send("{^}"); continue; }
                if (C == '%') { SendKeys.Send("{%}"); continue; }
                if (C == '~') { SendKeys.Send("{~}"); continue; }
                if (C == '{') { SendKeys.Send("{{}"); continue; }
                if (C == '}') { SendKeys.Send("{}}"); continue; }

                SendKeys.Send(C.ToString());
            }
        }

        //PART to take the action on the hotkey press
        private void HandleHotkey()
        {
            paste_text_from_clipboard("by_button");
        }
        //....or action for button press:
        private void button1_Click(object sender, EventArgs e)
        {
            paste_text_from_clipboard("by_hotkey");
        }

        //------------------------------------------------------------------------------------------------------
        //PART for HOTKEY support:
        private Hotkeys.GlobalHotkey ghk;
        
        public Form1()
        {
            InitializeComponent();
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.D1, this);   //Use CTRL and digit 1
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkeys.Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ghk.Register())
                label4.Text="Hotkey CTRL+1 registered.";
            else
                label4.Text = "Hotkey CTRL+1 failed to register";

            //this.Resize += new System.EventHandler(this.Form1_SizeChanged); // always hide to tray when minimized
            notifyIcon1.Click += NotifyIcon1_Click;

            //tray icon context menu
            MenuItem[] menuList = new MenuItem[]{new MenuItem("Open")};
            ContextMenu clickMenu = new ContextMenu(menuList);
            notifyIcon1.ContextMenu = clickMenu;
        }
        //------------------------------------------------------------------------------------------------------
        //System Tray methods
        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)       //if the form is minimized  hide it from the task bar   
        {                                                               //and show the system tray icon (represented by the NotifyIcon control) 
            if (this.WindowState == FormWindowState.Minimized)           //not used at this time
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        //------------------------------------------------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ghk.Unregiser())
                MessageBox.Show("Hotkey failed to unregister!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ghk.Unregiser();                                                   //unregister before registering again
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.D1, this);   //Use CTRL and digit 1
            if (ghk.Register())
                label4.Text = "Hotkey CTRL+1 registered.";
            else
                label4.Text = "Hotkey CTRL+1 failed to register";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ghk.Unregiser();                                                //unregister before registering again
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.D5, this);   //Use CTRL and digit 5
            if (ghk.Register())
                label4.Text = "Hotkey CTRL+5 registered.";
            else
                label4.Text = "Hotkey CTRL+5 failed to register";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ghk.Unregiser();                                                //unregister before registering again
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.OemQuotes, this);   //Use CTRL and quotes
            if (ghk.Register())
                label4.Text = "Hotkey CTRL+\" registered.";
            else
                label4.Text = "Hotkey CTRL+\" failed to register";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ghk.Unregiser();                                                //unregister before registering again
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.Space, this);   //Use CTRL and space
            if (ghk.Register())
                label4.Text = "Hotkey CTRL+space registered.";
            else
                label4.Text = "Hotkey CTRL+space failed to register";
        }

        private void TrayButton_Click(object sender, EventArgs e)
        {
            //hide to system tray button
            Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);

        }
                                                                        
        private void button6_Click(object sender, EventArgs e)          //help button
        {
           System.Windows.Forms.MessageBox.Show(
           "- Use at your own risk\n\n"+
           "- Application registers hotkey(ctrl + 1 or other) - only If this hotkey has not been already taken by other application.\n\n" +
           "- Pressing hotkey or pressing 'PASTE' button will take text from your clipboard and simulate keyboard keypresses to type this text. Effect is the same like typing on keyboard.\n\n" +
           "- Uninstall old version from control panel before installing new.\n\n" +
           "- Attention1: Sometimes some letter may be lost due to system / network performance.\n\n" +
           "- Attention2: Sometimes the destination, where you type, may recognize certain quick keypresses as its own shortcuts / hotkeys. (for example : better disable 'autocomplete' in notepad++ if you paste into notepad++ ).\n\n\n\n" +
           "Version information:\n"+
           "Version 1.5 - added capability to minimize into system tray\n\n"+
           "In case of questions contact me at jaroslaw.jankun@gmail.com\n\n"+
           "Copyright (C) 2018\n\n" +
           "This program is free software: you can redistribute it and/or modify\n" +
           "it under the terms of the GNU General Public License as published by\n" +
           "the Free Software Foundation, either version 3 of the License, or\n" +
           "(at your option) any later version.\n\n" +
           "This program is distributed in the hope that it will be useful,\n" +
           "but WITHOUT ANY WARRANTY; without even the implied warranty of\n" +
           "MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\n" +
           "GNU General Public License for more details.\n\n" +
           "You should have received a copy of the GNU General Public License\n" +
           "along with this program.  If not, see http://www.gnu.org/licenses/"
           );
        }
    }
}
