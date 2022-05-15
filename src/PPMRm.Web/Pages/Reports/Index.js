$(document).ready(function () {

    var l = abp.localization.getResource('PPMRm');

    const colorConfigs = {
        backgroundColor: [
            '#112e51',
            '#2e8540',
            '#4c2c92',
            '#212121',
            '#046b99',
            '#cd2026',
            '#fad980',
            '#94bfa2',
            '#112e51',
            '#2e8540',
            '#4c2c92',
            '#212121',
            '#046b99',
            '#cd2026',
            '#fad980',
            '#94bfa2'
        ],
        borderColor: [
            '#112e51',
            '#2e8540',
            '#4c2c92',
            '#212121',
            '#046b99',
            '#cd2026',
            '#fad980',
            '#94bfa2',
            '#112e51',
            '#2e8540',
            '#4c2c92',
            '#212121',
            '#046b99',
            '#cd2026',
            '#fad980',
            '#94bfa2'
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
                    legend: {
                        display: true,
                        labels: {
                            fontColor: 'rgb(255, 99, 132)'
                        }
                    },
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
                    },
                    legend: {
                        display: true,
                        labels: {
                            fontColor: 'rgb(255, 99, 132)'
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
                    //legend: {
                    //    display: true,
                    //    labels: {
                    //        fontColor: 'rgb(255, 99, 132)'
                    //    }
                    //},
                    title: {
                        display: true,
                        text: '# of Countries per Commodity Overstocked',
                        fontColor: '#000000',
                        position: 'bottom'
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
        window.print();

        // New Promise-based usage:
        //html2pdf().set(opt).from(element).save();
        //html2pdf(element).then(function () {
        //    console.log("printed!");
        //}).catch(function (e) {
        //    console.log(e.toString())
        //});
    });

    
});