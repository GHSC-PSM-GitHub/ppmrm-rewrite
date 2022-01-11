﻿$(function () {

    $(function () {


        var dataTable = $('#OrdersTable').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                serverSide: true,
                paging: true,
                order: [[1, "asc"]],
                searching: false,
                scrollX: true,
                ajax: abp.libs.datatables.createAjax(pPMRm.orders.order.getList),
                columnDefs: [
                    {
                        title: "Country",
                        data: "country"
                    },
                    {
                        title: "RO Number",
                        data: "country"
                    },
                    {
                        title: "Status",
                        data: "country"
                    },
                    {
                        title: "RDD",
                        data: "country"
                    },
                    {
                        title: "EDD",
                        data: "country"
                    },
                    {
                        title: "AcDD",
                        data: "country"
                    }
                ]
            })
        );
    });
});
