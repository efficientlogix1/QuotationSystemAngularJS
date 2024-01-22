app.controller("TermsOfServiceController", function ($scope, $compile) {
    $scope.TermsOfService = {
        ID: 0,
        Name: "",
        IsActive: true
    };
    $scope.TermsOfServiceSearch = {
        Search: "",
        Active: ""

    };
    TermsOfServiceBindGrid();


    //for edit redirect to user form page
    $scope.TermsOfServiceFetchById = function (recordId)
    {
        Get("/TermsOfService/FetchtermsOfServiceByID?termsOfServiceID=" + recordId).then(function (d) {
            if (d.msg.Success) {
                $scope.TermsOfService = d.Data;
                $scope.$apply();
                $("#TermsOfServiceModal").modal('show');
            }
            else
                ShowMessage(d);
        });

    }
    $scope.AddTermsOfServiceModalOpen = function ()
    {
        $("#TermsOfServiceModal").modal('show');
    }
    $scope.AddTermsOfServiceModalClose = function ()
    {
        $("#TermsOfServiceModal").modal('hide');
        $scope.ClearFields();
    }
    $scope.SaveTermsOfService = function ()
    {
        Post("/TermsOfService/Save", { termsOfService: $scope.TermsOfService }).then(function (d)
        {
            if (d.Success) {
                $scope.ClearFields();
                $("#TermsOfServiceModal").modal('hide');
                TermsOfServiceBindGrid();
            }

            ShowMessage(d);
        });
    }
    $scope.ClearFields = function () {
        $scope.TermsOfService.ID = 0;
        $scope.TermsOfService.Name = "";
        $scope.TermsOfService.IsActive = true;

    };
    $scope.TermsOfServiceFetch = function () {
        TermsOfServiceBindGrid();
    }
    function TermsOfServiceBindGrid() {
        // $("#chkUserToggle").iCheck('uncheck');
        $("#TermsOfServiceTable").advancedDataTable({
            url: "/TermsOfService/Fetch",
            postData: $scope.TermsOfServiceSearch,
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
                        return '<a class="btn btn-info" onclick="TermsOfServiceFetchById(' + row.Id + ')">Edit</a>';
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
function TermsOfServiceFetchById(recordId)
{

    angular.element(document.getElementById('DivTermsOfServiceManagement')).scope().TermsOfServiceFetchById(recordId);

}


