$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    var csUpdatesModal = new abp.ModalManager(abp.appPath + 'PeriodReports/CSUpdatesModal');

    $("#edit-cs-updates").click(function (e) {
        e.preventDefault();
        var reportId = $("#Id").val();
        csUpdatesModal.open({ id: reportId });
    });

    $(".btn-add-program-product").click(function (e) {
        e.preventDefault();
        alert(JSON.stringify({
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id')
        }));
        //e.preventDefault();
        //var reportId = $("#Id").val();
        //csUpdatesModal.open({ id: reportId });
    });

    var inputAction = function (requestData, dataTableSettings) {
        return {
            id: "AGO-202112",
            programId: "4"
        };
    };

    $(".btn-edit-product").click(function (e) {
        e.preventDefault();
        alert(JSON.stringify({
            periodReportId: $(this).data('period-report-id'),
            programId: $(this).data('program-id'),
            productId: $(this).data('id')
        }));
        //e.preventDefault();
        //var reportId = $("#Id").val();
        //csUpdatesModal.open({ id: reportId });
    });


});