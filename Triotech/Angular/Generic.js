var app = angular.module('TioTechModule', []);
$(function () {


    $('.Fixed').on('keypress', function (event) {
        if (((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which != 45 || $(this).val().indexOf('-') != -1)) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
        var input = $(this).val();
        if ((event.which == 45) && $(this).val().indexOf('-') == -1 && $(this).val() != "") {
            $(this).val(0 - input);
            event.preventDefault();
        }

        if ((input.indexOf('.') != -1) && (input.substring(input.indexOf('.')).length > 2)) {
            event.preventDefault();
        }
    });

    //    $('.i-checks').iCheck({
    //        checkboxClass: 'icheckbox_square-blue',
    //        radioClass: 'iradio_square-blue'
    //    });
    //    $('.datepicker').datetimepicker({ format: 'DD/MM/YYYY', showTodayButton: true, showClear: true, showClose: true });
    //    $('.timepicker').datetimepicker({ format: 'H:mm:ss', showClear: true, showClose: true });
    //    $('.datetimepicker').datetimepicker({ format: 'DD/MM/YYYY H:mm', showTodayButton: true, showClear: true, showClose: true });
    //    $('.switch').bootstrapToggle();
    //    $('.summernote').summernote();
    //    $(".dialog").dialog({
    //        autoOpen: false,
    //        modal: true,
    //        width: 400,
    //        show: {
    //            effect: "fade",
    //            duration: 500
    //        },
    //        hide: {
    //            effect: "fade",
    //            duration: 500
    //        }
    //    });
});

/*Customized Plugins*/
$.fn.advancedDataTable = function (options) {
    if ($.fn.dataTable.isDataTable(this))
        $(this).dataTable().fnPageChange($(this).dataTable().fnPagingInfo().iPage);
    else {
        $(this).dataTable().fnDestroy();
        $(this).DataTable({
            "processing": true,
            "bSort": true,
            "bFilter": false,
            "bDeferRender": true,
            //  "dom": 'Blfrtip',


            // lengthMenu: [
            //  [10, 25, 50, -1],
            //  ['10 rows', '25 rows', '50 rows', 'Show all']
            // ],
            //buttons: [
            // 'pageLength'
            //],
            "dom": 'lBfrtip',
            "pagingType": "full_numbers",

            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                { extend: 'excelHtml5', footer: true },
            ],

            
         //   "columnDefs": [
            //  { "width": "20%", "targets": 0 }
           // ],
    
            "oLanguage": {
                "sSearch": "",
                "sSearchPlaceholder": "Search...",
                "sProcessing": '<img src="/Assets/images/ajax-loader.gif" style="display:block; margin: 0 auto; width:200px;"/>',
                "sLengthMenu": "_MENU_",
                "oPaginate":
                    {
                        "sNext": '→',
                        "sLast": 'Last »',
                        "sFirst": '« First',
                        "sPrevious": '←'
                    }
            },
            "initComplete": function (settings, json) {
                $("[name=" + $(this).attr("id") + "_length]").addClass("form-control pull-right");
                //$("[name=" + $(this).attr("id") + "_length]").detach().prependTo(".manual-dropdown");
                //$(".dataTables_length").remove();
                // $("[name=" + $(this).attr("id") + "_length]").addClass("form-control");
                //$('.i-checks').iCheck({
                //    checkboxClass: 'icheckbox_square-blue',
                //    radioClass: 'iradio_square-blue'
                //});
                //$('.i-checks').iChecked();
            },
            "serverSide": true,
            "ajaxSource": options.url,
            //fnServerData method used to inject the parameters into the AJAX call sent to the server-side
            "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                //BlockUI();

                aoData.push({ "name": "SearchJson", "value": JSON.stringify(options.postData) }); // Add some extra data to the sender


                $.getJSON(sSource, aoData, function (d) {
                    /* Do whatever additional processing you want on the callback, then tell DataTables */

                    if (!d.msg.Success)
                        ShowMessage(d.msg);
                    else
                        fnCallback(d.Data);
                }).error(function (d, c, dd, t) {
                    if (d.status == 200) //{ }
                        AccessDenied();
                    else
                        ErrorMessage("Something went wrong, please try again");
                });
            },
            "bResetDisplay": false,
            "bLengthChange": true,
            "aaSorting": [],
            //"createdRow": options.createdRow,
            "drawCallback": function (settings) {
                //$('.i-checks').iCheck({
                //    checkboxClass: 'icheckbox_square-blue',
                //    radioClass: 'iradio_square-blue'
                //});
            },
            "columns": options.bindingData,
            "aoColumnDefs": options.disableSorting,
            
            "footerCallback": function (row, data, start, end, display) {
                if (this.selector == "#BuyerTableRpt") {
                    //var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    //var intVal = function (i) {
                    //    debugger;
                    //    return typeof i === 'string' ?
                    //        i.replace(/[\$,]/g, '') * 1 :
                    //        typeof i === 'number' ?
                    //            i : 0;
                    //};

                    // Total over all pages
                    //total = api
                    //    .column(5)
                    //    .data()
                    //    .reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    }, 0);

                    // Total over this page
                    //pageTotal = api
                    //    .column(5, { page: 'current' })
                    //    .data()
                    //    .reduce(function (a, b) {
                    //        return intVal(a) + intVal(b);
                    //    }, 0);
                    var pageTotal = 0;
                    var rowID = 0;
                    if (data.length>0) {
                        for (var i = 0; i < data.length; i++) {
                            if (rowID != data[i].Id) {
                                rowID = data[i].Id;
                                if (!isNaN(data[i].BuyerPrice)) {
                                    pageTotal += data[i].BuyerPrice;
                                }
                               
                            }
                        }
                    }
                    
                    // Update footer
                    //$(api.column(5).footer()).html(
                    //    '$' + pageTotal + ' ( $' + total + ' total)'
                    //);
                    $('#textTotal').text('$' + pageTotal.toFixed(2) + '');
                }
                
            }
        
        });
    }
};
$.fn.iChecked = function () {
    return $(this).prop("checked");
};

