//判断该表中的字段是否已经有值了
function hasOne(url, fieldname, fieldvalue, KeyValue) {
    var has = false;
    AjaxJson(url + "/OnlyOne", { Fieldname: fieldname, Fieldvalue: fieldvalue, KeyValue: KeyValue }, function (data) {
        if (data.hasdata == "yes") {
            has = true;
        }
    });
    return has;
}
function checkHasOne(KeyValue) {
    var Validatemsg = "";
    var Validateflag = true;
    var $checkEle = $(".checkdata");
    $($checkEle).each(function () {
        if ($(this).attr("checkdata") != undefined) {
            var value = $(this).val();
            if (value == "==请选择==") {
                value = "";
            }
            switch ($(this).attr("checkdata")) {
                case "hasOne": {
                    if (hasOne($(this).attr("url"), $(this).attr("id"), value, KeyValue)) {
                        Validatemsg ="该"+$(this).attr("err") + "已存在！\n";
                        Validateflag = false;
                        if ($('#message').length > 0) {
                            $('#message').html("");
                            $("#message").html("<div class=\"note-error\"><div class=\"note-icon-error\"></div><div class=\"note-text\">" + Validatemsg + "</div></div>").slideDown('fast');
                        } else {
                            top.TipMsg(Validatemsg, 5000, 'error');
                        };
                        return false;
                    }
                    break;
                }
                default:
                    break;
            }
        }
    });
    return Validateflag;
}