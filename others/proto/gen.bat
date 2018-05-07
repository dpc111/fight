del *.cs

for /f "delims=" %%i in ('dir /b /a-d /o-d ".\*.proto"') do (
	echo %%i
    protogen -i:.\%%i -o:.\%%i_tmp
)

ren *.proto_tmp *.cs

pause
