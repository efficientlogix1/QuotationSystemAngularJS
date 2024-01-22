app.controller("ProductController", function ($scope, $http, $compile) {
    $scope.product = {
        ID: 0,
        Name: '',
        Code: '',
        Description: '',
        IsActive: true,
        CategoryID: 0,
    };
    //$scope.lstCategories = [];
    PreBind();
    function PreBind() {


        Get('/Generic/FetchCategories', true).then(function (d) {
            if (d.msg.Success) {
                $scope.lstCategories = d.Data;
                $scope.$apply();
                BindForm();
            }
        });

    }
    
    function BindForm() {

        var editBanners = FetchParams();
        //isEdit is boolean type variable that check the call for save or Edit
        if (editBanners.isEdit) {
            Get('/Product/FetchProductByID?productID=' + editBanners.productID, true).then(function (d) {
                if (d.msg.Success) {

                    $scope.product = d.Data;
                    $scope.$apply();

                }
            });
        }
    }
    $scope.SubmitForm = function () {
        if ($scope.product.CategoryID != 0) {
            Post("/Product/Save", { product: $scope.product }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/Product/ProductList");
                }
                ShowMessage(d);
            });
        }
        else {
            ErrorMessage("Please select any category");
        }
        
    }

});

