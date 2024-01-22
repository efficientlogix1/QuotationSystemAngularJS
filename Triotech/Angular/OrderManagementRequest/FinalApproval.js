app.controller("FinalApprovalListController", function ($scope, $compile)
{

    $scope.FinalApprovalListSearch =
        {
            Search: "",
            Active: ""

        };

    PreBind();
    function PreBind() {
        Get('/OrderManagementRequest/PreBindListing', true).then(function (d)
        {
            if (d.msg.Success) {
                $scope.lstStatus = d.Data;
                $scope.$apply();
                FinalApprovalListBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    $scope.FinalApprovalListFetchById = function (recordId)
    {
        RedirectDelay('/OrderManagementRequest/requestsetup?orderManagementRequestId=' + recordId + '&isEdit=true');

    }
    $scope.FinalApprovalListFetch = function ()
    {
        FinalApprovalListBindGrid();
    }
    function FinalApprovalListBindGrid()
    {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#FinalApprovalListTable").advancedDataTable({
            url: "/OrderManagementRequest/FinalApprovalListFetch",
            postData: $scope.FinalApprovalListSearch,
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
                    "data": "SupervisorName"

                },
                {
                    "data": "ManagementName"

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
                        if (row.RequestStatus == 'Approved By Final Approval' || row.RequestStatus == 'Rejected By Final Approval') {
                            return '<a class="btn btn-info" href="javascript: void(0);" onclick="FinalApprovalListFetchById(' + row.Id + ')">View</a>';
                        }
                        else {
                            return '<a class="btn btn-info" href="javascript: void(0);" onclick="FinalApprovalListFetchById(' + row.Id + ')">Edit</a>';
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
function FinalApprovalListFetchById(recordId)
{
    angular.element(document.getElementById('DivFinalApprovalList')).scope().FinalApprovalListFetchById(recordId);

}