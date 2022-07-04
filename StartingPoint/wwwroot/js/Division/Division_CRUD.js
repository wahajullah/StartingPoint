var Details = function (id) {
    var url = "/Division/Details?id=" + id;
    $('#titleMediumModal').html("Details Division");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Division/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Division");
    }
    else {
        $('#titleMediumModal').html("Add Division");
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
                url: "/Division/Delete?id=" + id,
                success: function (result) {
                    var message = "Division has been deleted successfully. Division ID: " + result.Id;
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
