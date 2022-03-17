$(function () {


    var dataTable = $('#CountryTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(pPMRm.core.country.getUserCountryList),
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
                    title: "Min Stock",
                    data: "minStock"
                },
                {
                    title: "Max Stock",
                    data: "maxStock"
                },
                {
                    title: 'Actions',
                    rowAction: {
                        items:
                            [
                                {
                                    text: 'Edit',
                                    action: function (data) {
                                        //
                                    }
                                }
                            ]
                    }
                }
            ]
        })
    );
});
