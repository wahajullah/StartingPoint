var Details = function (id) {
    var url = "/Grade/Details?id=" + id;
    $('#titleMediumModal').html("Details Grade");
    loadMediumModal(url);
};

debugger;
var AddEdit = function (id) {
    var url = "/Grade/AddEdit?id=" + parseInt(id);
    if (id > 0) {
        $('#titleMediumModal').html("Edit Grade");
    }
    else {
        $('#titleMediumModal').html("Add Grade");
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
                url: "/Grade/Delete?id=" + id,
                success: function (result) {
                    var message = "Grade has been deleted successfully. Grade ID: " + result.GradeId;
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
