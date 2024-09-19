/*
$('#menu-toggle').click(function () {
    // Toggle the active class to animate the icon
    $(this).toggleClass('active');

    // Toggle the active class to show/hide the sidebar
    $('#side-bar').toggleClass('active');

    // Toggle icon between menu and close
    var icon = $('#icon');
    if (icon.hasClass('fa-bars')) {
        icon.removeClass('fa-bars').addClass('fa-times'); // Switch to close icon
    } else {
        icon.removeClass('fa-times').addClass('fa-bars'); // Switch back to menu icon
    }
});
*/


var sideBar = document.getElementById('sideBar');
var toggleButton = document.getElementById('toggle-btn');
var mainLayout = document.getElementById('main-layout');

function sidebarOnclick() {


    if (sideBar.style.display == 'none') {

        sideBar.style.display = 'block';
        mainLayout.style.marginLeft = '-220px'
        console.log('clicked show')

        setTimeout(() => {
            sideBar.style.opacity = '1';
        }, 100)
    }
    else {

        sideBar.style.opacity = '0';
        console.log('clicked hide');

        setTimeout(() => {
            sideBar.style.display = 'none';
            mainLayout.style.marginLeft = '0';
        }, 400);

        
    }



};








