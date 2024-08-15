baseModel = {

    getModal: $('#popupModal'),

    getSuccessModal: $('#popup-modal'),

    closeModalBtn: $('#close-btn')
    
}

$(window).on('load', function () {

    baseModel.getModal.css('display', 'block');
    baseModel.getSuccessModal.show();
});

baseModel.closeModalBtn.on('click', function () {
    baseModel.getSuccessModal.hide();
});



/*Currency Formatting
$('#inputAmount').on('input', function () {

    var input = $(this);
    input.val(formatCurrency(input.val()));

});

function formatCurrency(value) {
    value = value.replace(/\D/g, ''); // Remove non-digit characters
    return '$' + value.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

$('#inputAmount').on('input', function () {
    var value = $(this).val();
    var formattedValue = parseFloat(value).toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    $(this).val(formattedValue);
});*/