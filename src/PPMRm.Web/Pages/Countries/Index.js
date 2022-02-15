$(function () {


    var dataTable = $('#CountryTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(pPMRm.core.country.getList),
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
                    title: "CreationTime", data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                }
            ]
        })
    );
});
