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
                    data: "id"
                },
                {
                    title: "Name",
                    data: "name"
                },
                {
                    title: "Category",
                    data: "tracerCategory"
                },
                {
                    title: "Base Unit",
                    data: "baseUnit"
                },
                {
                    title: "Multiplier",
                    data: "baseUnitMultiplier"
                }
            ]
        })
    );
});
