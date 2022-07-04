var Details = function (id) {
    var url = "/Status/Details?id=" + id;
    $('#titleMediumModal').html("Details Status");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Status/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Status");
    }
    else {
        $('#titleMediumModal').html("Add Status");
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
                url: "/Status/Delete?id=" + id,
                success: function (result) {
                    var message = "Status has been deleted successfully. AddressStatus ID: " + result.Id;
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
