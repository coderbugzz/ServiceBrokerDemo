// wwwroot/js/signalr.js

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7218/apiHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(() => {
    console.log("SignalR Connected");
    //if (connection.state === signalR.HubConnectionState.Connected) {
    //    // Safe to send data
    //    connection.invoke("SendDataToClients", "Hello from client")
    //        .catch(err => console.error(`Error sending data: ${err}`));
    //} else {
    //    console.warn("Connection is not in the 'Connected' state. Data not sent.");
    //    // Optionally, you can try to re-establish the connection or handle accordingly
    //}


}).catch(err => console.error(err));

connection.on("ReceiveData", (message) => {
    console.log(message);

    var jsonObj = JSON.parse(message);
    console.log(jsonObj.data);

    var table = document.getElementById("tableData");
    var row = document.createElement("tr")

    var c1 = document.createElement("td")
    var c2 = document.createElement("td")

    c1.innerText = jsonObj.data.userid;
    c2.innerText = jsonObj.data.message;

    row.appendChild(c1);
    row.appendChild(c2);

    table.appendChild(row)

});

