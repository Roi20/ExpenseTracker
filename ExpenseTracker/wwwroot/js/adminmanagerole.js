

$(document).on('click','#remove-action-btn', function () {

    var $row = $(this).closest('tr');
    var userId = $row.data('id');
    var email = $row.data('email'); 
    var tr = $row.data('tr');

    $('#removeModeratorModal .modal-body').html
        (`
               <h6>Remove Moderator</h6>
               <span>Are you sure you want to remove &nbsp;<strong>${email} ?</strong>as Moderator ?</span>

        `)

    $('#remove-confirmed-btn').off('click').on('click', function () {

        $.ajax({

            type: 'POST',
            url: '/AdminManageRole/RemoveUserAsModerator',
            data: { userId: userId },
            success: function () {
                console.log('User removed as moderator.');
                $('#removeModeratorModal').modal('hide');
                $row.remove();
            },
            error: function (err) {
                console.log('Unabale to remove user as moderator.', err);
            }

        });

    });

    $('#removeModeratorModal').modal('show');


});


