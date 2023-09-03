$(function () {

    $(function () {


        var dataTable = $('#ShipmentsTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(pPMRm.orders.order.getShipmentsList),
                columnDefs: [
                    {
                        title: "Product",
                        data: "productName",
                        orderable: true
                    },
                    {
                        title: "Quantity",
                        data: "productName",
                        orderable: true
                    },
                    {
                        title: "Base Unit Multiplier",
                        data: "productName",
                        orderable: true
                    },
                    {
                        title: "RDD",
                        data: "productName",
                        orderable: true
                    },
                    {
                        title: "EDD",
                        data: "productName",
                        orderable: true
                    },
                    {
                        title: "AcDD",
                        data: "productName",
                        orderable: true
                    }
                ]
            })
        );
    });
});
