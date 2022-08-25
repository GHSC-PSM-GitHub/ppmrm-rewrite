

$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    var inputAction = function (requestData, dataTableSettings) {
        return {
            countries: $("#SelectedCountries").val(),
            year: $("#SelectedYear").val(),
            month: $("#SelectedMonth").val()
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
                    title: "Report ID",
                    data: "id",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a href='/periodreports/edit/" + oData.id + "'>"+oData.id+"</a>");
                    }
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
                    title: "Status",
                    data: "reportStatus",
                    render: function (data) {
                        return l('Enum:PeriodReportStatus:' + data);
                    }
                },
                {
                    "title": "Enter/View Product Info",
                    "data": "id",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='btn btn-primary btn-sm' role='button' href='/periodreports/edit/" + oData.id + "'>View/Edit</a>");
                    }
                }
            ]
        })
    );

    var csModal = new abp.ModalManager(abp.appPath + 'PeriodReports/CSModal');

    csModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#PeriodReportsTable tbody').on('click', 'button', function (e) {
        e.preventDefault();
        var params = { id: $(this).data('id') };
        csModal.open(params);
    });

    $("#SelectedCountries").multiselect({
        selectAllValue: 'multiselect-all',
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        // inherits the class of the button from the original select
        //inheritClass: true,
        buttonClass: 'form-control',
        buttonWidth: '100%',
        maxHeight: 250

    });
    $("span.multiselect-native-select").addClass("form-control p-0 border-0 text-left");


    $('#SelectedCountries').on('change', function () {
        dataTable.ajax.reload();
    });

    $('#SelectedYear').on('change', function () {
        dataTable.ajax.reload();
    });

    $('#SelectedMonth').on('change', function () {
        dataTable.ajax.reload();
    });

   
});