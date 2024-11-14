

var connection = new signalR.HubConnectionBuilder().withUrl('/notificationHub').build();


connection.on('ReceiveNotification', function (title, message, timeStamp, isRead) {
    location.reload();
    document.getElementById('notif-bell').classList.add('has-notifications');
    localStorage.setItem('hasNotifications', 'true');
    console.log('Notification Received.')
    
});


connection.on('NotificationCleared', function () {
    document.getElementById('notif-bell').classList.remove('has-notifications');
    localStorage.setItem('hasNotifications', 'false');
    console.Log('Notification Cleared');
});

connection.on('ReceiveNotificationIndicator', function () {
    document.getElementById('notif-bell').classList.add('has-notifications');
    localStorage.setItem('hasNotifications', 'true');
    console.log('Receive Notification Indicator');
});


connection.start().then(function () {

    document.getElementById('notif-button').addEventListener('click', function () {
        connection.invoke('ClearNotification').catch(function (err) {
            return console.error('Error Clearing Notification: ', err.toString());
        });
    });

    console.log('Connected to signalr')
}).catch(function (err) {
    return console.error(err.toString());
});


window.addEventListener('load', () => {
    const hasNotifications = localStorage.getItem('hasNotifications');
    if (hasNotifications === 'true') {
        document.getElementById('notif-bell').classList.add('has-notifications');
    }
});



