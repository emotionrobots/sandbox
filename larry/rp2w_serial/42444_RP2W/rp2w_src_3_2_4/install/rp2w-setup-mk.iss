; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{65EB0FF7-9F1E-4E09-B4B6-3880E0E6D456}
AppName=RP2W
AppVerName=RP2W 2.9.5
AppPublisher=SuperDroid Robots, Inc.
AppPublisherURL=http://superdroidrobots.com/
AppSupportURL=http://superdroidrobots.com/
AppUpdatesURL=http://superdroidrobots.com/
DefaultDirName={pf}\SuperDroid Robots\RP2W
DefaultGroupName=RP2W
AllowNoIcons=yes
LicenseFile=d:\Firmadat\Projekty\RobotiRP2W\install\licence.txt
InfoBeforeFile=d:\Firmadat\Projekty\RobotiRP2W\install\before.txt
OutputDir=d:\Firmadat\Projekty\RobotiRP2W\install\build
OutputBaseFilename=rp2w_setup
SetupIconFile=d:\Firmadat\Projekty\RobotiRP2W\install\SDR.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\RP2W.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\AxInterop.AVSIconfClientLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\AxInterop.AVSIconfServerLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\devcon.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\help\help.html"; DestDir: "{app}\help"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\Interop.AVSIconfClientLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\release\Interop.AVSIconfServerLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\defaults\C\SDR_Default.dat"; DestDir: "{app}\Data"; Flags: ignoreversion
Source: "c:\Program Files\AVSPEED Applications\iConf SDK\bin\AVSIconfClientLib.ocx"; DestDir: "{sys}"; Flags: regserver
Source: "c:\Program Files\AVSPEED Applications\iConf SDK\bin\AVSIconfServerLib.ocx"; DestDir: "{sys}"; Flags: regserver
Source: "c:\Program Files\AVSPEED Applications\iConf SDK\bin\AVSIconfClientLib.lic"; DestDir: "{sys}"
Source: "c:\Program Files\AVSPEED Applications\iConf SDK\bin\AVSIconfServerLib.lic"; DestDir: "{sys}"
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\dlls\avscodec.dll"; DestDir: "{sys}"; Flags: sharedfile
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\dlls\avsvideoext.dll"; DestDir: "{sys}"; Flags: sharedfile
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\dlls\avbox.dll"; DestDir: "{sys}"; Flags: sharedfile
Source: "d:\Firmadat\Projekty\RobotiRP2W\install\dlls\virtualw.dll"; DestDir: "{sys}"; Flags: sharedfile
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\RP2W"; Filename: "{app}\RP2W.exe"
Name: "{group}\{cm:UninstallProgram,RP2W}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\RP2W"; Filename: "{app}\RP2W.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\RP2W"; Filename: "{app}\RP2W.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\RP2W.exe"; Description: "{cm:LaunchProgram,RP2W}"; Flags: nowait postinstall skipifsilent

















