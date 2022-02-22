

$(document).ready(function () {

    var inputAction = function (requestData, dataTableSettings) {
        return {
            countries: $("#SelectedCountries").val()
        };
    };

    var dataTable = $('#PeriodReportsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            buttons: [
                'copyHtml5',
                'csvHtml5'
            ],
            serverSide: true,
            processing: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(pPMRm.periodReports.periodReport.getList, inputAction),
            columnDefs: [
                {
                    title: "Id",
                    data: "id"
                },
                {
                    title: "Country",
                    data: "country.name"
                },
                {
                    title: "Period",
                    data: "period.name"
                },
                {
                    title: "Shipments",
                    data: "shipmentsCount"
                },
                {
                    title: "SOH Information Count",
                    data: "productsCount"
                }
            ]
        })
    );

    $("#SelectedCountries").multiselect({
        selectAllValue: 'multiselect-all',
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 200,
        buttonWidth: '250px'
    });


    $('#SelectedCountries').on('change', function () {
        dataTable.ajax.reload();
    });
});