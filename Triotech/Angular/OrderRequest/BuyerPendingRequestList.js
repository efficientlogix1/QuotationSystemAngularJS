app.controller("BuyerController", function ($scope, $compile) {

    $scope.BuyerSearch = {
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
                BuyerBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    $scope.BuyerFetchById = function (recordId) {
        RedirectDelay('/OrderRequest/BuyerPendingRequestSetup?requestID=' + recordId + '&isEdit=true');

    }
    $scope.BuyerFetch = function () {
        BuyerBindGrid();
    }
    function BuyerBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#BuyerTable").advancedDataTable({
            url: "/OrderRequest/FetchBuyerPendingOrders",
            postData: $scope.BuyerSearch,
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
                { "data": "RequesterName" },
                {
                    "data": "StatusName",
                    "render": function (data, type, row) {
                        return LabelStatus(row.StatusName);
                    }
                },
                //{
                //    "data": "ActionName",
                //    "render": function (data, type, row) {
                //        return LabelActions(row.ActionName);
                //    }
                //},
                {
                    "render": function (data, type, row) {
                        //$('.i-checks').iCheck({
                        //    checkboxClass: 'icheckbox_square-blue',
                        //    radioClass: 'iradio_square-blue'
                        //});
                        return '<a class="btn btn-info" href="javascript: void(0);" onclick="BuyerFetchById(' + row.Id + ')">Edit</a>';
                    },
                    //  "className": "dropdown",
                    "orderable": false
                }
            ]
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }
});
function BuyerFetchById(recordId) {
    angular.element(document.getElementById('DivBuyerManagement')).scope().BuyerFetchById(recordId);

}