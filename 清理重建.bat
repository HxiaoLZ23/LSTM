@echo off
echo 正在清理项目...

REM 清理生成文件
for /d /r %%i in (bin,obj) do (
    if exist "%%i" (
        echo 删除 %%i
        rmdir "%%i" /s /q
    )
)

REM 清理NuGet缓存
echo 清理NuGet缓存...
dotnet nuget locals all --clear

REM 还原NuGet包
echo 还原NuGet包...
dotnet restore

echo 清理完成！
echo 现在可以在Visual Studio中重新生成解决方案。
pause 