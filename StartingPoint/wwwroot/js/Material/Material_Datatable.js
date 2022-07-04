$(document).ready(function () {
    document.title = 'Material';

    $("#tblMaterial").DataTable({
        /* "paging": true,*/
        "lengthChange": false,
        "pageLength": 12,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,


        /* paging: true,*/
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtp',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print',
        ],       

        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/Material/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                },
                visible: false,
            },
            { "data": "MaterialId", "name": "MaterialId" },
            { "data": "ServiceDisplay", "name": "ServiceDisplay" },
            { "data": "MaterialGroupDisplay", "name": "MaterialGroupDisplay" },            
            { "data": "Description", "name": "Description" },
            { "data": "Unit", "name": "Unit" },
            { "data": "Rate", "name": "Rate" },
            { "data": "Brand", "name": "Brand" },
            { "data": "Model", "name": "Model" },
            { "data": "Manufacturer", "name": "Manufacturer" },
            { "data": "SupplierDisplay", "name": "SupplierDisplay" },
            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');><i class='fas fa-pencil-alt'></i></a>" +
                        "<a href='#' class='btn btn-danger btn-xs ml-1' onclick=Delete('" + row.Id + "'); ><i class='fas fa-trash'></i></a>" +
                        "<a href='#' class='btn btn-primary btn-xs ml-1' onclick=Details('" + row.Id + "'); ><i class='fas fa-solid fa-eye'></i></a>";

                }
            },
            //{
            //    data: null, render: function (data, type, row) {
            //        return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.Id + "'); >Delete</a>";
            //    }
            //}
        ],

        "columnDefs": [
            {
                "targets": [5], "searchable": false, "orderable": false

            },

        ],


    });

});

