app.controller("UserSetupController", function ($scope, $http, $compile) {
    $scope.BannersList = [];
    $scope.user = {
        Username: "",
        Email: "",
        Password: "",
        ConfirmPassword: "",
        FirstName: "",
        LastName: "",
        PhoneNumber: "",
        Address: "",
        RoleId: 0,
        UserId: 0,
        IsActive: true,
        IsCreated: false,
        Country: "",
        State: "",
        City: "",
        Street: "",
        Near: "",
        PostalCode: "",
        PaymentTerms: "",
        CompanyProfile: '',
        CompanyRegisteration:''
    };
    $scope.lstPaymentTerms = ['30 days', '60 days', '90 days'];
    $scope.user.PaymentTerms = $scope.lstPaymentTerms[0];
    
    $scope.SubmitAdminFormForm = function () {
        
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterAdmin", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/AdminList");
                }

                ShowMessage(d);
            });
        }
    }
    $scope.SubmitBuyerForm = function () {
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterBuyer", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/BuyerList");
                }

                ShowMessage(d);
            });
        }
    }
    $scope.SubmitVendorForm = function () {
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            $scope.user.PaymentTerms = $('.square-purple:checked').val();
            Post("/User/RegisterVendor", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/VendorList");
                }

                ShowMessage(d);
            });
        }
    }
    $scope.SubmitRequesterForm = function () {
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterRequester", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/RequesterList");
                }

                ShowMessage(d);
            });
        }
    }
    $scope.SubmitSupervisorForm = function () {
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterSupervisor", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/SupervisorList");
                }

                ShowMessage(d);
            });
        }
    }   

    $scope.SubmitManagementForm = function ()
    {
        if ($scope.user.ConfirmPassword != $scope.user.Password) {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterManagement", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/ManagementList");
                }

                ShowMessage(d);
            });
        }
    } 

    $scope.SubmitFinalApprovalForm = function ()
    {
        if ($scope.user.ConfirmPassword != $scope.user.Password)
        {
            ErrorMessage("Password and confirm password does not match");
        }
        else {
            Post("/User/RegisterFinalApproval", { model: $scope.user }).then(function (d) {
                if (d.Success) {
                    RedirectDelay("/User/FinalApprovalList");
                }

                ShowMessage(d);
            });
        }
    }   


});

