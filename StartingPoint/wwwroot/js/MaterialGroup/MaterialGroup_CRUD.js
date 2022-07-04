var Details = function (id) {
    var url = "/MaterialGroup/Details?id=" + id;
    $('#titleMediumModal').html("Details MaterialGroup");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/MaterialGroup/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit MaterialGroup");
    }
    else {
        $('#titleMediumModal').html("Add MaterialGroup");
    }
    loadBigModal(url);
};

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/MaterialGroup/Delete?id=" + id,
                success: function (result) {
                    var message = "MaterialGroup Record has been deleted successfully. MaterialGroup Record ID: " + result.Id;
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
