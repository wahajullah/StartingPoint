var Details = function (id) {
    var url = "/Designation/Details?id=" + id;
    $('#titleMediumModal').html("Details Designation");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Designation/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Designation");
    }
    else {
        $('#titleMediumModal').html("Add Designation");
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
                url: "/Designation/Delete?id=" + id,
                success: function (result) {
                    var message = "Designation has been deleted successfully. Designation ID: " + result.Id;
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
