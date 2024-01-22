app.controller("BuyerController", function ($scope, $compile) {

    $scope.BuyerSearch = {
        Search: "",
        StatusID: 0,
        BuyerID: 0

    };

    PreBind();
    function PreBind() {
        Get('/Rpt/PreBindBuyerReport', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstBuyers = d.Data.lstBuyers.Data;
                $scope.lstStatus = d.Data.lstStatus.Data;
                $scope.$apply();
                BuyerBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    //$scope.BuyerFetchById = function (recordId) {
    //    RedirectDelay('/OrderRequest/BuyerPendingRequestSetup?requestID=' + recordId + '&isEdit=true');

    //}
    $scope.BuyerFetch = function () {
        BuyerBindGrid();
    }
    function BuyerBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#BuyerTableRpt").advancedDataTable({
            url: "/Rpt/FetchBuyerBusiness",
            postData: $scope.BuyerSearch,
            bindingData: [
                { "data": "Title" },
                {
                    "data": "QoutationDate",
                    "render": function (value) {

                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(value);
                        var dt = results == null ? null : new Date(parseFloat(results[1]));
                        //format date dd-mm-yyyy
                        return dt == null ? '' : dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    }
                },
                { "data": "RequesterName" },
                //{ "data": "VendorName" },
                {
                    "data": "StatusName",
                    "render": function (data, type, row) {
                        return LabelStatus(row.StatusName);
                    }
                },
                { "data": "StrBuyerPrice" }
                //{
                //    "data": "ActionName",
                //    "render": function (data, type, row) {
                //        return LabelActions(row.ActionName);
                //    }
                //},
                
            ]//,
            // "footerCallback": function (row, data, start, end, display) {
            //    var api = this.api(), data;

            //    // Remove the formatting to get integer data for summation
            //    var intVal = function (i) {
            //        return typeof i === 'string' ?
            //            i.replace(/[\$,]/g, '') * 1 :
            //            typeof i === 'number' ?
            //                i : 0;
            //    };

            //    // Total over all pages
            //    total = api
            //        .column(5)
            //        .data()
            //        .reduce(function (a, b) {
            //            return intVal(a) + intVal(b);
            //        }, 0);

            //    // Total over this page
            //    pageTotal = api
            //        .column(5, { page: 'current' })
            //        .data()
            //        .reduce(function (a, b) {
            //            return intVal(a) + intVal(b);
            //        }, 0);

            //    // Update footer
            //    $(api.column(5).footer()).html(
            //        '$' + pageTotal + ' ( $' + total + ' total)'
            //    );
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");

    }
});
