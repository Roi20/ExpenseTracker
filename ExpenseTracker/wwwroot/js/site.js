var sideBar = document.getElementById('sideBar');
var toggleButton = document.getElementById('toggle-btn');
var mainLayout = document.getElementById('main-layout');

function sidebarOnclick() {
    const sidebarComputedStyle = window.getComputedStyle(sideBar);
    const screenSize = window.innerWidth;
    const icon = document.getElementById('right-arrow');

    if (sidebarComputedStyle.display === 'none' && screenSize <= 768) {


        sideBar.style.display = 'block';
        mainLayout.style.marginLeft = '-220px';
        toggleButton.style.paddingLeft = '220px'
        console.log('clicked show')

        setTimeout(() => {
            sideBar.style.opacity = '1';
        }, 100)

    }
    else if (sidebarComputedStyle.display === 'none' && screenSize > 768) {

        sideBar.style.display = 'block';
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
            toggleButton.style.paddingLeft = '0';
            mainLayout.style.marginLeft = '0';
        }, 400);
    }
    
};

















