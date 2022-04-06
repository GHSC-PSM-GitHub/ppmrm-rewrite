$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    // EditProgramProduct page
    $("#Product_SOHLevels").multiselect({
        selectAllValue: 'multiselect-all',
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 200,
        buttonWidth: '450px'
    });

    var addProductModal = new abp.ModalManager({
        viewUrl: '/PeriodReports/AddProductModal',
        modalClass: 'ProductInfo' //Matches to the abp.modals.ProductInfo
    });
    var editProductModal = new abp.ModalManager({
        viewUrl: '/PeriodReports/EditProductModal',
        modalClass: 'ProductInfo' //Matches to the abp.modals.ProductInfo
    });
    var addShipmentModal = new abp.ModalManager(abp.appPath + 'PeriodReports/AddShipmentModal');
    var editShipmentModal = new abp.ModalManager(abp.appPath + 'PeriodReports/EditShipmentModal');
    var csModal = new abp.ModalManager(abp.appPath + 'PeriodReports/CSModal');

    $("#btn-save-program-product").click(function () {
        $("#frm-program-product").submit();
    });

    $('.btn-add-shipment').click(function (e) {
        e.preventDefault();
        var params = {
            id: $(this).data('product-id'),
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id')
        };
        addShipmentModal.open(params);
    });

    $('.btn-edit-shipment').click(function (e) {
        e.preventDefault();
        var params = {
            id: $(this).data('product-id'),
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id'),
            shipmentId: $(this).data('shipment-id')
        };
        editShipmentModal.open(params);
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

    editProductModal.onResult(function () {
        location.reload();
    });

    addShipmentModal.onResult(function () {
        location.reload();
    });

    editShipmentModal.onResult(function () {
        location.reload();
    });

    $(".btn-report-status").click(function (e) {
        e.preventDefault();
        var action = $(this).data('action');
        var id = $(this).data('id');
        abp.message.confirm('Are you sure you would like to ' + action + ' this report?')
            .then(function (confirmed) {
                if (confirmed) {
                    switch (action) {
                        case 'open':
                            console.log("opening report" + id);
                            pPMRm.periodReports.periodReport.open(id)
                                .then(function () {
                                    abp.message.success('Period report successfully opened!').then(function () {
                                        location.reload();
                                    });
                                });
                            break;
                        case 'reopen':
                            console.log("Re-opening report");
                            pPMRm.periodReports.periodReport.reopen(id)
                                .then(function () {
                                    abp.message.success('Period report successfully reopened!').then(function () {
                                        location.reload();
                                    });
                                });
                            break;
                        case 'finalize':
                            console.log("Finalizing report");
                            pPMRm.periodReports.periodReport.markAsFinal(id)
                                .then(function () {
                                    abp.message.success('Period report successfully marked as Final!').then(function () {
                                        location.reload();
                                    });
                                });
                            break;
                        case 'close':
                            console.log("Closing report");
                            pPMRm.periodReports.periodReport.close(id)
                                .then(function () {
                                    abp.message.success('Period report successfully closed!').then(function () {
                                        location.reload();
                                    });
                                });
                            break;
                        default:
                            console.log("Other action: ERROR!");
                            abp.message.error('Unknown action');
                            break;
                    }
                }
                else {
                    console.log("canceled");
                }
                //pPMRm.periodReports.periodReport.deleteShipment(id, shipmentId)
                //    .then(function () {
                //        abp.message.success('Shipment successfully deleted!').then(function () {
                //            location.reload();
                //        });
                //    });
            });
    });

    $(".btn-remove-shipment").click(function (e) {
        e.preventDefault();
        var shipmentId = $(this).data('id');
        var id = $(this).data('period-report-id');
        abp.message.confirm('Are you sure to delete this Shipment? This action is irreversible.')
            .then(function (confirmed) {
                if (confirmed) {
                    pPMRm.periodReports.periodReport.deleteShipment(id, shipmentId)
                        .then(function () {
                            abp.message.success('Shipment successfully deleted!').then(function () {
                                location.reload();
                            });
                        });
                }
            });
    });

    function compute() {
        var soh = $("#Product_SOH").val();
        var amc = $("#Product_AMC").val();
        if (amc != 0) {
            var mos = soh / amc;
            $('#Product_MOS').val(mos.toFixed(2));
        }
        else
            $("#Product_MOS").val(0);
    }

    function toggleSourceOfConsumption() {
        var source = $("#Product_SourceOfConsumption").val();
        if (source == "Other") {
            $("#Product_OtherSourceOfConsumption").parents('.form-group').show();
            console.log("show text box");
        }
        else {
            $("#Product_OtherSourceOfConsumption").val("");
            $("#Product_OtherSourceOfConsumption").parents('.form-group').hide();
            console.log("clear and hide");
        }
    }

    $("#Product_SOH").change(compute);
    $("#Product_AMC").change(compute);
    $("#Product_SourceOfConsumption").change(toggleSourceOfConsumption);
    toggleSourceOfConsumption();
    compute();

});

abp.modals.ProductInfo = function () {

    function compute() {
        var soh = $("#Product_SOH").val();
        var amc = $("#Product_AMC").val();
        if (amc != 0) {
            var mos = soh / amc;
            $('#Product_MOS').val(mos.toFixed(2));
        }
        else
            $("#Product_MOS").val(0);
    }

    function toggleSourceOfConsumption() {
        var source = $("#Product_SourceOfConsumption").val();
        if (source == "Other") {
            $("#Product_OtherSourceOfConsumption").parents('.form-group').show();
            console.log("show text box");
        }
        else {
            $("#Product_OtherSourceOfConsumption").val("");
            $("#Product_OtherSourceOfConsumption").parents('.form-group').hide();
            console.log("clear and hide");
        }
    }

    $("#Product_SOH").change(compute);
    $("#Product_AMC").change(compute);
    $("#Product_SourceOfConsumption").change(toggleSourceOfConsumption);
    function initModal(modalManager, args) {
        var $modal = modalManager.getModal();
        var $form = modalManager.getForm();
        compute();
        toggleSourceOfConsumption();
        $modal.find("#Product_SOHLevels").multiselect({
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