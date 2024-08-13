baseModel = {

    getModal: $('#popupModal'),

    getSuccessModal: $('#popup-modal'),

    closeModalBtn: $('#close-btn')
    
}

$(window).on('load', function () {

    baseModel.getModal.css('display', 'block');
    basemodel.getSuccessModal.show();
});

baseModel.closeModalBtn.on('click', function () {
    baseModel.getSuccessModal.hide();
});






