

//VIN码入站
function manualCarStart(allStaticInfo, vin, del, plineType, gid) {
    if (del == undefined) {
        del = true;
    } if (plineType == undefined) {
        plineType = "主线"
    }
    if (del) {
        AjaxJson('/OT/GetCarInfoByStf', { VIN: vin, WcId: allStaticInfo.WcId, del: del, plineType: plineType }, function (data) {
            if (data.code == 1 || data.code == 2) {
                $("#in_main_box_left")[0].innerHTML = "";
                $("#in_main_box_right")[0].innerHTML = "";
                if (allStaticInfo.DvcCatg == '多屏协同') {
                    data["gid"] = gid;
                    publishMqtt(JSON.stringify(data), "/OTScreen/NewVin/" + allStaticInfo.WcId);
                }
                if (carStart(allStaticInfo,data, plineType)) {
                    if (del == true) {
                        updateLog("车身入站成功:【工位：" + allStaticInfo.WcNm + "】【VIN：" + data.vinInfo[0].vin + "】");
                    }
                    if (plineType == "辅线" && $(".part_dis_box_right").length == 0) {
                        var i = 0;
                        setTimeout(function () {
                            swal(
                                {
                                    title: data.vinInfo[0].vin,
                                    text: "当前车身未配置物料，将在5s后自动加载后续车身",
                                    confirmButtonText: "提前刷新",
                                    cancelButtonText: "取消刷新",
                                    timer: 5000,
                                    closeOnConfirm: false,
                                    //closeOnCancel: false,
                                    //showLoaderOnConfirm: true,
                                    showCancelButton: true,
                                },
                                function (a) {
                                    swal.close();
                                    if (i++ == 0 && a != false) {
                                        getnewCar(allStaticInfo);
                                    }
                                });
                        }, 500);
                    }
                }
                return;
            }
            else if (data.code == -2) uperrorLog("车身入站异常：【工位：" + allStaticInfo.WcNm + "】【VIN：" + vin + "】【异常信息：" + data.msg + "】");
            else uperrorLog("车身入站错误：【工位：" + allStaticInfo.WcNm + "】【VIN：" + vin + "】【错误信息：" + data.msg + "】");
            NotHaveCar();
        });
    }
}
//辅线加载时获取车身信息
function getCar(allStaticInfo) {
    AjaxJson("/OT/getScanStatus", { WcId: allStaticInfo.WcId, type: "辅线" }, function (data) {
        if (data.Code == 1) {
            manualCarStart(allStaticInfo, data.VIN, true, "辅线");
        } else if (data.Code == -1) {
            uperrorLog("车身信息错误：【工位：" + allStaticInfo.WcId + "】【异常信息：" + data.Message + "】");
        } else if (data.Code == 0) {
            uperrorLog("无数据信息:【工位：" + allStaticInfo.WcNm + "】【异常信息：请手动输入VIN码】");
        }
        $("#vin_input").focus();
    });
}

//辅线获取新车
function getnewCar(allStaticInfo) {
    AjaxJson("/OT/getScanStatus2", { Vin: allStaticInfo.VIN }, function (data) {
        if (data.Code == 1) {
            manualCarStart(allStaticInfo, data.VIN, true, "辅线");
        } else if (data.Code == 2) {
            var i = 0;
            setTimeout(function () {
                swal(
                    {
                        title: "无车",
                        text: "当前车身队列无车，60s再次查找",
                        confirmButtonText: "提前刷新",
                        cancelButtonText: "取消刷新",
                        timer: 60000,
                        closeOnConfirm: false,
                        //closeOnCancel: false,
                        //showLoaderOnConfirm: true,
                        showCancelButton: true,
                    },
                    function (a) {
                        swal.close();
                        if (i++ == 0 && a != false) {
                            getnewCar(allStaticInfo);
                        }
                    });
            }, 500);
        } else if (data.Code == -1) {
            uperrorLog("车身信息错误：【工位：" + allStaticInfo.WcId + "】【异常信息：" + data.Message + "】");
        }
    })
}

