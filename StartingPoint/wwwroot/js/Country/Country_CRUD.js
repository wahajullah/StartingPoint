var Details = function (id) {
    var url = "/Country/Details?id=" + id;
    $('#titleMediumModal').html("Details Country");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Country/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Country");
    }
    else {
        $('#titleMediumModal').html("Add Country");
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
                url: "/Country/Delete?id=" + id,
                success: function (result) {
                    var message = "Coutry has been deleted successfully. Country ID: " + result.Id;
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
