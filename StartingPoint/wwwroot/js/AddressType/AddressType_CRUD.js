var Details = function (id) {
    var url = "/AddressType/Details?id=" + id;
    $('#titleMediumModal').html("Details AddressType");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/AddressType/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit AddressType");
    }
    else {
        $('#titleMediumModal').html("Add AddressType");
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
                url: "/AddressType/Delete?id=" + id,
                success: function (result) {
                    var message = "AddressType has been deleted successfully. AddressType ID: " + result.Id;
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
