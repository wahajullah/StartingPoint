var funAction = function (UserProfileId) {
    var _Action = $("#" + UserProfileId).val();
    if (_Action == 1)
        AddEditUserAccount(UserProfileId);
    else if (_Action == 2)
        ResetPasswordAdmin(UserProfileId);
    else if (_Action == 3)
        ManagePageAccess(UserProfileId);
    else if (_Action == 4)
        DeleteUserAccount(UserProfileId);
    $("#" + UserProfileId).prop('selectedIndex', 0);
};

var ViewUserDetails = function (Id) {
    var url = "/UserManagement/ViewUserDetails?Id=" + Id;
    $('#titleBigModal').html("User Details ");
    loadMediumModal(url);
};


var AddEditUserAccount = function (id) {
    var url = "/UserManagement/AddEditUserAccount?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit User");
    }
    else {
        $('#titleBigModal').html("Add User");
    }
    loadBigModal(url);

    setTimeout(function () {
        var _divPasswordHash = document.getElementById("divPasswordHash");
        var _divConfirmPassword = document.getElementById("divConfirmPassword");

        if (id == 0) {
            $('#FirstName').focus();
            $('#Email').prop('readonly', false);
            //_divPasswordHash.style.display = "block";
            //_divConfirmPassword.style.display = "block";
        }
        else {
            $('#Email').prop('readonly', true);
            _divPasswordHash.style.display = "none";
            _divConfirmPassword.style.display = "none";
        }
    }, 200);
};


var ResetPasswordAdmin = function (id) {
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserManagement/ResetPasswordAdmin?id=" + id;
    loadMediumModal(url);
};

var ResetPasswordGeneral = function (ApplicationUserId) {
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserProfile/ResetPasswordGeneral?ApplicationUserId=" + ApplicationUserId;
    loadMediumModal(url);
};

var ManagePageAccess = function (id) {
    $('#titleBigModal').html("<h4>Manage Page Access</h4>");
    var url = "/UserManagement/ManageRole?id=" + id;
    loadBigModal(url);
};

var DeleteUserAccount = function (id) {
    Swal.fire({
        title: 'Do you want to delete this user?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/UserManagement/DeleteUserAccount?id=" + id,
                success: function (result) {
                    var message = "User has been deleted successfully. User Id: " + result.Email;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblUserAccount').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

