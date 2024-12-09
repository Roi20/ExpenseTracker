




document.getElementById('admin-sidebar-toggle').addEventListener('click', function () {

    const sidebar = document.getElementById('admin-sidebar-layout');
    const sidebarComputedStyle = window.getComputedStyle(sidebar);

    if (sidebarComputedStyle.opacity === '0' || sidebarComputedStyle.opacity === '') {

        sidebar.style.display = 'block';
        setTimeout(() => {
            sidebar.style.opacity = '1';
            sidebar.style.width = '220px';
        }, 10);
        console.log('show admin sidebar toggle was pressed');

    }
    else {
        sidebar.style.opacity = '0';
        sidebar.style.width = '0';
        console.log('hide admin sidebar toggle was pressed');
    }

});
