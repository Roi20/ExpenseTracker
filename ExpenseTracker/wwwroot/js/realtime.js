
var connection = new signalR.HubConnectionBuilder().withUrl('/notificationHub').build();


connection.on('receiveNotification', function (title, message, timeStamp,isRead) {
 

    console.log('Notification Received.')
    
});

connection.start().then(function () {
    console.log('Connected to signalr')
}).catch(function (err) {
    return console.error(err.toString());
});




