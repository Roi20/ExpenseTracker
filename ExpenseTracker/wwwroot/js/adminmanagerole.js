

$('a[data-bs-toggle=tooltip]').on('click', function () {

    var $row = $(this);
    var userId = $row.data('id');
    var email = $row.data('email'); 

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


