app.controller("SupervisorListController", function ($scope, $compile) {

    $scope.SupervisorListSearch =
        {
            Search: "",
            Active: ""

        };

    PreBind();
    function PreBind() {
        Get('/OrderManagementRequest/PreBindListing', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstStatus = d.Data;
                $scope.$apply();
                SupervisorListBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    $scope.SupervisorListFetchById = function (recordId)
    {
        RedirectDelay('/OrderManagementRequest/requestsetup?orderManagementRequestId=' + recordId + '&isEdit=true');

    }
    $scope.SupervisorListFetch = function ()
    {
        SupervisorListBindGrid();
    }
    function SupervisorListBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#SupervisorListTable").advancedDataTable({
            url: "/OrderManagementRequest/SupervisorListFetch",
            postData: $scope.SupervisorListSearch,
            bindingData: [
                { "data": "RequestTittle" },
                {
                    "data": "RequestDate",
                    "render": function (value) {

                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(value);
                        var dt = results == null ? null : new Date(parseFloat(results[1]));
                        //format date dd-mm-yyyy
                        return dt == null ? '' : dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    }

                },

                {
                    "data": "RequesterName"

                },
                {
                    "data": "RequestStatus",
                    "render": function (data, type, row) {
                        return LabelOrderManagementStatus(row.RequestStatus);
                    }

                },
                {
                    "render": function (data, type, row) {
                        //$('.i-checks').iCheck({
                        //    checkboxClass: 'icheckbox_square-blue',
                        //    radioClass: 'iradio_square-blue'
                        //});
                        if (row.RequestStatus!== 'Pending')
                        {
                            return '<a class="btn btn-info" href="javascript: void(0);" onclick="SupervisorListFetchById(' + row.Id + ')">View</a>';
                        }
                        else
                        {
                        return '<a class="btn btn-info" href="javascript: void(0);" onclick="SupervisorListFetchById(' + row.Id + ')">Edit</a>';
                        }
                    },
                    //  "className": "dropdown",
                    "orderable": false
                }
            ]
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
    }
});
function SupervisorListFetchById(recordId) {
    angular.element(document.getElementById('DivSupervisorList')).scope().SupervisorListFetchById(recordId);

}