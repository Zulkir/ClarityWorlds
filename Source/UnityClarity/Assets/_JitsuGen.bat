copy "JitsuGen.Generator.cfg" "../../../Build/Debug/JitsuGen.Generator.cfg"
cd ../../../Build/Debug/
JitsuGen.Generator.exe
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe "%CD%\JitsuGenOutput\CS\JitsuGen.Output.csproj"
pause