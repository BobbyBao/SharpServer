@echo off
 
::Proto�ļ�·��
set SOURCE_PATH=.\Protos
 
::Protogen����·��
set PROTOGEN_PATH=..\..\..\Tools\ProtoGen\protogen.exe
::C#�ļ�����·��
set TARGET_PATH=.\Models
 
::ɾ��֮ǰ�������ļ�
del %TARGET_PATH%\*.* /f /s /q
echo -------------------------------------------------------------
 
for /f "delims=" %%i in ('dir /b "%SOURCE_PATH%\*.proto"') do (
    
    echo ת����%%i to %%~ni.cs
    %PROTOGEN_PATH% %%i --proto_path=%SOURCE_PATH% --csharp_out=%TARGET_PATH%

    
)
 
echo ת�����
 
pause
