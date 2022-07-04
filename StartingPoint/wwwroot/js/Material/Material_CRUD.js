var Details = function (id) {
    var url = "/Material/Details?id=" + id;
    $('#titleBigModal').html("Detail Material");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/Material/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Material");
    }
    else {
        $('#titleBigModal').html("Add Material");
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
                url: "/Material/Delete?id=" + id,
                success: function (result) {
                    var message = "Material Record has been deleted successfully. Material Record ID: " + result.Id;
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
