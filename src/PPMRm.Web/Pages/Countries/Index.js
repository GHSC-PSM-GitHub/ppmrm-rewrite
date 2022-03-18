$(function () {

    var editModal = new abp.ModalManager(abp.appPath + 'Countries/EditModal');
    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
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
                                        console.log("Editing country: " + data.record.id);
                                        editModal.open({ id: data.record.id });
                                    }
                                }
                            ]
                    }
                }
            ]
        })
    );
});
