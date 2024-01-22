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
        lstSelectedVendors: [],
        VendorName: '',
        RequesterName: '',
        BuyerPrice: 0,
        SelectedBuyerPrice: 0,
        VendorRequestID: 0,
        VendorCompany: ""
        //lstPrice:[]
    };
    //$scope.OrderSearch.;
    $scope.lstVendorDdl = [];
    $scope.lstCategories = [];
    $scope.lstProducts = [];
    $scope.lstProductsDDl = [];
    $scope.lstVendors = [];
    $scope.lstActions = [];
    $scope.lstStatus = [];
    //$scope.lstBuyerPrice = [];
    PreBind();
    function PreBind() {
        Get('/OrderRequest/PreBindBuyerRequest', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstProducts = d.Data.lstProducts.Data;
                $scope.lstCategories = d.Data.lstCategories.Data;
                $scope.lstVendors = d.Data.lstVendors.Data;
                $scope.lstActions = d.Data.lstActions.Data;
                $scope.lstStatus = d.Data.lstStatus.Data;
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
            Get('/OrderRequest/FetchVendorRequestById?requestID=' + editRequests.requestID, true).then(function (d) {
                if (d.msg.Success) {
                    $scope.request = d.Data;
                    $scope.request.BuyerPrice = d.Data.selectedBuyer.BuyerPrice;
                    $scope.request.ActionID = d.Data.selectedBuyer.ActionID;
                    $scope.request.VendorDescription = d.Data.selectedVendeorRequest.VendorDescription;
                    $scope.request.Comment = d.Data.selectedVendeorRequest.Comment;
                    $scope.request.Qoutation1 = d.Data.selectedVendeorRequest.Qoutation1;
                    $scope.request.SelectedBuyerPrice = d.Data.Qoutation1;
                    //$scope.request.VendorCompany = d.Data.VendorCompany;
                    //$scope.request.Qoutation2 = d.Data.selectedVendeorRequest.Qoutation2;
                    //$scope.request.Qoutation3 = d.Data.selectedVendeorRequest.Qoutation3;
                    //if (d.Data.lstPrice.length) {
                    //    for (var i = 0; i < d.Data.lstPrice.length; i++) {
                    //        if (d.Data.lstPrice[i] == d.Data.selectedBuyer.BuyerPrice) {
                    //            $scope.request.SelectedBuyerPrice = d.Data.lstPrice[i];
                    //        }
                    //    }
                    //}
                    $scope.lstVendorDdl = d.Data.lstVendors;
                    //$scope.request.VendorRequestID = d.Data.VendorRequestID;
                    //var lstVendors = [];
                    //for (var i = 0; i < $scope.request.BuyerRequests.length; i++) {
                    //    var option = 'number:' + $scope.request.BuyerRequests[i].VendorID;
                    //    lstVendors.push(option);
                    //}
                    //$('#Vendors').val(lstVendors).trigger('change');
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

                    for (var i = 0; i < d.Data.ProductRequests.length; i++) {
                        $('#productDdl_' + i).val($scope.request.ProductRequests[i].ProductID);
                    }
                    $scope.$apply();

                }
            });
        }
        else {
            ErrorMessage("No data found");
        }
    }

    $scope.OrderFetch = function () {
        Get('/OrderRequest/FetchVendorRequestByVendorRequestId?requestID=' + $scope.request.VendorRequestID, true).then(function (d) {
            if (d.msg.Success) {
                $scope.request.VendorDescription = d.Data.VendorDescription;
                $scope.request.Comment = d.Data.Comment;
                $scope.request.Qoutation1 = d.Data.Qoutation1;
                $scope.request.VendorName = d.Data.VendorName;
                $scope.request.SelectedBuyerPrice = d.Data.Qoutation1;
                $scope.request.VendorCompany = d.Data.VendorCompany;
                $scope.$apply();
            }
        });

    }

    $scope.SubmitForm = function () {
        var flag = true;
        if ($scope.request.SelectedBuyerPrice == 0) {
            flag = false;
        }
        if (flag) {
            var buyerRequest = { ID: $scope.request.BuyerRequestID, BuyerPrice: $scope.request.SelectedBuyerPrice, ActionID: $scope.request.ActionID };
            Post("/OrderRequest/SaveBuyerPrice", { buyerRequest: buyerRequest }).then(function (d) {
                if (d.msg.Success) {
                    RedirectDelay(d.Data);
                }

                ShowMessage(d.msg);
            });
        }
        else {
            ErrorMessage("Please select appropriate price!");
        }

    }
});
