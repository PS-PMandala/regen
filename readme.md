#regen#
*A script to convert widgets to the 3.1 framework.*
*Phoenix Mandala*
*7-30-19*
*DEL-24360*

##How to use this program##
**Before you run `regen`, make sure your WTB-template is up to date!**

Simply copy `regen.exe` and `Newtonsoft.Json.dll` to your $PATH-enabled scripts folder. Then, run `regen` from the folder of the widget you wish to regenerate.

Be sure to check that your regeneration is truly successful. This program does not touch any styling!

##What this program does##
`regen` will convert a 3.0 WTB to the newest 3.1 version.
It starts by extracting the account ID and config ID from `widget.json`. It will then get the appropriate config data from `cdn.pricespider.com`. The program will then replace the current `widget.json`'s config property with the extracted data from CDN.
Finally, it will save the JSON and execute `yo widget`.