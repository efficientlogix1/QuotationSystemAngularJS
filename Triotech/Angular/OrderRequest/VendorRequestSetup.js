app.controller("RequestController", function ($scope, $http, $compile) {
    $scope.request = {
        ID: 0,
        Title: "",
        Qoutation1: "",
        Qoutation2: "",
        Qoutation3: "",
        BuyerDescription: "",
        VendorDescription: "",
        RequesterName: '',
        BuyerName: "",
        Comment: "",
        StatusID: 0,
        ActionID: 0,
        BuyerRequestID: 0,
        ProductRequests: [],
        BuyerRequests: [],
        lstSelectedVendors: [],
        VendorRequests: [],
        isEdited: false
    };
    $scope.lstCategories = [];
    $scope.lstProducts = [];
    $scope.lstProductsDDl = [];
    $scope.lstVendors = [];
    $scope.lstActions = [];
    $scope.lstStatus = [];
    PreBind();
    function PreBind() {
        Get('/OrderRequest/PreBindVendorRequest', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstProducts = d.Data.lstProducts.Data;
                $scope.lstCategories = d.Data.lstCategories.Data;
                //$scope.lstVendors = d.Data.lstVendors.Data;
                $scope.lstActions = d.Data.lstActions.Data;
                $scope.lstStatus = d.Data.lstStatus.Data;
                //$scope.lstBuyers = d.Data.lstBuyers.Data;
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
            Get('/OrderRequest/FetchRequestVendorById?requestID=' + editRequests.requestID, true).then(function (d) {
                if (d.msg.Success) {
                    $scope.request = d.Data;
                    $scope.request.ActionID = 0;
                    $scope.request.ID = d.Data.selectedVendeorRequest.ID;
                    $scope.request.BuyerDescription = d.Data.selectedBuyer.BuyerDescription;
                    $scope.request.Qoutation1 = d.Data.selectedVendeorRequest.Qoutation1;
                    $scope.request.Qoutation2 = d.Data.selectedVendeorRequest.Qoutation2;
                    $scope.request.Qoutation3 = d.Data.selectedVendeorRequest.Qoutation3;
                    $scope.request.VendorDescription = d.Data.selectedVendeorRequest.VendorDescription;
                    $scope.request.Comment = d.Data.selectedVendeorRequest.Comment;
                    //$scope.request.VendorRequests = [];
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
                    if (d.Data.Qoutation1 != null) {
                        $scope.request.isEdited = true;
                    }
                    else {
                        $scope.request.isEdited = false;
                    }
                    $scope.$apply();
                    for (var i = 0; i < d.Data.ProductRequests.length; i++) {
                        $('#productDdl_' + i).val($scope.request.ProductRequests[i].ProductID);
                    }

                }
            });
        }
        else {
            ErrorMessage("No data found");
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
        //for (var i = 0; i < $scope.request.ProductRequests.length; i++) {
        //    $scope.request.ProductRequests[i].ProductID = parseInt($('#productDdl_' + i + ' option:selected').attr('ng-value'));
        //    if ($scope.request.ProductRequests[i].CategoryID == 0 || $scope.request.ProductRequests[i].ProductID == 0) {
        //        flag = false;
        //    }
        //}
        //if ($scope.request.ActionID == 0) {
        //    flag = false;
        //}
        //var statusID = 0;
        //if ($("#requestStatusDdl").length) {
        //    if ($scope.request.StatusID == 0) {
        //        flag = false;
        //    }
        //    else {
        //        statusID = $scope.request.StatusID;
        //    }
        //}
        //var actionID = 0;
        //if ($("#requestActionDdl").length) {
        //    if ($scope.request.ActionID == 0) {
        //        flag = false;
        //    }
        //    else {
        //        actionID = $scope.request.ActionID;
        //    }
        //}
        if (flag) {
            //if ($("#Vendors").length) {
            //    $scope.lstSelectedVendors = $("#Vendors").val();
            //    if ($scope.lstSelectedVendors.length != $scope.request.VendorRequests.length) {
            //        for (var i = 0; i < $scope.lstSelectedVendors.length; i++) {
            //            var innerflag = true;
            //            var buyerRequest = [];
            //            for (var j = 0; j < $scope.request.VendorRequests.length; j++) {

            //                if ($scope.request.VendorRequests[j].VendorID == $scope.lstSelectedVendors[i].split(':')[1]) {
            //                    innerflag = false;
            //                    break;

            //                }

            //            }
            //            if (innerflag) {

            //                $scope.request.VendorRequests.push({ VendorID: $scope.lstSelectedVendors[i].split(':')[1] });
            //            }
            //        }
            //    }

            //}
            Post("/OrderRequest/SaveVendorRequest", { vendorRequest: $scope.request }).then(function (d) {//, statusID }).then(function (d) {
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
