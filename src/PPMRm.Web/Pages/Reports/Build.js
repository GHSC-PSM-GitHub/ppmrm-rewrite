$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    

    var inputAction = function (requestData, dataTableSettings) {
        return {
            countries: $("#SelectedCountries").val(),
            year: $("#SelectedYear").val(),
            month: $("#SelectedMonth").val()
        };
    };

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

    $("#SelectedProducts").multiselect({
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

    $("#SelectedPrograms").multiselect({
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

    

    // $(".period-picker").datepicker({
    //     //altField: $(this).id + "Id",
    //     //altFormat: "yyyymm",
    //     format: "MM yyyy",
    //     startView: "months",
    //     minViewMode: "months",
    // });

    $("#StartPeriod").datepicker({
        format: "MM yyyy",
        startView: "years",
        minViewMode: "months",
        startDate: "December 2021",
        endDate: new Date()
    });

    $("#EndPeriod").datepicker({
        format: "MM yyyy",
        startView: "years",
        minViewMode: "months",
        startDate: "December 2021",
        endDate: new Date()
    });


    $('#tblReport').DataTable({
        dom: 'Brtp',
        paging: true,
        serverSide: false,
        columnDefs: [
            { targets: 'col-shipment', visible: false }
        ],
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [ 0, ':visible' ]
                }
            },
            {
                extend: 'excelHtml5',
                title: `Period Reports:  ${$("#StartPeriod").val()} - ${$("#EndPeriod").val()}`
            },
            'csv',
            'columnsToggle'
        ]
    });
    //$('#tblReport').DataTable();
    $('#tblReport').removeClass('dataTable');
    $('div.dt-buttons > button').removeClass('dt-button');
    $('div.dt-buttons > button').addClass('btn btn-primary mb-2');
});