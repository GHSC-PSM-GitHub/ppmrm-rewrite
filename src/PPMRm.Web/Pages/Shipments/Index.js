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
                        data: "productName"
                    },
                    {
                        title: "Quantity",
                        data: "productName"
                    },
                    {
                        title: "Base Unit Multiplier",
                        data: "productName"
                    },
                    {
                        title: "RDD",
                        data: "productName"
                    },
                    {
                        title: "EDD",
                        data: "productName"
                    },
                    {
                        title: "AcDD",
                        data: "productName"
                    }
                ]
            })
        );
    });
});
