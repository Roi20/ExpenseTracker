
var connection = new signalR.HubConnectionBuilder().withUrl('/notificationHub').build();


connection.on('ReceiveNotification', function (title, message, timeStamp, isRead) {
    document.getElementById('notif-bell').classList.add('has-notifications');

    console.log('Notification Received.')
    
});

connection.start().then(function () {

    document.getElementById('notif-button').addEventListener('click', function () {
        document.getElementById('notif-bell').classList.remove('has-notifications');
    });

    console.log('Connected to signalr')
}).catch(function (err) {
    return console.error(err.toString());
});




