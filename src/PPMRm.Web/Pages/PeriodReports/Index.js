

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
                    title:"CS Updates",
                    rowAction: {
                        items:
                            [
                                {
                                    text: "CS Updates",
                                    action: function (data) {
                                        //csUpdatesModal.open({ id: data.record.id });
                                        csModal.open({ id: data.record.id });
                                    }
                                }
                            ]
                    }
                },
                {
                    "title": "Enter/View Product Info",
                    "data": "id",
                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        $(nTd).html("<a class='btn btn-primary' role='button' href='/periodreports/edit/" + oData.id + "'>View/Edit</a>");
                    }
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

    $('#SelectedYear').on('change', function () {
        dataTable.ajax.reload();
    });

    $('#SelectedMonth').on('change', function () {
        dataTable.ajax.reload();
    });

    var csModal = new abp.ModalManager(abp.appPath + 'PeriodReports/CSModal');

    csModal.onResult(function () {
        dataTable.ajax.reload();
    });
});