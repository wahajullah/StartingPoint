var Details = function (id) {
    var url = "/AddressBook/Details?id=" + id;
    $('#titleBigModal').html("Address Book Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/AddressBook/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit AddressBook");
    }
    else {
        $('#titleBigModal').html("Add AddressBook");
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
                url: "/AddressBook/Delete?id=" + id,
                success: function (result) {
                    var message = "AddressBook Record has been deleted successfully. AddressBook Record ID: " + result.Id;
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
