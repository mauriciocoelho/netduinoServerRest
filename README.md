netduinoServerRest
==================

Netduino server that rest interpreter requests


request all pins:

GET http://192.168.1.200:8080/api/pin HTTP/1.1 
Host: 172.31.251.178:8080 
User-Agent: Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.3) 
Gecko/2008101315 Ubuntu/8.10 (intrepid) Firefox/3.0.3 
Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8

response:

HTTP/1.0 200 OK Content-Type: application/js; charset=utf-8 
Content-Length: 199 
Connection: close 
{"Pin10":"False","Pin9":"False","Pin1":"False","Pin11":"False","Pin8":"False","Pin0":"False","Pin12":"False","Pin3":"False","Pin6":"False","Pin5":"False","Pin4":"False","Pin7":"False","Pin2":"False"}
