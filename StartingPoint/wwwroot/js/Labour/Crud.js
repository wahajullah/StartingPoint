var Details = function (id) {
    var url = "/LineItem/Details?id=" + id;
    $('#titleMediumModal').html("Details LineItem");
    loadMediumModal(url);
};

var AddEdit = function (id) {
    var url = "/LineItem/AddEdit?id=" + parseInt(id);
    if (id > 0) {
        $('#titleMediumModal').html("Edit LineItem");
    }
    else {
        $('#titleMediumModal').html("Add LineItem");
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
                url: "/LineItem/Delete?id=" + id,
                success: function (result) {
                    var message = "LineItem has been deleted successfully. LineItem ID: " + result.Id;
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
