app.controller("AdminController", function ($scope, $compile) {

    $scope.AdminSearch = {
        Search: "",
        StatusID: 0,
        ActionID: 0

    };

    PreBind();
    function PreBind() {
        Get('/OrderRequest/PreBindRequestList', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstActions = d.Data.lstActions.Data;
                $scope.lstStatus = d.Data.lstStatus.Data;
                $scope.$apply();
                AdminBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    //$scope.AdminFetchById = function (recordId) {
    //    RedirectDelay('/OrderRequest/RequestSetup?requestID=' + recordId + '&isEdit=true');

    //}
    $scope.AdminFetch = function () {
        AdminBindGrid();
    }
    function AdminBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#AdminTable").advancedDataTable({
            url: "/OrderRequest/FetchAdminOrders",
            postData: $scope.AdminSearch,
            bindingData: [
                { "data": "Title" },
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
                { "data": "BuyerName" },
                { "data": "VendorName" },
                { "data": "RequesterName" },
                {
                    "data": "StatusName",
                    "render": function (data, type, row) {
                        return LabelStatus(row.StatusName);
                    }
                },
                {
                    "data": "ActionName",
                    "render": function (data, type, row) {
                        return LabelActions(row.ActionName);
                    }
                }
                //,{
                //    "render": function (data, type, row) {
                //        //$('.i-checks').iCheck({
                //        //    checkboxClass: 'icheckbox_square-blue',
                //        //    radioClass: 'iradio_square-blue'
                //        //});
                //        return '<a href="javascript: void(0);" onclick="AdminFetchById(' + row.Id + ')">Edit</a>';
                //    },
                //    //  "className": "dropdown",
                //    "orderable": false
                //}
            ]
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }
});
//function AdminFetchById(recordId) {
//    angular.element(document.getElementById('DivAdminManagement')).scope().AdminFetchById(recordId);

//}