/*Prototypes*/
String.prototype.parseBoolean = function () {
    return ("true" == this.toLowerCase()) ? true : false
}
String.prototype.IsNullOrEmpty = function () {
    return ((this == "" || this == null) ? true : false)
}
/*Events*/
$(document).on("keypress", ".allow-number", function (evt) {
    return AllowNumber(evt);
});
$(document).on("keypress", ".not-zero", function (e) {

    var code = e.which,
        chr = String.fromCharCode(code), // key pressed converted to s string
        cur = $(this).val(),
        newVal = parseFloat(cur + chr); // what the input box will contain after this key press


    // If this keypress would make the number
    // out of bounds, ignore it
    if (newVal <= 0) {
        return false;
    }
    return true;
});
$(document).on("keypress", ".allow-number-decimal", function (evt) {
    return AllowDecimal(evt);
});
$(document).on("keypress", ".allow-alphabet", function (e) {
    var regex = new RegExp("^[a-z A-Z ]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str))
        return true;
    else {
        e.preventDefault();
        return false;
    }
});
$(document).on("keypress", "input", function (e) {
    var regex = new RegExp("<|>");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(str))
        return true;
    else {
        e.preventDefault();
        return false;
    }
});
$(document).on("keypress", "textarea", function (e) {
    var regex = new RegExp("<|>");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(str))
        return true;
    else {
        e.preventDefault();
        return false;
    }
});