//入站处理
function carStart(allStaticInfo,data, plineType) {
    try {
        if (data.code == "2" || data.code == 2) {
            NotHaveCar();
            return true;
        }
        parts = data.Parts;
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

        $("#in_main_box_right")[0].innerHTML = "";
        if (data.Parts.length > 0) {
            let right = "";
            if (allStaticInfo.PlineCd =="Line-23") {
                right = `  <div class="part_box_right" id="part_box_right_环保码_1">
                                    <div class="part_btn_box_right" id="part_btn_bind_right_环保码_1">
                                        <input id="part_bind_right_环保码_1" class="part_btn_right" type="button" value="--" onclick="">
                                    </div>
                    <div class="part_dis_box_right" id="part_dis_box_right_环保码_1">
                        <div id="part_code_right_环保码_1" class="part_code_right">Environmental Code</div>
                        <div id="part_name_right_环保码_1" class="part_name_right">环保码(大于45位小于60位)</div>
                    </div>
                    <div class="part_btn_box_right" id="part_btn_box_right_环保码">
                        <input id="part_btn_right_环保码_1" class="part_btn_right" type="button" value="PASS" onclick="byPass(this)">
                    </div>
                </div>`;
            }
            $.each(data.Parts, function (i, item) {
                if (item.isscan == 1 || item.isscan == "1") {
                    for (var i = 1; i < item.matnum + 1; i++) {
                        right += `  <div class="part_box_right" id="part_box_right_${item.matid}_${i}">
                                    <div class="part_btn_box_right" id="part_btn_bind_right_${item.matid}">
                                        <input id="part_bind_right_${item.matid}_${i}" class="part_btn_right" type="button" value="强录" onclick="bind(this)">
                                    </div>
                                    <div class="part_dis_box_right" id="part_dis_box_right_${item.matid}_${i}">
                                        <div id="part_code_right_${item.matid}_${i}" class="part_code_right">${item.matcd}</div>
                                        <div id="part_name_right_${item.matid}_${i}" class="part_name_right">${item.matnm}</div>
                                    </div>
                                    <div class="part_btn_box_right" id="part_btn_box_right_${item.matid}">
                                        <input id="part_btn_right_${item.matid}_${i}" class="part_btn_right" type="button" value="PASS" onclick="byPass(this)">
                                    </div>
                                </div>`;
                    }
                }
            });
            $("#in_main_box_right").append(right);//为扫码添加div
        }
        for (var j = 0; j < data.PartBond.length; j++) {
            var matcd = data.PartBond[j].matcd;
            $.each(data.Parts, function (i, item) {
                if (item.matcd == matcd) {
                    partDeal(item.matid, item.matnum, "green", plineType)
                    return false;
                }
            });
        }
        return true;
    } catch (e) {
        uperrorLog("车身入站错误：【工位：" + allStaticInfo.WcNm + "】【错误信息：" + e + "】");
        return false;
    }
}
//4.2.1根据回车触发关重件绑定
function partBind(allStaticInfo, barcodeRule, parts, BarCode, plineType, stationCarState) {
    let matno;
    if (allStaticInfo.PlineCd == "Line-23" && BarCode.length > 45 && BarCode.length < 60) {
        AjaxJson('/OT/EnvironmentalCodeBind', { VIN: allStaticInfo.VIN, strCode: BarCode }, function (date) {
            if (date.code == 1) {
                partDeal("环保码", "1", "green", plineType);
                updateLog("环保码绑定: 【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【环保码：" + BarCode + "】");
            }
            else {
                uperrorLog("环保码绑定异常: 【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【环保码：" + BarCode + "】【异常信息：" + date.msg + "】");
            }
        });
        return;
    } else {
        $.each(barcodeRule, function (i, RuleItem) {
            var tuhao;
            if (RuleItem["barlength"] != null && BarCode.length != RuleItem["barlength"]) return;
            if (RuleItem["clength"] == null) tuhao = BarCode.substring(RuleItem["cstart"] - 1);
            else tuhao = BarCode.substring(RuleItem["cstart"] - 1, RuleItem["cstart"] + RuleItem["clength"] - 1);
            $.each(parts, function (j, PartItem) {
                if (PartItem.matcd == tuhao) {
                    allStaticInfo["MatId"] = PartItem.matid;
                    allStaticInfo["MatCd"] = PartItem.matcd;
                    allStaticInfo["MatNm"] = PartItem.matnm;
                    matno = PartItem.matnum;
                    allStaticInfo["BarCode"] = BarCode;
                    allStaticInfo["SupplierCd"] = BarCode.substring(RuleItem["bstart"] - 1, RuleItem["bstart"] + RuleItem["blength"] - 1);
                    allStaticInfo["RsvFld1"] = BarCode.substring(RuleItem["astart"] - 1, RuleItem["astart"] + RuleItem["alength"] - 1);//批次号
                    return false;
                }
            });
        });
    }
    if (!!allStaticInfo.MatId) {
        if (matno <= 0) {
            $("#matfinish").show();
            setTimeout(finishhide, 30000);
            uperrorLog("物料完成输入:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + BarCode + "】");
        } else {
            AjaxJson('/OT/PartBind', { KeyPartsBind: allStaticInfo, MatNo: matno, plineType: plineType }, function (data) {
                if (data.code == 1 || data.code == "1") {
                    updateLog("物料绑定提醒:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + BarCode + "】");
                    partDeal(allStaticInfo.MatId, Number(matno), "green", plineType, stationCarState);
                    if (allStaticInfo.DvcCatg == '多屏协同') {
                        var message = {
                            matId: allStaticInfo.MatId,
                            matno: Number(matno),
                            gid: gid
                        };
                        publishMqtt(JSON.stringify(message), "/OTScreen/MatBind/" + allStaticInfo.WcId);
                    }
                }
                else {
                    uperrorLog("物料绑定异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + BarCode + "】【异常信息：" + data.msg + "】");
                }
            });
        }
    } else {
        $("#materro").show();
        setTimeout(errohide, 30000);
        uperrorLog("物料解析异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件条码：" + BarCode + "】【异常信息：未解析到对应物料，请查看条码是否正确】");
    }
    delete allStaticInfo.MatId;
}


function isAmindBind(WcId) {
    var result;
    AjaxJson('/OT/isAmindBind', { WcId: WcId }, function (date) {
        result= date.msg;
    });
    return result;
}
//强制绑定
function bindFun(allStaticInfo, parts, obj, plineType, stationCarState) {
    if (!isAmindBind(allStaticInfo.WcId)) {
        swal({
            title: "无强制录入权限",
            text: "请联系管理员维护工位信息",
            type: "error",
            showConfirmButton: false,
            timer: 1500,
        });
        return false;
    }
    let mat = obj.id.substring(16).split('_');
    let matid = mat[0];
    //if (matid == "环保码") {
    //    partDeal("环保码", "1", "orange", plineType);
    //    return;
    //}
    let matno;
    $.each(parts, function (i, item) {
        if (item.matid == matid) {
            allStaticInfo["MatId"] = item.matid;
            allStaticInfo["MatCd"] = item.matcd;
            allStaticInfo["MatNm"] = item.matnm;
            matno = item.matnum;
            return false;
        }
    });
    swal({
        title: allStaticInfo.MatNm,
        type: "input",
        html: true,
        text:"<input type='text' name='barcode' id='bindbarcode' placeholder='请输入物料码'>",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        animation: "slide-from-top",
        inputPlaceholder: "强制绑定物料条码"
    },
        function (inputValue) {
            if (inputValue === false) return false;
            allStaticInfo["BarCode"] = $("#bindbarcode").val();
            if (allStaticInfo.BarCode === "") {
                swal.showInputError("不能为空！");
                return false;
            }
            allStaticInfo["SupplierCd"] = " ";
            allStaticInfo["RsvFld1"] = " ";//批次号
            AjaxJson('/OT/PartBind', { KeyPartsBind: allStaticInfo, MatNo: matno, plineType: plineType }, function (data) {
                if (data.code == 1 || data.code == "1") {
                    updateLog("物料绑定提醒:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + allStaticInfo.BarCode + "】");
                    partDeal(allStaticInfo.MatId, Number(matno), "green", plineType, stationCarState);
                    if (allStaticInfo.DvcCatg == '多屏协同') {
                        var message = {
                            matId: allStaticInfo.MatId,
                            matno: Number(matno),
                            gid: gid
                        };
                        publishMqtt(JSON.stringify(message), "/OTScreen/MatBind/" + allStaticInfo.WcId);
                    }
                }
                else {
                    uperrorLog("物料绑定异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + allStaticInfo.BarCode + "】【异常信息：" + data.msg + "】");
                }
            });
        });
}


//ByPass
function byPassFun(allStaticInfo, parts, obj, plineType, stationCarState) {
    let mat = obj.id.substring(15).split('_');
    let matid = mat[0];

    if (matid == "环保码") {
        partDeal("环保码", "1", "orange", plineType);
        return;
    }

    let matno;
    $.each(parts, function (i, item) {
        if (item.matid == matid) {
            allStaticInfo["MatId"] = item.matid;
            allStaticInfo["MatCd"] = item.matcd;
            allStaticInfo["MatNm"] = item.matnm;
            matno = item.matnum;
            return false;
        }
    });
    if (matno == 0) {
        $("#matfinish").show();
        setTimeout(finishhide, 30 * 1000);
        uperrorLog("物料完成输入:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【关重件条码：" + BarCode + "】");
    } else {
        allStaticInfo["BarCode"] = "ByPass";
        AjaxJson('/OT/PassBind', { KeyByPass: allStaticInfo, MatNo: matno, plineType: plineType }, function (data) {
            passDeal(data, allStaticInfo, matno, stationCarState, plineType)
        });
    }
}
function passDeal(data, allStaticInfo, matno, stationCarState, plineType) {
    if (data.code == 1) {
        updateLog("物料Pass提醒:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【" + data.msg + "】");
        partDeal(allStaticInfo.MatId, Number(matno), "orange", plineType, stationCarState);
        if (allStaticInfo.DvcCatg == '多屏协同') {
            var message = {
                matId: allStaticInfo.MatId,
                matno: Number(matno),
                gid: gid,
            };
            publishMqtt(JSON.stringify(message), "/OTScreen/MatPass/" + allStaticInfo.WcId);
        }
    } else {
        uperrorLog("物料Pass异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【关重件物料码：" + allStaticInfo.MatCd + "】【关重件名称：" + allStaticInfo.MatNm + "】【异常信息：" + data.msg + "】");
        if (data.code == 2) {
            passPermisionCheck(allStaticInfo, matno, stationCarState, plineType);
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
}
function passPermisionCheck(allStaticInfo, matno, stationCarState, plineType){
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
        //showLoaderOnConfirm: true,
        closeOnConfirm: false,
        //allowOutsideClick: true,
    }, function (a) {
        swal.close();
        if (a != false) {
            var account = $("#account").val();
            var password = $.md5($("#password").val());
            AjaxJson('/OT/PassBind', { KeyByPass: allStaticInfo, MatNo: matno, plineType: plineType, account: account, password: password }, function (data) {
                passDeal(data, allStaticInfo, matno, stationCarState, plineType)
            });
        }
    })
}
//4.2.3绑定/ByPass成功处理函数
function partDeal(matId, matNo, color, plineType, stationCarState) {
    try {
        $("#part_dis_box_right_" + matId + "_" + matNo).attr("class", "part_dis_box_right_select");
        $("#part_dis_box_right_" + matId + "_" + matNo).css("background-color", color);
        $("#part_bind_right_" + matId + "_" + matNo).css("background-color", color);
        $("#part_bind_right_" + matId + "_" + matNo).attr("onclick", "");
        $("#part_btn_right_" + matId + "_" + matNo).css("background-color", color);
        $("#part_btn_right_" + matId + "_" + matNo).attr("onclick", "");
        $.each(parts, function (i, item) {
            if (item.matid == matId) {
                item.matnum = matNo - 1;
            }
        });
        if ($(".part_dis_box_right").length > 0) {
            $("#in_main_box_right").append($("#part_box_right_" + matId + "_" + matNo));
        } else {
            if (plineType=="主线"&& stationCarState == 3) {
                NotHaveCar();
                uperrorLog("车身超时作业:【VIN：" + $("#vin").html() + "】【工位：" + allStaticInfo.WcNm + "】");
            }
            else {
                updateLog("车身完成作业:【VIN：" + $("#vin").html() + "】【工位：" + allStaticInfo.WcNm + "】");
                if (plineType == "辅线" && allStaticInfo.PlineCd != "Line-17" && allStaticInfo.PlineCd != "Line-19") {
                    var i = 0;
                    setTimeout(function () {
                        swal(
                            {
                                title: allStaticInfo.VIN,
                                text: "当前车身完成作业，将在5s后自动加载后续车身",
                                confirmButtonText: "提前刷新",
                                cancelButtonText: "取消刷新",
                                timer: 5000,
                                closeOnConfirm: false,
                                showCancelButton: true,
                                //closeOnCancel: false,
                                //showLoaderOnConfirm: true,
                            },
                            function (a) {
                                swal.close();
                                if (i++ == 0 && a != false) {
                                    getnewCar(allStaticInfo);
                                }
                            });
                    }, 500);
                }
            }
        }
        return true;
    } catch (e) {
        uperrorLog("物料界面处理异常：【工位：" + allStaticInfo.WcNm + "】【错误信息：" + e + "】");
        return false;
    }
}


//界面刷新为无车状态
function NotHaveCar() {
    $("#in_main_box_left")[0].innerHTML = "";
    $("#in_main_box_right")[0].innerHTML = "";
    CarPicture("车身默认照片");
    $("#vin").html("当前工位无车");//Vin码
    $("#sequence").html("顺序号");//顺序号
    $("#color").html("颜色");//颜色
    $("#cartype").html("车型");//车型
    $("#car_dis").html("车身描述");//车身描述
}

//加载左侧车身照片
function CarPicture(name) {
    AjaxJson("/VideoModule/BBdbR_GuidanceFile/GetPicture?name=" + name, {}, function (data) {
        $("#productpicture").attr("src", 'data:image/' + data.name + ';base64,' + data.file);
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

//加载界面的宽高
function SetAllWH() {
    var width = $(window).width();
    var height = $(window).height();
    $("#all").width(width);
    $("#all").height(height);

    $("#matfinish").css("left", (width - 1000) / 2);
    $("#matfinish").css("top", (height - 150) / 2);
    $("#materro").css("left", (width - 1000) / 2);
    $("#materro").css("top", (height - 150) / 2);
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

//加载静态信息
function GetStaticInfo(isMain) {
    try {
        AjaxJson('/OT/GetStaticInfo', null, function (data) {
            if (data.code == 1) {
                allStaticInfo = data.props;
                barcodeRule = data.BarCode;
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

//显示大图
function imgShow(_this) {
    outerdiv = $("#outerdiv");
    innerdiv = $("#innerdiv");
    max_img = $("#max_img");
    var src = _this.src;//获取当前点击的min_img元素中的src属性
    $("#max_img").attr("src", src);//设置#max_img元素的src属性

    /*获取当前点击图片的真实大小，并显示弹出层及大图*/
    $("<img/>").attr("src", src).load(function () {
        var windowW = $(window).width();//获取当前窗口宽度
        var windowH = $(window).height();//获取当前窗口高度
        var realWidth = this.width;//获取图片真实宽度
        var realHeight = this.height;//获取图片真实高度
        var imgWidth, imgHeight;
        var scale = 0.8;//缩放尺寸，当图片真实宽度和高度大于窗口宽度和高度时进行缩放

        if (realHeight > windowH * scale) {//判断图片高度
            imgHeight = windowH * scale;//如大于窗口高度，图片高度进行缩放
            imgWidth = imgHeight / realHeight * realWidth;//等比例缩放宽度
            if (imgWidth > windowW * scale) {//如宽度扔大于窗口宽度
                imgWidth = windowW * scale;//再对宽度进行缩放
            }
        } else if (realWidth > windowW * scale) {//如图片高度合适，判断图片宽度
            imgWidth = windowW * scale;//如大于窗口宽度，图片宽度进行缩放
            imgHeight = imgWidth / realWidth * realHeight;//等比例缩放高度
        } else {//如果图片真实高度和宽度都符合要求，高宽不变
            imgWidth = realWidth;
            imgHeight = realHeight;
        }
        $("#max_img").css("width", imgWidth);//以最终的宽度对图片缩放
        var w = (windowW - imgWidth) / 2;//计算图片与窗口左边距
        var h = (windowH - imgHeight) / 2;//计算图片与窗口上边距
        $(innerdiv).css({ "top": h, "left": w });//设置#innerdiv的top和left属性
        $(outerdiv).fadeIn("fast");//淡入显示#outerdiv及.pimg
    });
    $(outerdiv).click(function () {//再次点击淡出消失弹出层
        $(this).fadeOut("fast");
    });
}

//更新日志信息
function updateLog(data) {
    let html = $("#bottom_text")[0].innerHTML;
    $("#bottom_text")[0].innerHTML = `<p style='color:green;font-size:20px;float:left;margin-left:20px;text-align: left;' >${data}-【时间：${$("#localtime").html()}】</p><br/>` + html;
    AjaxJson("/OT/WriteLog", { PlineCd: allStaticInfo.PlineCd, text: data }, function (data) {
        return false;
    });
}
//更新异常日志信息
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

//工艺指导文件
function getVideo() {
    Dialog("/VideoModule/BBdbR_GuidanceFile/File", "Form", "工艺指导文件", 1000, 600);
}

//弹窗警告
function errohide() {
    $("#materro").hide();
}
function finishhide() {
    $("#matfinish").hide();
}

//获取GUID
function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

//3.本地交互方式函数
////3.1可视界面变化时重新加载界面
//$(window).resize(function () {
//    location.reload()
//});