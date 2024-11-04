const { signalR } = require("../lib/microsoft/signalr/dist/browser/signalr");

var connection = new signalR.HubConnectionBuilder().withUrl('/notification').build();

connection.on('receiveNotification', function (title, message, timeStamp,isRead) {

    let spanMessage = document.createElement('span');
    let li = document.createElement('li');
    let pTitle = document.createElement('p');

    pTitle.textContent = title;
    spanMessage.textContent = message;
    li.appendChild(pTitle);
    li.appendChild(spanMessage);
    document.querySelector('.navBar #notif-dropdown ul').appendChild(li);

});

connect.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});