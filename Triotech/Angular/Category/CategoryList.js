app.controller("CategoryController", function ($scope, $compile) {

    $scope.CategorySearch = {
        Search: "",
        Active: ""
    };


    BindGrid();


    //for edit redirect to user form page
    $scope.CategoryFetchById = function (recordId) {
        RedirectDelay('/Category/CategorySetup?categoryID=' + recordId + '&isEdit=true');

    }

    $scope.CategoryFetch = function () {
        BindGrid();
    }
    function BindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#CategoryTable").advancedDataTable({
            url: "/Category/Fetch",
            postData: $scope.CategorySearch,
            bindingData: [
                            { "data": "Name" },
                            {
                                "data": "CreationDate",
                                "render": function (value) {

                                    var pattern = /Date\(([^)]+)\)/;
                                    var results = pattern.exec(value);
                                    var dt = results == null ? null : new Date(parseFloat(results[1]));
                                    //format date dd-mm-yyyy
                                    return dt == null ? '' : dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
                                }
                            },
                            { "data": "type" },
                            
                            {
                                "data": "IsActive",
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
                                    return '<a class="btn btn-info" href="javascript: void(0);" onclick="CategoryFetchById(' + row.Id + ')">Edit</a>';
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
function CategoryFetchById(recordId) {
    angular.element(document.getElementById('DivCategoryManagement')).scope().CategoryFetchById(recordId);

}