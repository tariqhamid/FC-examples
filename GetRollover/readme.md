With this program you can get rollover/swap rates for all symbols in your account,

To run it, download all files in this folder: 

https://github.com/FXCMAPI/FC-examples/tree/master/GetRollover/bin

edit file rollover1.cmd, the file looks like:

-l Username -p Password -u http://www.fxcorporate.com/Hosts.jsp -c Demo 

you need to change few parameters:

Username – your login

Password  -  login password

Demo – for demo account should be Demo, for real account change to Real

save the changes and run rollover1.cmd


the output will be in two files format:

1.	Text file – rollover.txt, Symbol, Roll Sell, Roll Buy: 

EUR/USD,0.81,-1.86

USD/JPY,-1.17,0.51

GBP/USD,0.48,-1.11

USD/CHF,-2.25,1.05

2.	XML file – follover.xml
<rate>
<symbol>EUR/USD</symbol>
<rolbuy>-1.86</rolbuy>
<rolsell>0.81</rolsell>
</rate>
<rate>
<symbol>USD/JPY</symbol>
<rolbuy>0.51</rolbuy>
<rolsell>-1.17</rolsell>
</rate>

This example program included with full source code in C++, using Forex Connect API 1.4.1, Win32.

Daniel
