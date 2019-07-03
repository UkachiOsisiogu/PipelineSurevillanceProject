/* 
 * MQTT-WebClient example for Web-IO 4.0
*/
var hostname = "localhost";
var port = 1884;
var clientId = "webio4mqttexample"+ Date.now();//this is done to mke the connection unique
//var username = "webclient";
//var password = "Super$icher123";
var subscription = "droneCommands";

mqttClient = new Paho.MQTT.Client(hostname, port, clientId);
mqttClient.onMessageArrived =  MessageArrived;
mqttClient.onConnectionLost = ConnectionLost;
Connect();

/*Initiates a connection to the MQTT broker*/
function Connect(){
	mqttClient.connect({
		onSuccess: Connected,
		onFailure: ConnectionFailed,
		keepAliveInterval: 10,
		//userName: username,
		useSSL: false,
		//password: password	
	});
}

/*Callback for successful MQTT connection */
function Connected() {
  console.log("MQtt Connected");
  mqttClient.subscribe(subscription);
}

/*Callback for failed connection*/
function ConnectionFailed(res) {
	console.log("MQtt Connect failed:" + res.errorMessage);
}

/*Callback for lost connection*/
function ConnectionLost(res) {
  if (res.errorCode !== 0) {
	console.log("MQtt Connection lost:" + res.errorMessage);
	Connect();
  }
}

/*Callback for incoming message processing */
function MessageArrived(message) {
	console.log(message.destinationName +" : " + message.payloadString);
}



