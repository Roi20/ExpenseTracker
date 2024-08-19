
var toggle = document.getElementById('sidebar-toggle');
var hideSidebar = document.getElementById('hide-sidebar');

toggle.addEventListener('click', function () {
    var sidebar = document.getElementById('side-bar');
    var main = document.getElementById('main');
    var wrapper = document.getElementById('sidebar-wrapper');

    if (sidebar.classList.contains('d-none')) {
        sidebar.classList.remove('d-none');
        sidebar.classList.add('d-block');
        main.classList.remove('col-11');
        main.classList.add('col-md-10');
        wrapper.classList.add('d-none');
       
    } else {
        sidebar.classList.remove('d-block');
        sidebar.classList.add('d-none');
        main.classList.remove('col-md-10');
        main.classList.add('col-11');
        
    }
});

hideSidebar.addEventListener('click', function () {
    var sidebar = document.getElementById('side-bar');
    var main = document.getElementById('main');
    var wrapper = document.getElementById('sidebar-wrapper');

    if (sidebar.classList.contains('d-block')) {
        sidebar.classList.remove('d-block');
        wrapper.classList.add('col-1')
        wrapper.classList.add('d-block')
    
       // sidebar.classList.add('d-none');
       // main.classList.remove('col-11');
       // main.classList.add('col-md-11');
        //wrapper.classList.add('col-1');
       // wrapper.classList.add('d-block');


    } else {
        sidebar.classList.remove('d-block');
        sidebar.classList.add('d-none');
        main.classList.remove('col-md-10');
        main.classList.add('col-11');

    }
});

