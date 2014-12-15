netduinoServerRest
==================

Netduino server that rest interpreter requests

Samples:

- GET <br />
  api/pin
- Response
  {"Pin10":"False","Pin9":"False","Pin1":"False","Pin11":"False","Pin8":"False","Pin0":"False","Pin12":"False","Pin3":"False","Pin6":"False","Pin5":"False","Pin4":"False","Pin7":"False","Pin2":"False"}.
 
- GET 
  api/pin/1
- Response
  {"Pin1":"True"}

- POST
  api/pin
  {"Pin":"1", "Status":true}
- Response
  {"Sucess":true}







