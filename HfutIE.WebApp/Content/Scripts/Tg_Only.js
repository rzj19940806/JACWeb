//加载界面的宽高
function SetAllWH() {
    var width = $(window).width();
    var height = $(window).height();
    $("#all").width(width);
    $("#all").height(height);
}
//加载静态信息
function GetStaticInfo() {
    try {
        AjaxJson('/TightOnly/GetStaticInfo', null, function (data) {
            if (data.code == 1) {
                allStaticInfo = data.props;
                $("#station").html(data.props.WcNm);
                $("#people").html(data.props.StfNm);
                MqttConnect();      //连接Mqtt
                updateLog("设备登陆成功:【工位：" + data.props.WcNm + "】【人员：" + data.props.StfNm + "】");
            } else if (data.code == -1) {
                uperrorLog("设备登陆错误:【错误信息：" + data.msg + "】");
                $("#vin_input").attr("readonly", "true");
            }
        });

    } catch (e) {
        uperrorLog("HTML静态信息错误:【错误信息：" + e + "】");
    }
}

//加载静态信息
function GetStaticInfoFZ(isMain) {
    try {
        AjaxJson('/TightOnly/GetStaticInfoFZ', null, function (data) {
            if (data.code == 1) {
                allStaticInfo = data.props;
                barcodeRule = data.BarCode;
                targetTuhao = data.strs;
                if (data.props.PlineCd == "Line-17") {
                    AviId = data.AviProps.AviId;
                }
                $("#station").html(data.props.WcNm);
                $("#people").html(data.props.StfNm);
                if (isMain == true) {
                    MqttConnect();      //连接Mqtt
                }
                updateLog("设备登陆成功:【工位：" + data.props.WcNm + "】【人员：" + data.props.StfNm + "】");
            } else if (data.code == -1) {
                uperrorLog("设备登陆错误:【错误信息：" + data.msg + "】");
                $("#vin_input").attr("readonly", "true");
                $("#part_input").attr("readonly", "readonly");
            }
        });
    } catch (e) {
        uperrorLog("HTML静态信息错误:【错误信息：" + e + "】");
    }
}


//更新日志信息
function updateLog(data) {
    let html = $("#bottom_text")[0].innerHTML;
    $("#bottom_text")[0].innerHTML = `<p style='color:green;font-size:20px;float:left;margin-left:20px;text-align: left;' >${data}-【时间：${$("#localtime").html()}】</p><br/>` + html;
    AjaxJson("/OT/WriteLog", { PlineCd: allStaticInfo.PlineCd, text: data }, function (data) {
        return false;
    });
}
function uperrorLog(data) {
    let html = $("#bottom_text")[0].innerHTML;
    $("#bottom_text")[0].innerHTML = `<p style='color:red;font-size:20px;float:left;margin-left:20px;text-align: left;' >${data}-【时间：${$("#localtime").html()}】</p><br/>` + html;
    if (allStaticInfo == undefined) {
        return false;
    }
    AjaxJson("/OT/WriteLog", { PlineCd: allStaticInfo.PlineCd + "Error", text: data }, function (data) {
        return false;
    });
}
//时间刷新
function GetCurrentDate() {
    var myDate = new Date();

    var year = myDate.getFullYear();        //获取当前年
    var month = myDate.getMonth() + 1;   //获取当前月
    var date = myDate.getDate();            //获取当前日

    var h = myDate.getHours();              //获取当前小时数(0-23)
    var m = myDate.getMinutes();          //获取当前分钟数(0-59)
    var s = myDate.getSeconds();

    var now = year + '-' + month + "-" + date + " " + h + ':' + m + ":" + s;
    jQuery("#localtime").html(now);
    setTimeout(GetCurrentDate, 1000);
}
//界面刷新为无车状态
function NotHaveCar() {
    $("#Tighten")[0].innerHTML = "";
    CarPicture("车身默认照片");
    $("#vin").html("当前工位无车");//Vin码
    $("#sequence").html("顺序号");//顺序号
    $("#color").html("颜色");//颜色
    $("#cartype").html("车型");//车型
    $("#car_dis").html("车身描述");//车身描述
}
//拧紧相关
//ByPass
function byPassTgFun(allStaticInfo, obj, plineType, stationCarState, matno) {
    var rst = 0;
    AjaxJson('/TightOnly/PassBindTg', { KeyByPass: allStaticInfo, plineType: plineType, WcJobCd: matno, Position: allStaticInfo.DvcLocatn }, function (data) {
        rst = passDealTg(data, matno, allStaticInfo, stationCarState, plineType);
    });
    return rst;
}
function passDealTg(data, matno, allStaticInfo, stationCarState, plineType) {
    var rst = 0;
    if (data.code == 1) {
        updateLog("拧紧Pass提醒:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【工位Job号：" + matno + "】");
        rst = 1;
    } else {
        uperrorLog("拧紧Pass异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【异常信息：" + data.msg + "】");
        if (data.code == 2) {
            passPermisionCheckTg(allStaticInfo, matno, stationCarState, plineType);
        } else if (data.code == 3) {
            setTimeout(function () {
                swal({
                    title: "账号异常",
                    text: data.msg,
                    type: "error",
                    showConfirmButton: false,
                    timer: 1500,
                });
            }, 500);
        }
    }
    return rst;
}
function passPermisionCheckTg(allStaticInfo, matno, stationCarState, plineType) {
    //弹出账号密码输入框
    swal({
        title: "PASS校核",
        html: true,
        type: "input",
        text:
            "账号 <input type='text' name='account' id='account'>" +
            "密码 <input type='password' name='password' id='password'>",
        showCancelButton: true,
        confirmButtonText: "确认",
        cancelButtonText: "取消",
        closeOnConfirm: false,
    }, function (a) {
        swal.close();
        if (a != false) {
            var account = $("#account").val();
            var password = $.md5($("#password").val());
            AjaxJson('/TightOnly/PassBindTg', { KeyByPass: allStaticInfo, plineType: plineType, account: account, password: password, WcJobCd: matno, Position: allStaticInfo.DvcLocatn }, function (data) {
                var rst = passDealTg(data, matno, allStaticInfo, stationCarState, plineType);
                if (rst == 1) {
                    $("#" + matno).attr("class", "Tighten_btn_passed");;
                    $("#" + matno).attr("onclick", "");
                }
            });
        }
    })
}

