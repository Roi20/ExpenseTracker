

$('tr[data-caller="tr-inboxModal"]').on('click', function (e) {
    var $row = $(this);
    var notificationId = $row.data('id');
    var title = $row.data('title');
    var message = $row.data('message');
    var timeStamp = $row.data('timestamp');
    var isRead = $row.data('isread');
    var controllerName = $row.data('controller');
    var actionName = 'MarkAsread';
    var dynamicUrl = `/${controllerName}/${actionName}`;

    if ($(e.target).closest('td').index() === $row.children().length - 1) {

        $('#deleteMessageModal .modal-body').html
            (`
               <h6>Delete Message</h6>
               <span>Are you sure you want to delete &nbsp;<strong>${title} ? </strong> </span>

           `)
        console.log('delete was clicked');

        $('#deleteMessageModal').modal('show');

        $('#delete-confirmed-btn').off('click').on('click', function () {

            var confirmedDeleteAction = 'ConfirmedDeleteNotification';

            $.ajax({

                type: 'POST',
                url: `/${controllerName}/${confirmedDeleteAction}`,
                data: { id: notificationId },
                success: function () {
                    console.log('Message deleted successfully.');
                    $('#deleteMessageModal').modal('hide');
                    $row.remove();
                },
                error: function (error) {
                    console.error('Error deleting the message.', error);
                }

            });

        });

        return;
    }


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

    $('#inboxModal2').modal('show');
});




   