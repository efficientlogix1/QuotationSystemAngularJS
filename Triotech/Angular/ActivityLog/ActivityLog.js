app.controller("ActivityLogController", function ($scope, $compile) {
    $scope.ActivityLogSearch = {
        Search: "",
        SearchDateRange: ""
    };
    ActivityLogBindGrid();


    ////for edit redirect to user form page
    //$scope.ActivityLogFetchById = function (recordId) {

    //    RedirectDelay('/ActivityLog/Edit?id=' + recordId);
    //}
    $scope.ActivityLogFetch = function () {
        ActivityLogBindGrid();
    }
    function ActivityLogBindGrid() {
        $("#ActivityLogTable").advancedDataTable({
            url: "/ActivityLog/Fetch",
            postData: $scope.ActivityLogSearch,
            bindingData: [
                { "data": "FirstName" },
                { "data": "LastName" },
                { "data": "UserName" },
                { "data": "Action" },
                { "data": "Detail" },
                {
                    "data": "CreateDate",
                    "render": function (value) {

                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(value);
                        var dt = results === null ? null : new Date(parseFloat(results[1]));
                        //format date dd-mm-yyyy
                        return dt === null ? '' : dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                    }
                },
                //{
                //    "render": function (data, type, row) {
                //        //$('.i-checks').iCheck({
                //        //    checkboxClass: 'icheckbox_square-blue',
                //        //    radioClass: 'iradio_square-blue'
                //        //});
                //        return '<a href="#" data-toggle="dropdown" class="dropDown_link"><i class="fa fa-chevron-down"></i></a><ul class="dropdown-menu"><li><a href="javascript: void(0);" onclick="ActivityLogFetchById(' + row.Id + ')"><i class="fa fa-pencil-square-o"></i> Edit</a></li></ul>';
                //    },
                //    "className": "dropdown",
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
//function ActivityLogFetchById(recordId) {
//    angular.element(document.getElementById('DivActivityLogManagement')).scope().ActivityLogFetchById(recordId);

//}
