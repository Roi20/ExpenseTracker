baseModel = {

    getModal: $('#popupModal'),

    getSuccessModal: $('#popup-modal'),

    closeModalBtn: $('#close-btn'),

    getSelect: $('#inputGroupSelect02')
    
}

$(window).on('load', function () {

    baseModel.getModal.css('display', 'block');
    baseModel.getSuccessModal.show();
});

baseModel.closeModalBtn.on('click', function () {
    baseModel.getSuccessModal.hide();
});

/*Sidebar Toggle Button*/



function submitForm() {
    document.getElementById('sortForm').submit();
}



