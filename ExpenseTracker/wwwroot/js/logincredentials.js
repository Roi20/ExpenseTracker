var usernameInput = document.getElementById('username-input');
var passwordInput = document.getElementById('password-input');
var loginForm = document.getElementById('account');


document.getElementById('admin-credential').addEventListener('click', function () {

    usernameInput.value = "admin@admin.com";
    passwordInput.value = "@Password123";
    loginForm.submit();

});


document.getElementById('user-credential').addEventListener('click', function () {

    usernameInput.value = "joenorgg25@gmail.com";
    passwordInput.value = "@Password123";
    loginForm.submit();
});