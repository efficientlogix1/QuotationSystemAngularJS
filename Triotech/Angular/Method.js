function Post(url, data, isBlockUI) {
    if (typeof isBlockUI === "undefined" || isBlockUI)
        BlockUI();

    return $.ajax({
        method: "Post",
        url: url,
       // contentType: 'application/json',
        data: data,//JSON.stringify(data),
        success: function (d) {
            UnBlockUI();
            if ($.type(d) == "string")
                AccessDenied();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            UnBlockUI();
            ErrorMessage("Something went wrong, please try again");
        }
    });
}

function Get(url, isBlockUI) {
    if (typeof isBlockUI === "undefined" || isBlockUI)
        BlockUI();

    return $.ajax({
        method: "Get",
        url: url,
        success: function (d) {
            UnBlockUI();
            if ($.type(d) == "string")
                AccessDenied();

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            UnBlockUI();
            ErrorMessage("Something went wrong, please try again");
        }
    });
}

function GetWithoutAccess(url) {
    return $.ajax({
        method: "Get",
        url: url,
        success: function (d) {
            UnBlockUI();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            UnBlockUI();
            ErrorMessage("Something went wrong, please try again");
        }
    });
}

function PostWithoutAccess(url, data) {
    return $.ajax({
        method: "Post",
        url: url,
       // contentType: 'application/json',
        data:data,// JSON.stringify(data),
        success: function (d) {
            UnBlockUI();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            UnBlockUI();
            ErrorMessage("Something went wrong, please try again");
        }
    });
}

function SaveAndUpload(url, formData) {
    BlockUI();
    return $.ajax({
        data: formData,
        method: "Post",
        url: url,
        processData: false,
        contentType: false,
        success: function (d) {
            UnBlockUI();
            if ($.type(d) == "string")
                AccessDenied();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            UnBlockUI();
            ErrorMessage("Something went wrong, please try again");
        }
    });
}