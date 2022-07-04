﻿$(document).ready(function () {
    //document.title = 'Grade';

    $("#tblGrade").DataTable({

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
            "url": "/Grade/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                "data": "GradeId", "name": "GradeId",
                visible: true,
                //data: "GradeId", "name": "GradeId", render: function (data, type, row) {
                //    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                //}
            },
            { "data": "GradeId", "name": "GradeId" },
            { "data": "GradeName", "name": "GradeName" },

            {
                "data": "Section",
                "name": "Section",
                "autoWidth": true
                //"render": function (data) {
                //    var date = new Date(data);
                //    var month = date.getMonth() + 1;
                //    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                //}
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.GradeId + "');><i class='fas fa-pencil-alt'></i></a>" +
                        "<a href='#' class='btn btn-danger btn-xs ml-1' onclick=Delete('" + row.GradeId + "'); ><i class='fas fa-trash'></i></a>" +
                        "<a href='#' class='btn btn-primary btn-xs ml-1' onclick=Details('" + row.GradeId + "'); ><i class='fas fa-solid fa-eye'></i></a>";
                },

            },

        ],

        "columnDefs": [
            {
                "targets": [4], "searchable": false, "orderable": false

            },

        ],
        //'columnDefs': [{
        //    'targets': [4, 5],
        //    'orderable': false,
        //}],
        /* "lengthMenu": false*/

        /* "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]*/
    });

});

