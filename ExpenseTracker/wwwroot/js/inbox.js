

$('tr[data-bs-toggle="modal"]').on('click', function () {
    var $row = $(this);
    var notificationId = $row.data('id');
    var title = $row.data('title');
    var message = $row.data('message');
    var timeStamp = $row.data('timestamp');
    var isRead = $row.data('isread');
    var controllerName = $row.data('controller');
    var actionName = 'MarkAsread';
    var dynamicUrl = `/${controllerName}/${actionName}`;


    $.ajax({

        type: 'POST',
        url: dynamicUrl,
        data: { id: notificationId },
        success: function () {
            console.log('Notification marked as read.')
            $row.removeClass('unread').addClass('read');
            $row.find('.status').text('Read');
        },
        error: function (error) {
            console.error('Error marking notification as read.', error);
        }


    });

    $('#inboxModal2 .modal-body').html
        (`
           <h6>${title}</h6>
           <span>${message}</span>
           <p>${timeStamp}</p>
        `);
});
