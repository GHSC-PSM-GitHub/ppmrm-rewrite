$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    const colorConfigs = {
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)',
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
            'rgba(255, 159, 64, 1)',
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)'
        ]
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

    $("span.multiselect-native-select").addClass("form-control p-0 border-0 text-left");

    if ($("#SelectedPeriodId").val() !== '' && $("#chrtStockouts").length > 0) {
        pPMRm.reports.report.get($("#SelectedPeriodId").val()).then(function (result) {
        
            const ctx = document.getElementById('chrtStockouts');
            const ctxShortages = document.getElementById('chrtShortages');

            const ctxOversupply = document.getElementById('chrtOversupplies');


            const stockoutChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: result.stockouts.labels,
                    datasets: [{
                        label: '# of Countries/Channels per commodity stocked out',
                        backgroundColor: colorConfigs.backgroundColor,
                        borderColor: colorConfigs.borderColor,
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
                                stepSize: 1,
                                beginAtZero: true
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
                        backgroundColor: colorConfigs.backgroundColor,
                        borderColor: colorConfigs.borderColor,
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
                                stepSize: 1,
                                beginAtZero: true
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
                        backgroundColor: colorConfigs.backgroundColor,
                        borderColor: colorConfigs.borderColor,
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
                                stepSize: 1,
                                beginAtZero: true
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

    $("#SelectedPeriod").datepicker({
        format: "MM yyyy",
        startView: "years",
        minViewMode: "months",
        startDate: "December 2021",
        endDate: new Date()
    });

    $("#btnPdf").click(function () {
        console.log("printing...");
        var element = document.getElementById("reportSummary");
        var opt = {
            margin: 1,
            filename: 'myfile.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 2 },
            jsPDF: { unit: 'in', format: 'letter', orientation: 'landscape' }
        };

        // New Promise-based usage:
        html2pdf().set(opt).from(element).save();
        //html2pdf(element).then(function () {
        //    console.log("printed!");
        //}).catch(function (e) {
        //    console.log(e.toString())
        //});
    });

    
});