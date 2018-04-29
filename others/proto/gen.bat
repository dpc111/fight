for /f "delims=" %%i in ('dir /b /a-d /o-d ".\*.proto"') do (
	echo %%i
    protogen --csharp_out=.\ %%i
)

pause
