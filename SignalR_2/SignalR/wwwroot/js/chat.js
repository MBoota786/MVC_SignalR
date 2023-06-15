//______________________________  1st code _____________________________________________
////class Message {
////    constructor(username, text, when) {
////        this.userName = username;
////        this.text = text;
////        this.when = when;
////    }
////}

////// userName is declared in razor page.
////const username = userName;
////const textInput = document.getElementById('messageText');
////const whenInput = document.getElementById('when');
////const chat = document.getElementById('chat');
////const messagesQueue = [];

////document.getElementById('submitButton').addEventListener('click', () => {
////    var currentdate = new Date();
////    when.innerHTML =
////        (currentdate.getMonth() + 1) + "/"
////        + currentdate.getDate() + "/"
////        + currentdate.getFullYear() + " "
////        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
////});

////function clearInputField() {
////    messagesQueue.push(textInput.value);
////    textInput.value = "";
////}

////function sendMessage() {
////    let text = messagesQueue.shift() || "";
////    if (text.trim() === "") return;
    
////    let when = new Date();
////    let message = new Message(username, text);
////    sendMessageToHub(message);
////}

////function addMessageToChat(message) {
////    let isCurrentUserMessage = message.userName === username;

////    let container = document.createElement('div');
////    container.className = isCurrentUserMessage ? "container darker" : "container";

////    let sender = document.createElement('p');
////    sender.className = "sender";
////    sender.innerHTML = message.userName;
////    let text = document.createElement('p');
////    text.innerHTML = message.text;

////    let when = document.createElement('span');
////    when.className = isCurrentUserMessage ? "time-left" : "time-right";
////    var currentdate = new Date();
////    when.innerHTML = 
////        (currentdate.getMonth() + 1) + "/"
////        + currentdate.getDate() + "/"
////        + currentdate.getFullYear() + " "
////        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

////    container.appendChild(sender);
////    container.appendChild(text);
////    container.appendChild(when);
////    chat.appendChild(container);
////}

//______________________________  2nd code _____________________________________________
//"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

////Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var sender = document.getElementById("senderInput").value;
//    var receiver = document.getElementById("receiverInput").value;
//    var message = document.getElementById("messageInput").value;

//    if (receiver != "") {

//        connection.invoke("SendMessageToGroup", sender, receiver, message).catch(function (err) {
//            return console.error(err.toString());
//        });
//    }
//    else {
//        connection.invoke("SendMessage", sender, message).catch(function (err) {
//            return console.error(err.toString());
//        });
//    }


//    event.preventDefault();
//});








//______________________________  3rd code _____________________________________________
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var receiver = document.getElementById("receiverInput").value;
    var message = document.getElementById("messageInput").value;

    if (receiver != "") {
        connection.invoke("SendMessageToGroup", user, receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
    }


    event.preventDefault();
});