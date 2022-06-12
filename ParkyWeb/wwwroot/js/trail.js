﻿
var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {

    dataTable = $('#tblData').DataTable({

        "ajax": {
            "url": "/trails/GetAllTrail",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {"data":"nationalPark.name", "width":"30%"},
            { "data": "name", "width": "25%" },
            { "data": "distance", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                          <div class="text-center">
                            <a href="/trails/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer;"><i class="far fa-edit"></i></a>
                            &nbsp;
                            <a onclick=Delete("/trails/Delete/${data}") class="btn btn-danger" style="cursor:pointer;"><i class="far fa-trash-alt"></i></a>
                          </div>
                        `;
                },
                "width":"30%"
            }
        ]

    });

}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore data!",
        icon: "warning",
        buttons: true,
        dangerMode: true 
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}