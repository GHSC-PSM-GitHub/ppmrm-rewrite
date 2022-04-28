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

    const ctx = document.getElementById('myChart');
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['RDT','Artesunate/Amodiaquine 25mg/67.5mg FDC 3 tabs', 'Artesunate Injectable 60mg', 'Sulphadoxine-Pyrimethamine', 'Artemether/Lumefantrine 20mg/120mg 6x4 Blisters', 'Artemether/Lumefantrine 20mg/120mg 6x3 Blisters'],
            datasets: [{
                label: '# of Stockouts',
                data: [1, 4, 2, 1, 6, 20],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

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
});