/*Functions*/
function CopyToClipboard(text) {
    var aux = document.createElement("input");
    aux.setAttribute("value", text);
    document.body.appendChild(aux);
    aux.select();
    document.execCommand("copy");
    document.body.removeChild(aux);

}
function Print() {
    var prtContent = document.getElementById('DivReport');
    var WinPrint = window.open('', '', 'left=0,top=0,width=780,height=600,toolbar=0,scrollbars=1,status=1');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
    prtContent.innerHTML = strOldOne;

}
function IsNullOrEmpty(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
}
function CheckImage(obj) {
    obj.onerror = null;
    obj.src = '/Content/Image/User/defaultImage.gif';
}
function Label(isActive, isActiveInactive) {
    if (typeof isActiveInactive === "undefined")
        isActiveInactive = false;

    return (isActive ? '<span class="label label-success">' + (isActiveInactive ? 'Active' : 'Unlocked') + '</span>' : '<span class="label label-warning">' + (isActiveInactive ? 'Inactive' : 'Locked') + '</span>');

}
function LabelActiveStatus(status) {
    if (status == 'Active') {
        return '<span class="label label-success">' + status + '</span>'
    }
    if (status == 'InActive') {
        return '<span class="label label-warning">' + status + '</span>'
    }
    if (status == 'Expired') {
        return '<span class="label label-info">' + status + '</span>'
    }
}
function LabelStatus(status) {
    if (status == 'Pending') {
        return '<span style="background-color: #c60f7a">' + status + '</span>'
    }
    if (status == 'Completed') {
        return '<span style="background-color: #6ef442;">' + status + '</span>'
    }
    if (status == 'Not Respondant') {
        return '<span style="background-color: #e83d09">' + status + '</span>'
    }
}
function LabelActions(action) {
    if (action == 'Confirmed') {
        return '<span style="background-color: #0ae806">' + action + '</span>'
    }
    if (action == 'In Progress') {
        return '<span style="background-color: #06e8a7">' + action + '</span>'
    }
    if (action == 'Send To Vendor') {
        return '<span style="background-color: #7998e0">' + action + '</span>'
    }
    if (action == 'Incomplete') {
        return '<span style="background-color: #b282aa">' + action + '</span>'
    }
    if (action == 'Send To Buyer') {
        return '<span style="background-color: #AED39F">' + action + '</span>'
    }
    
}
function LabelOrderManagementStatus(status) {
    if (status == 'Pending') {
        return '<span style="background-color: #0ae806">' + status + '</span>';
    }
    if (status == 'Approved By Management') {
        return '<span style="background-color: #06e8a7">' + status + '</span>';
    }
    if (status == 'Rejected By Management') {
        return '<span style="background-color: #7998e0">' + status + '</span>';
    }
    if (status == 'Approved By Supervisor') {
        return '<span style="background-color: #b282aa">' + status + '</span>';
    }
    if (status == 'Rejected By Supervisor') {
        return '<span style="background-color: #AED39F">' + status + '</span>';
    }
    if (status == 'Approved By Final Approval') {
        return '<span style="background-color: #42f4d9">' + status + '</span>';
    }
    if (status == 'Rejected By Final Approval') {
        return '<span style="background-color: #f46b41">' + status + '</span>';
    }

}
function RedirectDelay(url) {
    setTimeout(function () { window.location.href = url; }, 1000);
}
function FetchParams()
{
    var url = window.location.href;
    var regex = /([^=&?]+)=([^&#]*)/g, params = {}, parts, key, value;

    while ((parts = regex.exec(url)) != null)
    {

        key = parts[1], value = parts[2];
        var isArray = /\[\]$/.test(key);

        if (isArray) {

            params[key] = params[key] || [];
            params[key].push(value);
        }
        else {

            params[key] = value;
        }
    }

    return params;
}
function FetchParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function AllowNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function AllowDecimal(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        // If the number field already has . then don't allow to enter . again.
        if (evt.target.value.search(/\./) > -1 && charCode == 46) {
            return false;
        }
        return true;
    }
}
function SuccessMessage(msg) {
    $.toast({
        heading: 'Success',
        text: msg,
        position: 'top-right',
        loaderBg: '#fff',
        icon: 'success',
        hideAfter: 8000
    });

}
function ErrorMessage(msg) {
    $.toast({
        heading: 'Error',
        text: msg,
        position: 'top-right',
        loaderBg: '#fff',
        icon: 'error',
        hideAfter: 8000
    });

}
function WarningMessage(msg) {
    $.toast({
        heading: 'Warning',
        text: msg,
        position: 'top-right',
        loaderBg: '#fff',
        icon: 'warning',
        hideAfter: 8000
        // stack: 1
    });
}
function InfoMessage(msg) {
    $.toast({
        heading: 'Info',
        text: msg,
        position: 'top-right',
        loaderBg: '#fff',
        icon: 'info',
        hideAfter: 8000
        //stack: 1
    });
}
function ShowMessage(msg) {
    if (!($.type(msg.MessageDetail) === "undefined")) {
        if (msg.Info) {
            InfoMessage(msg.MessageDetail);
        }
        else if (msg.Warning) {
            WarningMessage(msg.MessageDetail);
        }
        else if (msg.Success) {
            SuccessMessage(msg.MessageDetail);
        }
        else if (!msg.Success) {
            ErrorMessage(msg.MessageDetail);
        }
        else if (msg.Info) {
            InfoMessage(msg.MessageDetail);
        }
        else if (msg.Warning) {
            WarningMessage(msg.MessageDetail);
        }
    }
}
function BlockUI() {
    $("#divLoader").fadeIn("slow");
}
function UnBlockUI() {
    $("#divLoader").fadeOut("slow");
}
function AccessDenied() {
    swal({
        title: "Unauthorized access has been detected",
        text: "You are not authorize to perform this action",
        confirmButtonClass: 'btn-danger',
        confirmButtonText: "Ok, got it!",
        type: "error"
    });
}
function ConvertDecimal(value) {
    return parseFloat(value.toFixed(2));
}
function ChangePassword() {      //aamar
    var changePassword = {
        OldPassword: $("#OldPassword").val(),
        NewPassword: $("#NewPassword").val(),
        ConfirmPassword: $("#ConfirmPassword").val()
    };
    Post("/Manage/ChangePassword", { model: changePassword }).then(function (d) {
        ShowMessage(d)
        if (d.Success)
            RedirectDelay("/")
    })
}
jQuery.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
    return {
        "iStart": oSettings._iDisplayStart,
        "iEnd": oSettings.fnDisplayEnd(),
        "iLength": oSettings._iDisplayLength,
        "iTotal": oSettings.fnRecordsTotal(),
        "iFilteredTotal": oSettings.fnRecordsDisplay(),
        "iPage": oSettings._iDisplayLength === -1 ?
			0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
        "iTotalPages": oSettings._iDisplayLength === -1 ?
			0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
    };
};
//$('.daterange-basic').daterangepicker({
//    applyClass: 'bg-slate-600',
//    cancelClass: 'btn-default'
//});

//$('.daterange-basic').daterangepicker({
//    applyClass: 'bg-slate-600',
//    cancelClass: 'btn-default',
//    locale: {
//        format: 'DD/MM/YYYY',
//    }
//});

//$('.daterange-basic').val('');

//$('.daterange-basic').on('cancel.daterangepicker', function (ev, picker) {
//    $(this).val('');
//});

function PrintElem(element) {
    var printcontent = $('#' + element).clone();
    if (printcontent.css('display') == 'none') {
        printcontent.css('display', '');
    }
    $('body').empty().html(printcontent);
    window.print();
    location.reload();
}