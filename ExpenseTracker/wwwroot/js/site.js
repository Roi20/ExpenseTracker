var sideBar = document.getElementById('sideBar');
var toggleButton = document.getElementById('toggle-btn');
var mainLayout = document.getElementById('main-layout');





function sidebarOnclick() {
    const sidebarComputedStyle = window.getComputedStyle(sideBar);

    if (sidebarComputedStyle.display === 'none') {


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












