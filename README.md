# paste
Executable and installation notes are in 7z file. Help notes are inside the application itself.

INSTALLATION

#########################################################

1) First, please put program folder in your final location
2) Please uninstall old version if it exist
3) Install using .application file or setup, launch using .application file

If there is security problem with installation on windows 10:
Error like 'Administrator has blocked this program...'
Go to regedit \HKEY_LOCAL_MACHINE\SOFTWARE\MICROSOFT\.NETFramework\Security\TrustManager\PromptingLevel 
and change Strings:Internet,MyComputer,LocalInternet,Trusted sites : values to Enabled

HELP

#########################################################

- Use at your own risk.
- Application registers hotkey(ctrl  1 or other) - only If this hotkey has not been already taken by other application. 
- Pressing hotkey or pressing 'PASTE' button will take text from your clipboard and simulate keyboard keypresses to type this text. Effect is the same like typing on keyboard. 
- With 'Second clipboard' enabled, use: Ctrl+2 to copy, Ctrl+3 to paste 
- You may put config file 'paste_config.txt' in C:\Paste\ folder to load some settings automatically.

- Attention1: Sometimes some letter may be lost due to system / network performance. 
- Attention2: Sometimes the destination, where you type, may recognize certain quick keypresses as its own shortcuts / hotkeys. (for example : better disable 'autocomplete' in notepad++ if you paste into notepad++ ). 

There is also AutoIT version of the program (Paste_autoIT.7z. It starts in tray. Clicking on tray will pause the appplication. With right-click it can be resumed. It pastes clipboard with ctrl+1 and it has second 'pause mechanism' with F9 key. It may have some bugs.

VERSION INFORMATION

#########################################################
- Added 'AutoIT' version  of Paste - v0.9
- Version 1.62 - fixed option to 'hide in tray' with conf file (but there is a bug in Windows7)
- Version 1.61 - changed behaviour of second clipboard
- Version 1.6 - added 'second clipboard' and possiblitity to load some settings from config file
- Version 1.5 - added capability to minimize into system tray
- In case of questions contact me at jaroslaw.jankun@gmail.com

Known issues: 
-on windows 7, if config file is used to start program in system tray, program will still be present in taskbar, but not 'clickable', need to open it from tray then and click hide
