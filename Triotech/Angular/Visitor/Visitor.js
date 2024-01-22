app.controller("VisitorController", function ($scope, $compile) {
    //$scope.Visitor = {
    //    Id: 0,
    //    Name: "",
    //};
    $scope.VisitorSearch = {
        Search: ""
    };
    VisitorBindGrid();


    //for edit redirect to Visitor form page
    //$scope.VisitorFetchById = function (recordId) {
    //    Get("/Visitor/FetchVisitorById?VisitorId=" + recordId).then(function (d) {
    //        if (d.msg.Success) {
    //            $scope.Visitor = d.Data;
    //            $scope.$apply();
    //            $("#VisitorModal").modal('show');
    //        }
    //        else
    //            ShowMessage(d);
    //    });

    //}
    //$scope.AddVisitorModalOpen = function () {
    //    $scope.ClearFields();
    //    $("#VisitorModal").modal('show');
    //}
    //$scope.AddVisitorModalClose = function () {
    //    $("#VisitorModal").modal('hide');
    //    $scope.ClearFields();
    //}
    //$scope.SaveVisitor = function () {
    //    Post("/Visitor/Save", { Visitor: $scope.Visitor }).then(function (d) {
    //        if (d.Success) {
    //            $scope.ClearFields();
    //            $("#VisitorModal").modal('hide');
    //            VisitorBindGrid();
    //        }

    //        ShowMessage(d);
    //    });
    //}
    //$scope.ClearFields = function () {
    //    $scope.Visitor.Id = 0;
    //    $scope.Visitor.Name = "";
    //};
    $scope.VisitorFetch = function () {
        VisitorBindGrid();
    }
    function VisitorBindGrid() {
        // $("#chkVisitorToggle").iCheck('uncheck');
        $("#VisitorTable").advancedDataTable({
            url: "/Visitor/Fetch",
            postData: $scope.VisitorSearch,
            bindingData: [
                { "data": "IpAddress" },
                { "data": "Location" },
                {
                    "data": "VisitTime",
                    "render": function (value) {

                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(value);
                        var dt = results == null ? null : new Date(parseFloat(results[1]));
                        //format date dd-mm-yyyy
                        return dt == null ? '' : dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear() + ' ' + dt.toLocaleString().split(',')[1];
                    }
                }
            ]
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }
});
//function VisitorFetchById(recordId) {
//    angular.element(document.getElementById('DivVisitorManagement')).scope().VisitorFetchById(recordId);

//}
