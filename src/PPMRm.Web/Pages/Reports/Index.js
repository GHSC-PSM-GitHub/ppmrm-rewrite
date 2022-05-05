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

    if ($("#SelectedPeriodId").val() !== '') {
        pPMRm.reports.report.get($("#SelectedPeriodId").val()).then(function (result) {
        console.log(JSON.stringify(result));
        const ctx = document.getElementById('chrtStockouts');
        const ctxShortages = document.getElementById('chrtShortages');
        const ctxOversupply = document.getElementById('chrtOversupplies');
        if (ctx === null || ctx === undefined) return;
        //const chartData = pPMRm.report.report.get()
        const stockoutChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: result.stockouts.labels,
                datasets: [{
                    label: '# of Countries/Channels per commodity stocked out',
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
                    data: result.stockouts.data,
                    borderWidth: 1
                }]
            },
            options: {
                maintainAspectRatio: false,
                scales: {
                    y: {
                        stacked: true,
                        grid: {
                            display: true,
                            color: "rgba(255,99,132,0.2)"
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    yAxes: [{
                        ticks: {
                            stepSize: 1
                        },
                        gridLines: {
                            display: false
                        }
                    }]
                },
                plugins: {
                    title: {
                        display: true,
                        text: '# of Countries'
                    }
                }
            }
        });

        const shortageChart = new Chart(ctxShortages, {
            type: 'bar',
            data: {
                labels: result.shortages.labels,
                datasets: [{
                    label: '# of Countries/Channels per Commodity Understocked',
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        //'rgba(255, 206, 86, 0.2)',
                        //'rgba(75, 192, 192, 0.2)',
                        //'rgba(153, 102, 255, 0.2)',
                        //'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        //'rgba(255, 206, 86, 1)',
                        //'rgba(75, 192, 192, 1)',
                        //'rgba(153, 102, 255, 1)',
                        //'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1,
                    data: result.shortages.data,
                }]
            },
            options: {
                maintainAspectRatio: false,
                scales: {
                    y: {
                        stacked: true,
                        grid: {
                            display: true,
                            color: "rgba(255,99,132,0.2)"
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    yAxes: [{
                        ticks: {
                            stepSize: 1
                        },
                        gridLines: {
                            display: false
                        }
                    }]
                },
                plugins: {
                    title: {
                        display: true,
                        text: '# of Countries'
                    }
                }
            }
        });

        const oversupplyChart = new Chart(ctxOversupply, {
            type: 'bar',
            data: {
                labels: result.oversupplies.labels,
                datasets: [{
                    label: '# of Countries/Channels per Commodity Overstocked',
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        //'rgba(255, 206, 86, 0.2)',
                        //'rgba(75, 192, 192, 0.2)',
                        //'rgba(153, 102, 255, 0.2)',
                        //'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        //'rgba(255, 206, 86, 1)',
                        //'rgba(75, 192, 192, 1)',
                        //'rgba(153, 102, 255, 1)',
                        //'rgba(255, 159, 64, 1)'
                    ],
                    data: result.oversupplies.data,
                    borderWidth: 1
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: true,
                        labels: {
                            color: 'rgb(255, 99, 132)'
                        }
                    }
                },
                indexAxis: 'x',
                maintainAspectRatio: false,
                scales: {
                    y: {
                        stacked: true,
                        grid: {
                            display: true,
                            color: "rgba(255,99,132,0.2)"
                        }
                    },
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    yAxes: [{
                        ticks: {
                            stepSize: 1
                        },
                        gridLines: {
                            display: false
                        }
                    }]
                },
                plugins: {
                    title: {
                        display: true,
                        text: '# of Countries'
                    }
                }
            }
        });
    });
    }

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

    $("#SelectedPeriod").datepicker({
        format: "MM yyyy",
        startView: "years",
        minViewMode: "months",
        startDate: "December 2021",
        endDate: new Date()
    });

    $("#btnPdf").click(function () {
        window.print();
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