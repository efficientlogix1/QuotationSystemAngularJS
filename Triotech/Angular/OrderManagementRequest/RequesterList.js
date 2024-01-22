app.controller("RequesterListController", function ($scope, $compile) {

    $scope.RequesterListSearch =
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
               RequesterListBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    $scope.RequesterListFetchById = function (recordId)
    {
        RedirectDelay('/OrderManagementRequest/requestsetup?orderManagementRequestId=' + recordId + '&isEdit=true');

    }
    $scope.RequesterListFetch = function ()
    {
        RequesterListBindGrid();
    }
    function RequesterListBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#RequesterListTable").advancedDataTable({
            url: "/OrderManagementRequest/RequesterListFetch",
            postData: $scope.RequesterListSearch,
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
                    "data": "SupervisorName"

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
                        return '<a class="btn btn-info" href="javascript: void(0);" onclick="RequesterListFetchById(' + row.Id + ')">View</a>';
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
function RequesterListFetchById(recordId) {
    angular.element(document.getElementById('DivRequesterList')).scope().RequesterListFetchById(recordId);

}