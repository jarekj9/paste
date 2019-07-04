/*

    private void paste_text_from_clipboard(String by_hotkey, String text)
    {
        if (by_hotkey == "by_button") SendKeys.Send("%{TAB}");              //alt tab if launched by button
        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.4));       //pause 0.4sec, because some chars have been cut

        if (text != null)
            for (int i = 0; i < text.Length; i++)          //types char by char, while treating special chars in other way
            {
                char C = text[i];
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



    */