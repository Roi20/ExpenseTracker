/*var menuToggle = document.getElementById('menu-toggle');
var sideBar = document.getElementById('side-bar');
var icon = document.getElementById('icon');
var main = document.getElementById('main');


menuToggle.addEventListener('click', function () {
    sideBar.classList.toggle('d-none');
    if (sideBar.contains('d-none'))
    {
        sideBar.classList.remove('d-none');
        main.classList.remove('col-md-10');
        main.classList.add('col-12');
        icon.classList.remove("fa-bars");
        icon.classList.add("fa-xmark");
    }
    else {
        main.classList.remove("col-12");
        main.classList.add("col-md-10");
        icon.classList.remove("fa-xmark");
        icon.classList.add("fa-bars");
    }


}); 
    */

    $('#menu-toggle').click(function () {
        // Toggle sidebar visibility
        $('#side-bar').toggleClass('d-none');
        $('#mainContainer').toggleClass('col-md-12 col-md-10');

        // Toggle icon between menu and close
        var icon = $('#icon');
        if (icon.hasClass('fa-bars')) {
            icon.removeClass('fa-bars').addClass('fa-times'); // Switch to close icon
        } else {
            icon.removeClass('fa-times').addClass('fa-bars'); // Switch back to menu icon
        }
    });
    



