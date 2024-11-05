console.log('SignalR Samplesssss')

var connection = new signalR.HubConnectionBuilder().withUrl('/notification').build();

connection.on('receiveNotification', function (title, message, timeStamp,isRead) {

    let spanMessage = document.createElement('span');
    let li = document.createElement('li');
    let pTitle = document.createElement('p');
    let sendBtn = document.getElementById('send-notif-btn');
    sendBtn.disabled = true;

    pTitle.textContent = title;
    spanMessage.textContent = message;
    li.appendChild(pTitle);
    li.appendChild(spanMessage);
    document.querySelector('.navBar #notif-dropdown ul').appendChild(li);
    console.log('connection on')

});

connection.start().then(function () {
    sendBtn.disabled = false;
    console.log('Connected to signalr')
}).catch(function (err) {
    return console.error(err.toString());
});