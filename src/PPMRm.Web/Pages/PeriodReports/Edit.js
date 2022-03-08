$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');


    var addProductModal = new abp.ModalManager({
        viewUrl: '/PeriodReports/AddProductModal',
        modalClass: 'ProductInfo' //Matches to the abp.modals.ProductInfo
    });
    var editProductModal = new abp.ModalManager({
        viewUrl: '/PeriodReports/EditProductModal',
        modalClass: 'ProductInfo' //Matches to the abp.modals.ProductInfo
    });
    var addShipmentModal = new abp.ModalManager(abp.appPath + 'PeriodReports/AddShipmentModal');
    var csModal = new abp.ModalManager(abp.appPath + 'PeriodReports/CSModal');

    $('.btn-add-shipment').click(function (e) {
        e.preventDefault();
        var params = {
            id: $(this).data('product-id'),
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id')
        };
        addShipmentModal.open(params);
    });

    $("#edit-cs-updates").click(function (e) {
        e.preventDefault();
        var params = { id: $(this).data('id') };
        csModal.open(params);
    });

    $(".btn-add-program-product").click(function (e) {
        e.preventDefault();
        var params = {
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id')
        };
        addProductModal.open(params);
    });

    $(".btn-edit-product").click(function (e) {
        e.preventDefault();
        var params = {
            id: $(this).data('id'),
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id')
        };
        editProductModal.open(params);
    });

    addProductModal.onResult(function () {
        location.reload();
    });

    var inputAction = function (requestData, dataTableSettings) {
        return {
            id: "AGO-202112",
            programId: "4"
        };
    };


});

abp.modals.ProductInfo = function () {

    function initModal(modalManager, args) {
        var $modal = modalManager.getModal();
        var $form = modalManager.getForm();

        $modal.find("#SOHLevels").multiselect({
            selectAllValue: 'multiselect-all',
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            buttonWidth: '450px'
        });
    };

    return {
        initModal: initModal
    };
};