
$(document).ready(function () {

    $.fn.dataTable.render.moment = function (from, to, locale) {
        // Argument shifting
        if (arguments.length === 1) {
            locale = 'en';
            to = from;
            from = 'YYYY-MM-DD';
        }
        else if (arguments.length === 2) {
            locale = 'en';
        }

        return function (d, type, row) {
            if (!d) {
                return type === 'sort' || type === 'type' ? 0 : d;
            }

            var m = window.moment(d, from, locale, true);

            // Order and type get a number value from Moment, everything else
            // sees the rendered value
            return m.format(type === 'sort' || type === 'type' ? 'x' : to);
        };
    };


    var inputAction = function (requestData, dataTableSettings) {
        var countries = $("SelectedCountries").find("option:selected");
        var products = $("SelectedCountries").find("option:selected");
        return {
            countries: $("#SelectedCountries").val(),
            products: $("#SelectedProducts").val()
        };
    };

    $('#SelectedCountries').on('change', function () {
        dataTable.ajax.reload();
    });

    $('#SelectedProducts').on('change', function () {
        dataTable.ajax.reload();
    });

    $("#SelectedCountries").multiselect({
        selectAllValue: 'multiselect-all',
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 200,
        buttonWidth: '250px'
    });
    $("#SelectedProducts").multiselect({
        selectAllValue: 'multiselect-all',
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 200,
        maxWidth: 400,
        buttonWidth: '400px'
    });

    var dataTable = $('#OrderLinesTable').DataTable(
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
            ajax: abp.libs.datatables.createAjax(pPMRm.aRTMIS.orderLines.orderLine.getList, inputAction),
            columnDefs: [
                {
                    title: "Country",
                    data: "country.name"
                },
                {
                    title: "RO Number",
                    data: "roNumber"
                },
                {
                    title: "Order Number",
                    data: "orderNumber"
                },
                {
                    title: "Product",
                    data: "product.name"
                },
                {
                    title: "Ordered Quantity",
                    data: "orderedQuantity",
                    render: $.fn.dataTable.render.number(',', '.', 0, '')
                },
                {
                    title: "Base Unit Multiplier",
                    data: "item.baseUnitMultiplier"
                },
                {
                    title: "Total Quantity",
                    data: "totalQuantity",
                    render: $.fn.dataTable.render.number(',', '.', 0, '')
                },
                {
                    title: "Shipment Date",
                    data: "shipmentDateFormatted"
                },
                {
                    title: "Shipment Date Type",
                    data: "shipmentDateType"
                }
            ]
        })
    );

    
});