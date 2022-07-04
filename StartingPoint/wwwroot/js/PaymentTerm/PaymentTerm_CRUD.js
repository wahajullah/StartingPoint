var Details = function (id) {
    var url = "/PaymentTerm/Details?id=" + id;
    $('#titleMediumModal').html("Details PaymentTerm");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/PaymentTerm/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit PaymentTerm");
    }
    else {
        $('#titleMediumModal').html("Add PaymentTerm");
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
                url: "/PaymentTerm/Delete?id=" + id,
                success: function (result) {
                    var message = "PaymentTerm has been deleted successfully. Country ID: " + result.Id;
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
