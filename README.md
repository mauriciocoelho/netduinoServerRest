netduinoServerRest
==================

Netduino server that rest interpreter requests

Samples:

- GET <br />
  api/pin
- Response<br />
  {"Pin10":"False","Pin9":"False","Pin1":"False","Pin11":"False","Pin8":"False","Pin0":"False","Pin12":"False","Pin3":"False","Pin6":"False","Pin5":"False","Pin4":"False","Pin7":"False","Pin2":"False"}.<br />
 
- GET <br />
  api/pin/1<br />
- Response<br />
  {"Pin1":"True"}<br />

- POST<br />
  api/pin<br />
  {"Pin":"1", "Status":true}<br />
- Response<br />
  {"Sucess":true}<br />







