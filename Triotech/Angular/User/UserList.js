app.controller("UserController", function ($scope, $compile) {

    $scope.UserSearch = {
        Search: "",
        Active: "",
        RoleName: ''

    };
    UserBindGrid();


    //for edit redirect to user form page
    $scope.UserFetchById = function (recordId) {
        RedirectDelay('/User/UserProfile?userId=' + recordId + '&isEdit=true');

    }

    $scope.UserFetch = function () {
        UserBindGrid();
    }
    function UserBindGrid() {
        $scope.UserSearch.RoleName = $('#RoleName').val();
        // $("#chkUserToggle").iCheck('uncheck');
        $("#UserTable").advancedDataTable({
            url: "/User/Fetch",

            postData: $scope.UserSearch,
            bindingData: [
                { "data": "FirstName" },
                { "data": "LastName" },
                { "data": "Email" },
                { "data": "UserName" },
                {
                    "data": "StatusIsActive",
                    "render": function (data, type, row) {
                        return LabelActiveStatus(row.StatusIsActive);
                    }
                },
                {
                    "render": function (data, type, row) {
                        //$('.i-checks').iCheck({
                        //    checkboxClass: 'icheckbox_square-blue',
                        //    radioClass: 'iradio_square-blue'
                        //});href="javascript: void(0);"
                        if (!row.IsEmailSent) {
                            return '<a class="btn btn-info" onclick="UserFetchById(' + row.Id + ')">Edit</a> <a class="btn btn-primary" onclick="SendEmail(' + row.Id + ')">SendEmail</a>';
                        }
                        else {
                            return '<a class="btn btn-info" onclick="UserFetchById(' + row.Id + ')">Edit</a>';
                        }
                    },
                    //  "className": "dropdown",
                    "orderable": false
                },
            ]
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }
});
function UserFetchById(recordId) {
    angular.element(document.getElementById('DivUserManagement')).scope().UserFetchById(recordId);

}
function DeleteUser(recordId) {
    angular.element(document.getElementById('DivUserManagement')).scope().DeleteUser(recordId);

}
function SendEmail(recordId) {
    Post("/User/SendEmailToExpiredUser", { userId: recordId }).then(function (d) {
        if (d.Success) {
            angular.element(document.getElementById('DivUserManagement')).scope().UserFetch();
        }
        ShowMessage(d);
    });
}