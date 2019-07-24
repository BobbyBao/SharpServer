@echo off
 
::Proto文件路径
set SOURCE_PATH=.\Protos
 
::Protogen工具路径
set PROTOGEN_PATH=..\..\..\Tools\ProtoGen\protogen.exe
::C#文件生成路径
set TARGET_PATH=.\Models
 
::删除之前创建的文件
del %TARGET_PATH%\*.* /f /s /q
echo -------------------------------------------------------------
 
for /f "delims=" %%i in ('dir /b "%SOURCE_PATH%\*.proto"') do (
    
    echo 转换：%%i to %%~ni.cs
    %PROTOGEN_PATH% %%i --proto_path=%SOURCE_PATH% --csharp_out=%TARGET_PATH%

    
)
 
echo 转换完成
 
pause
