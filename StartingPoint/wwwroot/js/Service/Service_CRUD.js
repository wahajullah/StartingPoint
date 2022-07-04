var Details = function (id) {
    var url = "/Service/Details?id=" + id;
    $('#titleMediumModal').html("Details Service");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Service/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Service");
    }
    else {
        $('#titleMediumModal').html("Add Service");
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
                url: "/Service/Delete?id=" + id,
                success: function (result) {
                    var message = "Service has been deleted successfully. Service ID: " + result.Id;
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
