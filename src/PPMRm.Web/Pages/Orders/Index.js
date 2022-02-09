
function format(d) {

    var linesHtml = "";
    for (let index = 0; index < d.lines.length; ++index) {
        const element = d.lines[index];
        linesHtml += '<tr>' +
            '<td>' + element.lineNumber + '</td>' +
            '<td>' + element.productId + '</td>' +
            '<td>' + element.itemName + '</td>' +
            '<td>' + element.uom + '</td>' +
            '<td>' + element.orderedQuantity + '</td>' +
            '<td>' + element.baseUnitMultiplier + '</td>' +
            '<td>' + element.totalQuantity + '</td>' +
            '</tr>';
        // ...use `element`...
    }

    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<thead>' +
        '<tr><td>Line #</td><td>Product ID</td><td>Name</td><td>UOM</td><td>Quantity</td><td>Multiplier</td><td>Total</td></tr>' +
        '</thead>' +
        '<tbody>' + 
        linesHtml +
        '</tbody>'
        '</table>';
}

$(document).ready(function () {

    var inputAction = function (requestData, dataTableSettings) {
        return {
            countries: $("#SelectedCountries").val()
        };
    };

    var dataTable = $('#OrdersTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            processing: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(pPMRm.orders.order.getList, inputAction),
            columnDefs: [
                {
                    title: "Country",
                    data: "countryId"
                },
                {
                    title: "RO Number",
                    data: "roNumber"
                },
                {
                    title: "PO/DO/IO Number",
                    data: "podoioNumber"
                },
                {
                    title: "RDD",
                    data: "requestedDeliveryDate",
                    dataFormat: "date"
                },
                {
                    title: "EDD",
                    data: "estimatedDeliveryDate",
                    dataFormat: "date"
                },
                {
                    title: "AcDD",
                    data: "actualDeliveryDate",
                    dataFormat: "date"
                },
                {
                    title: "Display Date",
                    data: "displayDate",
                    dataFormat: "date"
                },
                {
                    title: "PPMRm Date",
                    data: "deliveryDateType"
                },
                {
                    "className": 'dt-control',
                    title: "PPMRm Products (total)",
                    data: "lineTotal"
                }
            ]
        })
    );

    $('#SelectedCountries').on('change', function () {
        dataTable.ajax.reload();
    });

    //$("#OrdersTable tbody").click(function () {
    //    alert("Handler for .click() called.");
    //});

    $(document).on("click", "#OrdersTable tbody tr td", function () {
        //alert("Clicked td");
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);
        if (row.child.isShown()) {
            //alert("Close");
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
    $("#SelectedCountries").multiselect({
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 200,
        buttonWidth: '250px'
    });
    $(document).on("click", "#reload", function () {
        dataTable.reload();
    });
    //// Add event listener for opening and closing details
    //$('td.dt-control').on('click', function () {
    //    alert("Hello world");
    //    var tr = $(this).closest('tr');
    //    var row = table.row(tr);

    //    if (row.child.isShown()) {
    //        // This row is already open - close it
    //        row.child.hide();
    //        tr.removeClass('shown');
    //    }
    //    else {
    //        // Open this row
    //        row.child(format(row.data())).show();
    //        tr.addClass('shown');
    //    }
    //});
});