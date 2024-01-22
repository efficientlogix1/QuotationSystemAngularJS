app.controller("OrderManagementRequestController", function ($scope, $http, $compile) {
    $scope.OrderManagementRequest =
        {

            ID: 0,
            RequestDate: "",
            StrRequestDate: "",
            RequestStatus: "",
            CurrentStatus: "",
            PhoneExtID: 0,
            PhoneExtCode: '',
            DepartmenttID: 0,
            DepartmenttName: '',
            
            ManagementName: '',
            FinalApprovalName: '',
            LocationID: 0,
            LocationName: '',
            PriorityID: 0,
            PriorityName: '',
            TermOfServiceID: 0,
            TermOfServiceName: '',
            ManagementID: 0,
            FinalApprovalID: 0,
            SupervisorID: 0,
            SupervisorName: '',
            // RequesterName: '',
            RequesterID: 0,
            RequesterComment: '',
            ManagementComment: '',
            RequestTittle: "",
            SupervisorComment: "",
            CEOComment: "",
            RequesterName: "",
            isUpdateBySupervisor: false,
            isUpdateByCEO: false,
            isUpdateByManagement: false,
            isPending: false



        };

    $scope.OrderManagementRequestSearch =
        {
            Search: "",
            Active: ""

        };


    $scope.ListDepartment = [];
    $scope.ListLocation = [];
    $scope.ListTermsOfService = [];
    $scope.ListPriority = [];
    $scope.ListPhoneExt = [];
    $scope.ListSupervisor = [];
    $scope.ListStatuses = [];
    $scope.ListManegement = [];
    $scope.ListFinalApproval = [];

    $scope.OrderManagementRequestSubmitForm = function () {

        if ($scope.OrderManagementRequest.ID == 0) {
            $scope.OrderManagementRequest.RequestStatus = 'Pending';
        }
        if ($scope.OrderManagementRequest.PhoneExtID != 0 && $scope.OrderManagementRequest.DepartmenttID != 0 && $scope.OrderManagementRequest.LocationID != 0 && $scope.OrderManagementRequest.PriorityID != 0 && $scope.OrderManagementRequest.TermOfServiceID != 0 && $scope.OrderManagementRequest.SupervisorID != 0 && $scope.OrderManagementRequest.RequestStatus != 0 && $scope.OrderManagementRequest.RequestStatus != $scope.OrderManagementRequest.CurrentStatus) {
            $scope.OrderManagementRequest.StrRequestDate = $('#requestDate').text();
            Post("/OrderManagementRequest/Save", { orderManagementRequest: $scope.OrderManagementRequest }).then(function (d) {
                if (d.Success) {
                    RedirectDelay($('#lstLink').attr('href'));//"/OrderManagementRequest/RequesterDataFetch");

                }
                ShowMessage(d);
            });
        }
        else {
            ErrorMessage("please select all values");
        }




    }





    PreBind();
    function PreBind() {
        Get('/OrderManagementRequest/PrebindorderManagementRequestSetup', true).then(function (d) {
            if (d.msg.Success) {
                $scope.ListDepartment = d.Data.lstDepartment;
                $scope.ListLocation = d.Data.lstLocation;
                $scope.ListTermsOfService = d.Data.lstTermsOfService;
                $scope.ListPriority = d.Data.lstPriority;
                $scope.ListPhoneExt = d.Data.lstPhoneExt;
                $scope.ListSupervisor = d.Data.lstSupervisor.Data;
                $scope.ListManegement = d.Data.lstManagement.Data;
                $scope.ListFinalApproval = d.Data.lstFinalApproval.Data;
                $scope.ListStatuses = d.Data.lstStatuses.Data;
                $scope.$apply();
                BindForm();
            }
        });

    }


    function BindForm() {

        var editRequests = FetchParams();
        //isEdit is boolean type variable that check the call for save or Edit
        if (editRequests.isEdit) {
            Get('/OrderManagementRequest/FetchorderManagementRequestByID?orderManagementRequestId=' + editRequests.orderManagementRequestId, true).then(function (d) {
                if (d.msg.Success) {
                    $scope.OrderManagementRequest = d.Data;
                    for (var i = 0; i < $scope.ListDepartment.length; i++) {
                        if (d.Data.DepartmenttID == $scope.ListDepartment[i].ID) {
                            $scope.OrderManagementRequest.DepartmenttName = $scope.ListDepartment[i].Name;
                            break;
                        }
                    }
                    for (var i = 0; i < $scope.ListLocation.length; i++) {
                        if (d.Data.LocationID == $scope.ListLocation[i].ID) {
                            $scope.OrderManagementRequest.LocationName = $scope.ListLocation[i].Name;
                            break;
                        }
                    }
                    for (var i = 0; i < $scope.ListTermsOfService.length; i++) {
                        if (d.Data.TermOfServiceID == $scope.ListTermsOfService[i].ID) {
                            $scope.OrderManagementRequest.TermOfServiceName = $scope.ListTermsOfService[i].Name;
                            break;
                        }
                    }
                    for (var i = 0; i < $scope.ListPriority.length; i++) {
                        if (d.Data.PriorityID == $scope.ListPriority[i].ID) {
                            $scope.OrderManagementRequest.PriorityName = $scope.ListPriority[i].Name;
                            break;
                        }
                    }
                    for (var i = 0; i < $scope.ListPhoneExt.length; i++) {
                        if (d.Data.PhoneExtID == $scope.ListPhoneExt[i].ID) {
                            $scope.OrderManagementRequest.PhoneExtCode = $scope.ListPhoneExt[i].Code;
                            break;
                        }
                    }
                    for (var i = 0; i < $scope.ListSupervisor.length; i++) {
                        if (d.Data.SupervisorID == $scope.ListSupervisor[i].Id) {
                            $scope.OrderManagementRequest.SupervisorName = $scope.ListSupervisor[i].Name;
                            break;
                        }
                    }
                    if (d.Data.RequestStatus == "Pending") {
                        $scope.OrderManagementRequest.isPending = true;
                    }
                    else {
                        $scope.OrderManagementRequest.isPending = false;
                    }
                    if (d.Data.RequestStatus == 'Approved By Management' || d.Data.RequestStatus == 'Rejected By Management') {
                        $scope.OrderManagementRequest.isUpdateByManagement = true;
                    }
                    else {
                        $scope.OrderManagementRequest.isUpdateByManagement = false;
                    }


                    if (d.Data.RequestStatus == 'Approved By Final Approval' || d.Data.RequestStatus == 'Rejected By Final Approval')
                    {

                        $scope.OrderManagementRequest.isUpdateByCEO = true;
                    }
                    else {
                        $scope.OrderManagementRequest.isUpdateByCEO = false;
                    }


                    if (d.Data.RequestStatus == 'Approved By Supervisor' || d.Data.RequestStatus == 'Rejected By Supervisor') {
                        $scope.OrderManagementRequest.isUpdateBySupervisor = true;
                    }
                    else {
                        $scope.OrderManagementRequest.isUpdateBySupervisor = false;
                    }

                    $scope.OrderManagementRequest.CurrentStatus = d.Data.RequestStatus;
                    $scope.$apply();


                }
            });
        }

    }



});
