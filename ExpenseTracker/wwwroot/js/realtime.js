

var connection = new signalR.HubConnectionBuilder().withUrl('/notificationHub').build();


connection.on('ReceiveNotification', function (title, message, timeStamp, isRead) {

    var newRow = $('<tr>')
        .addClass(isRead ? 'read' : 'unread')
        .attr('data-caller', 'tr-inboxModal')
        .attr('data-title', title)
        .attr('data-message', message)
        .attr('data-timestamp', timeStamp)
        .attr('data-isread', isRead);
  
    newRow.append(`<td>${title}</td>`);
    newRow.append(`<td>${message}</td>`);
    newRow.append(`<td>${timeStamp.toString('g')}</td>`);
    newRow.append('<td>' + (isRead ? 'Read' : 'Unread') + '</td>');

    var actionTd = $('<td>');
    var actionButton = $('<a>Pending</a>')
        .addClass('text-secondary')
        .attr('data-bs-toggle', 'tooltip')
        .attr('data-bs-placement', 'left')
        .attr('title', 'Pending')
        .attr('id', 'delete-message-btn');

    var newDiv = $('<div>').addClass('action-button');

    newDiv.append(actionButton);
    actionTd.append(newDiv);
    newRow.append(actionTd);

    $('#notificationTableBody').prepend(newRow);
    console.log('New Row Appended');

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



