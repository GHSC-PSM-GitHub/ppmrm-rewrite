$(function () {


    var dataTable = $('#ItemTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(pPMRm.items.item.getList),
            columnDefs: [
                {
                    title: "Code",
                    data: "id",
                    orderable: true
                },
                {
                    title: "Name",
                    data: "name",
                    orderable: true
                },
                {
                    title: "Category",
                    data: "tracerCategory",
                    orderable: true
                },
                {
                    title: "Base Unit",
                    data: "baseUnit",
                    orderable: true
                },
                {
                    title: "Multiplier",
                    data: "baseUnitMultiplier",
                    orderable: true
                }
            ]
        })
    );
});
