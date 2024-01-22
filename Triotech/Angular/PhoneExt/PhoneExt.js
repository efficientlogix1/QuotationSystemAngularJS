app.controller("PhoneExtController", function ($scope, $compile) {
    $scope.PhoneExt = {
        ID: 0,
        Code: "",
        IsActive: true
    };
    $scope.PhoneExtSearch = {
        Search: "",
        Active: ""

    };
    PhoneExtBindGrid();


    //for edit redirect to user form page
    $scope.PhoneExtFetchById = function (recordId) {
        Get("/PhoneExt/FetchPhoneExtByID?PhoneExtID=" + recordId).then(function (d) {
            if (d.msg.Success) {
                $scope.PhoneExt = d.Data;
                $scope.$apply();
                $("#PhoneExtModal").modal('show');
            }
            else
                ShowMessage(d);
        });

    }
    $scope.AddPhoneExtModalOpen = function () {
        $("#PhoneExtModal").modal('show');
    }
    $scope.AddPhoneExtModalClose = function () {
        $("#PhoneExtModal").modal('hide');
        $scope.ClearFields();
    }
    $scope.SavePhoneExt = function () {
        Post("/PhoneExt/Save", { phoneExt: $scope.PhoneExt }).then(function (d) {
            if (d.Success) {
                $scope.ClearFields();
                $("#PhoneExtModal").modal('hide');
                PhoneExtBindGrid();
            }

            ShowMessage(d);
        });
    }
    $scope.ClearFields = function () {
        $scope.PhoneExt.ID = 0;
        $scope.PhoneExt.Name = "";
        $scope.PhoneExt.IsActive = true;

    };
    $scope.PhoneExtFetch = function () {
        PhoneExtBindGrid();
    }
    function PhoneExtBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#PhoneExtTable").advancedDataTable({
            url: "/PhoneExt/Fetch",
            postData: $scope.PhoneExtSearch,
            bindingData: [
                { "data": "Code" },
                {
                    "render": function (data, type, row) {
                        return Label(row.IsActive, true);
                    }
                },
                {
                    "render": function (data, type, row) {
                        //$('.i-checks').iCheck({
                        //    checkboxClass: 'icheckbox_square-blue',
                        //    radioClass: 'iradio_square-blue'
                        //});
                        return '<a class="btn btn-info" onclick="PhoneExtFetchById(' + row.Id + ')">Edit</a>';
                    },
                    //"className": "dropdown"
                    "orderable": false
                }
            ],
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }

});
function PhoneExtFetchById(recordId) {
    angular.element(document.getElementById('DivPhoneExtManagement')).scope().PhoneExtFetchById(recordId);

}


