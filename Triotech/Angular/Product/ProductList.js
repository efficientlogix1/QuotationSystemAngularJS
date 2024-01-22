app.controller("ProductController", function ($scope, $compile) {

    $scope.ProductSearch = {
        Search: "",
        Active: "",
        CategoryID: 0

    };

    PreBind();
    function PreBind() {


        Get('/Generic/FetchCategories', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstCategories = d.Data;
                $scope.$apply();
                ProductBindGrid();
            }
        });

    }
    //for edit redirect to user form page
    $scope.ProductFetchById = function (recordId) {
        RedirectDelay('/Product/ProductSetup?productID=' + recordId + '&isEdit=true');

    }
    $scope.ProductFetch = function () {
        ProductBindGrid();
    }
    function ProductBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#ProductTable").advancedDataTable({
            url: "/Product/Fetch",
            postData: $scope.ProductSearch,
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
                            { "data": "Code" },
                            { "data": "CategoryName" },
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
                                    return '<a class="btn btn-info" href="javascript: void(0);" onclick="ProductFetchById(' + row.Id + ')">Edit</a>';
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
function ProductFetchById(recordId) {
    angular.element(document.getElementById('DivProductManagement')).scope().ProductFetchById(recordId);

}