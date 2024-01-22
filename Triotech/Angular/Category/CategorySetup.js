app.controller("CategoryController", function ($scope, $http, $compile) {
    $scope.category = {
        ID: 0,
        Name: '',
        Type: "",
        Description: "",
        Number: "",
        Upload: "",
        IsActive: true
    };


    BindForm();
    function BindForm() {

        var editBanners = FetchParams();
        //isEdit is boolean type variable that check the call for save or Edit
        if (editBanners.isEdit) {
            Get('/Category/FetchCategoryById?categoryID=' + editBanners.categoryID, true).then(function (d) {
                if (d.msg.Success) {
                    $scope.category = d.Data;
                    $scope.$apply();

                }
            });
        }
    }
    $scope.SubmitForm = function () {
        Post("/Category/Save", { category: $scope.category }).then(function (d) {
            if (d.Success) {
                RedirectDelay("/Category/CategoryList");
            }
            ShowMessage(d);
        });
    }
    $scope.readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            var mimeType = input.files[0]['type'];

            if (mimeType.split('/')[1].trim() == 'vnd.ms-excel' || mimeType.split('/')[1].trim() == 'vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
                BlockUI();
                var formdata = new FormData();
                var files = $("#" + input.id + '')[0].files[0];
                formdata.append(files.name, files);
                $.ajax({
                    data: formdata,
                    method: "Post",
                    url: "/Category/UploadExcelFIle",
                    processData: false,
                    contentType: false,
                    success: function (d) {
                        UnBlockUI();
                        //location.reload();
                        $("#imgInp").val('');
                        ShowMessage(d);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown)
                    {
                        UnBlockUI();
                        ErrorMessage("Something went wrong, please try again");
                    }
                });

            }
            else {
                $("#" + input.id + '').val('');
                ErrorMessage('Please select excell file only');
            }
        }
    }
});
