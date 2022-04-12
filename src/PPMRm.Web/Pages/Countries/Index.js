$(function () {

    var editModal = new abp.ModalManager({
        viewUrl: '/Countries/EditModal',
        modalClass: 'CountryInfo' //Matches to the abp.modals.CountryInfo
    });
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

abp.modals.CountryInfo = function () {
    function initModal(modalManager, args) {
        var $modal = modalManager.getModal();
        var $form = modalManager.getForm();

        $modal.find("#Country_ProductIds").multiselect({
            selectAllValue: 'multiselect-all',
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            buttonWidth: '600px'
        });

    };

    return {
        initModal: initModal
    };
};