
$


/*
var editBtn = document.querySelectorAll('.update-user');

editBtn.forEach(button => {
    button.addEventListener('click', function () {

        const userId = this.getAttribute('data-id');

        fetch(`/AdminUser/GetUserInfo/${userId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('userId').value = data.id;
                document.getElementById('email').value = data.email;
                document.getElementById('firstname').value = data.firstname;
                document.getElementById('lastname').value = data.lastname;
                document.getElementById('sourceofincome').value = data.sourceofincome;
                document.getElementById('password').value = data.password;
                $('#updateModal').modal('show');
            })
            .catch(error => console.error('Error fetching user data', error));
    });

});

document.getElementById('updateForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const formData = new FormData(this);
    fetch('@Url.Action("UpdateUser", "AdminUser")', {
        method: 'POST',
        body: formData,

        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'X-CSRF-TOKEN': this.querySelector('[name="_RequestVerificationToken"]').value
        }
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                $('#updateModal').modal('hide');
            }
            else {
                alert('Something went wrong')
            }
        })
        .catch(error => console.error('Error updating user:', error));
});
*/