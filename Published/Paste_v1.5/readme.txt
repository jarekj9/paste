1) First, please put program folder in your final location
2) Please uninstall old version if it exist
3) Install using .application file or setup, launch using .application file

If there is security problem with installation on windows 10:
Error like 'Administrator has blocked this program...'
Change these in \HKEY_LOCAL_MACHINE\SOFTWARE\MICROSOFT\.NETFramework\Security\TrustManager\PromptingLevel 
Strings:Internet,MyComputer,LocalInternet,Trusted sites values to Enabled
