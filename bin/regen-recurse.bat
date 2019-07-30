for /d %%a in (*) do (
    cd %%a 
    call regen
    cd ../
)