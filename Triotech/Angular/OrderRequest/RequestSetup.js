app.controller("RequestController", function ($scope, $http, $compile) {
    $scope.request = {
        ID: 0,
        Title: "",
        Qoutation1: "",
        Qoutation2: "",
        Qoutation3: "",
        BuyerDescription: "",
        VendorDescription: "",
        Comment: "",
        StatusID: 0,
        ActionID: 0,
        BuyerRequestID: 0,
        ProductRequests: [],
        BuyerRequests: [],
        lstSelectedVendors: []
    };
    $scope.lstCategories = [];
    $scope.lstProducts = [];
    $scope.lstProductsDDl = [];
    $scope.lstVendors = [];
    $scope.lstActions = [];
    $scope.lstStatus = [];
    PreBind();
    function PreBind() {
        Get('/OrderRequest/PreBindRequest', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstProducts = d.Data.lstProducts.Data;
                $scope.lstCategories = d.Data.lstCategories.Data;
                $scope.lstVendors = d.Data.lstVendors.Data;
                $scope.lstActions = d.Data.lstActions.Data;
                $scope.lstStatus = d.Data.lstStatus.Data;
                $scope.lstBuyers = d.Data.lstBuyers.Data;
                $scope.$apply();
                BindForm();
            }
        });

    }
    $scope.ChangeCategory = function (element) {
        var id = $(element).attr('id');
        var catVal = parseInt($(element).val().split(':')[1]);
        var index = id.split('_')[1];
        $('#productDdl_' + index).children('option:not(:first)').remove();
        for (var i = 0; i < $scope.lstProducts.length; i++) {
            if (catVal != 0) {

                for (var i = 0; i < $scope.lstProducts.length; i++) {
                    if ($scope.lstProducts[i].CategoryID == catVal) {
                        $('#productDdl_' + index).append('<option ng-value="' + $scope.lstProducts[i].ID + '" >' + $scope.lstProducts[i].Name + '</option>');
                    }
                }

            }
        }

    }
    //$('.catDDl').change(function () {


    //});
    function BindForm() {

        var editRequests = FetchParams();
        //isEdit is boolean type variable that check the call for save or Edit
        if (editRequests.isEdit) {
            Get('/OrderRequest/FetchRequestByID?requestID=' + editRequests.requestID, true).then(function (d) {
                if (d.msg.Success) {
                    $scope.request = d.Data;
                    var lstVendors = [];
                    for (var i = 0; i < $scope.request.BuyerRequests.length; i++) {
                        var option = 'number:' + $scope.request.BuyerRequests[i].VendorID;
                        lstVendors.push(option);
                    }
                    $('#Vendors').val(lstVendors).trigger('change');
                    $('.catDDl').each(function () {
                        var id = $(this).attr('id');
                        var catVal = parseInt($(this).val().split(':')[1]);
                        var index = id.split('_')[1];
                        //$('#productDdl_' + index).children('option:not(:first)').remove();
                        for (var i = 0; i < $scope.lstProducts.length; i++) {
                            if (catVal != 0) {

                                for (var i = 0; i < $scope.lstProducts.length; i++) {
                                    if ($scope.lstProducts[i].CategoryID == catVal) {
                                        $('#productDdl_' + index).append('<option ng-value="' + $scope.lstProducts[i].ID + '" value="' + $scope.lstProducts[i].ID + '">' + $scope.lstProducts[i].Name + '</option>');
                                    }
                                }
                            }
                        }

                    });

                    $scope.$apply();
                    for (var i = 0; i < d.Data.ProductRequests.length; i++) {
                        $('#productDdl_' + i).val($scope.request.ProductRequests[i].ProductID);
                    }

                }
            });
        }
        else {
            $scope.request.ProductRequests.push({ ID: 0, CategoryID: 0, ProductID: 0, CategoryName: "", ProductName: '', RequestID: 0 });
            $scope.$apply();
        }
    }
    $scope.AddDiv = function () {
        $scope.request.ProductRequests.push({ ID: 0, CategoryID: 0, ProductID: 0, CategoryName: "", ProductName: '', RequestID: 0 });
        //$scope.$apply();
    }

    $scope.RemoveDiv = function (index) {

        $scope.request.ProductRequests.splice(index, 1);
    }

    $scope.SubmitForm = function () {
        var flag = true;
        for (var i = 0; i < $scope.request.ProductRequests.length; i++) {
            $scope.request.ProductRequests[i].ProductID = parseInt($('#productDdl_' + i + ' option:selected').attr('ng-value'));
            if ($scope.request.ProductRequests[i].CategoryID == 0 || $scope.request.ProductRequests[i].ProductID == 0) {
                flag = false;
            }
        }
        if ($scope.request.ActionID == 0) {
            flag = false;
        }
        var statusID = 0;
        if ($("#requestStatusDdl").length) {
            if ($scope.request.StatusID == 0) {
                flag = false;
            }
            else {
                statusID = $scope.request.StatusID;
            }
        }
        if (flag) {
            if ($("#Vendors").length) {
                $scope.lstSelectedVendors = $("#Vendors").val();
                if ($scope.lstSelectedVendors.length != $scope.request.BuyerRequests.length) {
                    for (var i = 0; i < $scope.lstSelectedVendors.length; i++) {
                        var innerflag = true;
                        var buyerRequest = [];
                        for (var j = 0; j < $scope.request.BuyerRequests.length; j++) {

                            if ($scope.request.BuyerRequests[j].VendorID == $scope.lstSelectedVendors[i].split(':')[1])
                            {
                                innerflag = false;
                                break;

                            }

                        }
                        if (innerflag) {

                            $scope.request.BuyerRequests.push({ VendorID: $scope.lstSelectedVendors[i].split(':')[1] });
                        }
                    }
                }

            }

            Post("/OrderRequest/Save", { orderRequest: $scope.request, statusID }).then(function (d) {
                if (d.msg.Success) {
                    RedirectDelay(d.Data);
                }

                ShowMessage(d.msg);
            });
        }
        else {
            ErrorMessage("Please fill all required fields");
        }

    }
});
