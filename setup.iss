[Setup]
AppName=FOMS 2.0
AppVersion=2.1
AppPublisher=LS
DefaultDirName={pf}\FOMS2
DefaultGroupName=FOMS2
OutputDir=D:\FOMS2\Installer
OutputBaseFilename=FOMS2_Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=D:\FOMS2\FOMS2\app.ico

[Languages]
Name: "korean"; MessagesFile: "compiler:Languages\Korean.isl"

[Tasks]
Name: "desktopicon"; Description: "바탕화면에 아이콘 생성"; GroupDescription: "추가 아이콘:"

[Files]
; 실행 파일
Source: "D:\FOMS2\FOMS2\bin\Release\FOMSSubmarine.exe"; DestDir: "{app}"; Flags: ignoreversion
; DLL 파일
Source: "D:\FOMS2\FOMS2\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
; 설정 파일
Source: "D:\FOMS2\FOMS2\bin\Release\*.config"; DestDir: "{app}"; Flags: ignoreversion
; 아이콘
Source: "D:\FOMS2\FOMS2\app.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\FOMS Submarine"; Filename: "{app}\FOMSSubmarine.exe"
Name: "{group}\FOMS Submarine 제거"; Filename: "{uninstallexe}"
Name: "{commondesktop}\FOMS Submarine"; Filename: "{app}\FOMSSubmarine.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\FOMSSubmarine.exe"; Description: "FOMS Submarine 실행"; Flags: nowait postinstall skipifsilent