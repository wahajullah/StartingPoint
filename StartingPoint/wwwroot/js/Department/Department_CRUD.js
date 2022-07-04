﻿var Details = function (id) {
    var url = "/Department/Details?id=" + id;
    $('#titleMediumModal').html("Details Department");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Department/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Department");
    }
    else {
        $('#titleMediumModal').html("Add Department");
    }
    loadMediumModal(url);
};

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this record?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/Department/Delete?id=" + id,
                success: function (result) {
                    var message = "Department has been deleted successfully. Department ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        text: 'Deleted!',
                        onAfterClose: () => {
                            location.reload();
                        }
                    });
                }
            });
        }
    });
};
