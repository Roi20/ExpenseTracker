

//const { signalR } = require("~/lib/microsoft/signalr/dist/browser/signalr.js");
var connection = new signalR.HubConnectionBuilder().withUrl('/notificationHub').build();
let sendBtn = document.getElementById('send-notif-btn');
sendBtn.disabled = true;

connection.on('receiveNotification', function (title, message, timeStamp,isRead) {
   
    let spanTitle = document.createElement('span');
    let spanMessage = document.createElement('span');
    let spanTimeStamp = document.createElement('span');
    let spanIsRead = document.createElement('span');
    let div = document.createElement('div')
     /*
    let li = document.createElement('li');
    let pTitle = document.createElement('p');
    let ul = document.createElement('ul');
    pTitle.textContent = title;
    spanMessage.textContent = message;
    li.appendChild(pTitle);
    li.appendChild(spanMessage);
    ul.appendChild(li);
    */
    spanTitle.textContent = title;
    spanMessage.textContent = message;
    spanTimeStamp.textContent = timeStamp;
    spanIsRead.textContent = isRead

    div.appendChild(spanTitle);
    div.appendChild(spanMessage);
    div.appendChild(spanTimeStamp);
    div.appendChild(spanIsRead);
    document.querySelector('#notif-dropdown').appendChild(div);
    


    console.log('connection on')
    
});

connection.start().then(function () {
    sendBtn.disabled = false;
    console.log('Connected to signalr')
}).catch(function (err) {
    return console.error(err.toString());
});