//获取GUID
function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
//绑定回车
function bindKeyDown(domid, callback) {
    $("#" + domid).bind("keydown", function (e) {
        //兼容浏览器的事件
        let theEvent = e || window.event;
        //兼容各浏览器的键盘事件
        let keyCode = theEvent.keyCode || theEvent.which || theEvent.charCode;
        if (keyCode == 13) {
            callback();
        }
    });
}

function carStart(allStaticInfo, data, plineType) {
    try {
        if (data.code == "1" || data.code == 1) {
            allStaticInfo["VIN"] = data.vinInfo[0].vin;
            allStaticInfo["OrderCd"] = data.vinInfo[0].ordercd;
            allStaticInfo["SequenceNo"] = data.vinInfo[0].pastno;
            allStaticInfo["BodyNo"] = data.vinInfo[0].bodyno;
            allStaticInfo["ProductMatCd"] = data.vinInfo[0].matcd;
            allStaticInfo["CarType"] = data.vinInfo[0].cartype;
            allStaticInfo["CarColor1"] = data.vinInfo[0].carcolor1;
            //左侧加载图片
            CarPicture(data.vinInfo[0].cartype + data.vinInfo[0].carcolor1);
            $("#vin").html(data.vinInfo[0].vin);//Vin码
            $("#sequence").html(data.vinInfo[0].sequenceno);//顺序号
            $("#color").html(data.vinInfo[0].carcolor1);//颜色
            $("#cartype").html(data.vinInfo[0].cartype);//车型
            $("#car_dis").html(data.Product[0].matnm);//车身描述
            //加载装配指示
            let left = "";
            $("#in_main_box_left")[0].innerHTML = "";
            if (data.PartImgs.length > 0) {
                $.each(data.PartImgs, function (i, item) {
                    let src = "data: image/png;base64," + item.matimg;
                    left += `   <div class="part_box_left">
                                <div class="part_dis_box_left baclground">
                                    <p class="part_code_left" id="part_code_left_${item.matid}">${item.matcd}</p>
                                    <p class="part_name_left" id="part_name_left_${item.matid}">${item.matnm}</p>
                                </div>
                                <div class="part_img_box_left">
                                    <img class="part_img_left" id="part_image_left_${item.matnm}" src="${src}" onclick="imgShow(this)">
                                </div>
                            </div>`;
                });
                $("#in_main_box_left").append(left);//为装配指示添加div
            }
        }
    } catch (e) {
        uperrorLog("车身入站错误：【工位：" + allStaticInfo.WcNm + "】【错误信息：" + e + "】");
    }
}


//加载左侧车身照片
function CarPicture(name) {
    AjaxJson("/VideoModule/BBdbR_GuidanceFile/GetPicture?name=" + name, {}, function (data) {
        $("#productpicture").attr("src", 'data:image/' + data.name + ';base64,' + data.file);
    });
}
//工艺指导文件
function getVideo() {
    Dialog("/VideoModule/BBdbR_GuidanceFile/File", "Form", "工艺指导文件", 1000, 600);
}
