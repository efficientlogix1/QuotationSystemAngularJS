app.controller("DepartmentController", function ($scope, $compile) 
{
    $scope.Department =
    {
        ID: 0,
        Name: "",
        IsActive: true
    };
    $scope.DepartmentSearch =
    {
        Search: "",
        Active: ""

    };
    DepartmentBindGrid();


    //for edit redirect to user form page
    $scope.DepartmentFetchById = function (recordId) {
        Get("/Department/FetchdepartmentByID?departmentID=" + recordId).then(function (d)
        {
            if (d.msg.Success)
            {
                $scope.Department = d.Data;
                $scope.$apply();
                $("#DepartmentModal").modal('show');
            }
            else
                ShowMessage(d);
        });

    }
    $scope.AddDepartmentModalOpen = function () {
        $("#DepartmentModal").modal('show');
    }
    $scope.AddDepartmentModalClose = function () {
        $("#DepartmentModal").modal('hide');
        $scope.ClearFields();
    }
    $scope.SaveDepartment = function () {
        Post("/Department/Save", { department: $scope.Department }).then(function (d)
           {
            if (d.Success)
            {
                $scope.ClearFields();
                $("#DepartmentModal").modal('hide');
                DepartmentBindGrid();
            }

            ShowMessage(d);
        });
    }
    $scope.ClearFields = function () {
        $scope.Department.ID = 0;
        $scope.Department.Name = "";
        $scope.Department.IsActive = true;

    };
    $scope.DepartmentFetch = function ()
    {
        DepartmentBindGrid();
    }
    function DepartmentBindGrid()
    {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#DepartmentTable").advancedDataTable({
            url: "/Department/Fetch",
            postData: $scope.DepartmentSearch,
            bindingData: [
                { "data": "Name" },
                {
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
                        return '<a class="btn btn-info" onclick="DepartmentFetchById(' + row.Id + ')">Edit</a>';
                    },
                    //"className": "dropdown"
                    "orderable": false
                }
            ],
            //createdRow: function (row, data, dataIndex) {
            //    $(row).attr('onclick', 'UserGoto(' + data.Id + ')');
            //}
        });
        $(".buttons-html5").addClass("btn btn-primary").text("Export To Excel");
    }

});
function DepartmentFetchById(recordId)
{
    angular.element(document.getElementById('DivDepartmentManagement')).scope().DepartmentFetchById(recordId);

}


