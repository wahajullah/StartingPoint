$(document).ready(function () {
    document.title = 'AddressBook';

    $("#tblAddressBook").DataTable({
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
            "url": "/AddressBook/GetDataTabelData",
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
            { "data": "AddressId", "name": "AddressId" },
            { "data": "Name", "name": "Name" },
            { "data": "JobTitle", "name": "JobTitle" },
            { "data": "Company", "name": "Company" },
            { "data": "Mobile", "name": "Mobile" },
            { "data": "OEmail", "name": "OEmail" },
            { "data": "CityDisplay", "name": "CityDisplay" },

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
                "targets": [9], "searchable": false, "orderable": false

            },

        ],


    });

});

