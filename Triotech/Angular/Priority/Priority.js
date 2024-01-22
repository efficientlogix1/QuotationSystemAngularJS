app.controller("PriorityController", function ($scope, $compile)
   {
    $scope.Priority =
    {
        ID: 0,
        Name: "",
        IsActive: true
    };
    $scope.PrioritySearch =
    {
        Search: "",
        Active: ""

    };
    PriorityBindGrid();


    //for edit redirect to user form page
    $scope.PriorityFetchById = function (recordId)
    {
        Get("/Priority/FetchPriorityById?priorityID=" + recordId).then(function (d) {
            if (d.msg.Success)
            {
                $scope.Priority = d.Data;
                $scope.$apply();
                $("#PriorityModal").modal('show');
            }
            else
                ShowMessage(d);
        });

    }
    $scope.AddPriorityModalOpen = function ()
    {
        $("#PriorityModal").modal('show');
    }
    $scope.AddPriorityModalClose = function ()
    {
        $("#PriorityModal").modal('hide');
        $scope.ClearFields();
    }
    $scope.SavePriority = function ()
    {
        Post("/Priority/Save", { priority: $scope.Priority }).then(function (d)
           {
            if (d.Success)
            {
                $scope.ClearFields();
                $("#PriorityModal").modal('hide');
                PriorityBindGrid();
            }

            ShowMessage(d);
        });
    }

    $scope.ClearFields = function ()
    {
        $scope.Priority.ID = 0;
        $scope.Priority.Name = "";
        $scope.Priority.IsActive = true;

    };
    $scope.PriorityFetch = function ()
    {
        PriorityBindGrid();
    }

    function PriorityBindGrid()
    {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#PriorityTable").advancedDataTable({
            url: "/Priority/Fetch",
            postData: $scope.PrioritySearch,
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
                        return '<a class="btn btn-info" onclick="PriorityFetchById(' + row.Id + ')">Edit</a>';
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
function PriorityFetchById(recordId)
{
    angular.element(document.getElementById('DivPriorityManagement')).scope().PriorityFetchById(recordId);

}


