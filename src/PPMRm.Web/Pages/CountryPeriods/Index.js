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
                    data: "id",
                    orderable: true
                },
                {
                    title: "Name",
                    data: "name",
                    orderable: true
                },
                {
                    title: "Actions", data: "id",
                    render: function (data) {
                        return '<a href="/country/' + data + '/periods">View Periods</a>';
                    }
                }
            ]
        })
    );
});
