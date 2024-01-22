app.controller("LocationController", function ($scope, $compile) {
    $scope.Location = {
        ID: 0,
        Name: "",
        IsActive: true
    };
    $scope.LocationSearch = {
        Search: "",
        Active: ""

    };
    LocationBindGrid();


    //for edit redirect to user form page
    $scope.LocationFetchById = function (recordId) {
        Get("/Location/FetchLocationById?locationID=" + recordId).then(function (d) {
            if (d.msg.Success) {
                $scope.Location = d.Data;
                $scope.$apply();
                $("#LocationModal").modal('show');
            }
            else
                ShowMessage(d);
        });

    }
    $scope.AddLocationModalOpen = function () {
        $("#LocationModal").modal('show');
    }
    $scope.AddLocationModalClose = function () {
        $("#LocationModal").modal('hide');
        $scope.ClearFields();
    }
    $scope.SaveLocation = function () {
        Post("/Location/Save", { location: $scope.Location }).then(function (d) {
            if (d.Success) {
                $scope.ClearFields();
                $("#LocationModal").modal('hide');
                LocationBindGrid();
            }

            ShowMessage(d);
        });
    }
    $scope.ClearFields = function () {
        $scope.Location.ID = 0;
        $scope.Location.Name = "";
        $scope.Location.IsActive = true;

    };
    $scope.LocationFetch = function () {
        LocationBindGrid();
    }
    function LocationBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#LocationTable").advancedDataTable({
            url: "/Location/Fetch",
            postData: $scope.LocationSearch,
            bindingData: [
                { "data": "Name" },
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
                        return '<a class="btn btn-info" onclick="LocationFetchById(' + row.Id + ')">Edit</a>';
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
function LocationFetchById(recordId) {
    angular.element(document.getElementById('DivLocationManagement')).scope().LocationFetchById(recordId);

}


