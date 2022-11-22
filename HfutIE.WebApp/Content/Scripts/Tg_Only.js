//加载界面的宽高
function SetAllWH() {
    var width = $(window).width();
    var height = $(window).height();
    $("#all").width(width);
    $("#all").height(height);
}
//加载静态信息--主线
function GetStaticInfo() {
    try {
            AjaxJson('/TightOnly/GetStaticInfo', null, function (data) {
                if (data.code == 1) {
                    allStaticInfo = data.props;
                    if (allStaticInfo.DvcLocatn != "ALL") {
                        allStaticInfo.WcNm +=allStaticInfo.DvcLocatn;
                    }
                    $("#station").html(allStaticInfo.WcNm);
                    $("#people").html(data.props.StfNm);
                    MqttConnect();      //连接Mqtt
                    updateLog("设备登陆成功:【工位：" + allStaticInfo.WcNm + "】【人员：" + data.props.StfNm + "】");
                } else if (data.code == -1) {
                    uperrorLog("设备登陆错误:【错误信息：" + data.msg + "】");
                    $("#vin_input").attr("readonly", "true");
                }
            });
    } catch (e) {
        uperrorLog("HTML静态信息错误:【错误信息：" + e + "】");
    }
}
//加载静态信息--分装
function GetStaticInfoFZ() {
    try {
        AjaxJson('/TightOnly/GetStaticInfoFZ', null, function (data) {
            if (data.code == 1) {
                allStaticInfo = data.props;
                if (!allStaticInfo.DvcLocatn == "ALL") {
                    allStaticInfo.WcNm += allStaticInfo.DvcLocatn;
                }
                barcodeRule = data.BarCode;
                targetTuhao = data.strs;
                $("#station").html(data.props.WcNm);
                $("#people").html(data.props.StfNm);
                MqttConnect(); 
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
function byPassTgFun(allStaticInfo, obj, matno) {
    var rst = 0;
    AjaxJson('/TightOnly/PassBindTg', { KeyByPass: allStaticInfo, WcJobCd: matno, Position: allStaticInfo.DvcLocatn }, function (data) {
        rst = passDealTg(data, matno, allStaticInfo);
    });
    return rst;
}
function passDealTg(data, matno, allStaticInfo) {
    var rst = 0;
    if (data.code == 1) {
        updateLog("拧紧Pass提醒:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【工位Job号：" + matno + "】");
        rst = 1;
    } else {
        uperrorLog("拧紧Pass异常:【工位：" + allStaticInfo.WcNm + "】【VIN：" + allStaticInfo.VIN + "】【异常信息：" + data.msg + "】");
        if (data.code == 2) {
            passPermisionCheckTg(allStaticInfo, matno);
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
function passPermisionCheckTg(allStaticInfo, matno) {
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
            AjaxJson('/TightOnly/PassBindTg', { KeyByPass: allStaticInfo, account: account, password: password, WcJobCd: matno, Position: allStaticInfo.DvcLocatn }, function (data) {
                var rst = passDealTg(data, matno, allStaticInfo);
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
//加载左侧和中间区域
function carStart(allStaticInfo, data) {
    try {
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


//发动机工位自动入站显示
function manualCarStart(allStaticInfo, vin, isscan) {
    AjaxJson('/TightOnly/VinIn', { VIN: vin, WcId: allStaticInfo.WcId, ShowNm: allStaticInfo.WcNm, IsScan: isscan }, function (data) {
        if (data.Code == 1) {
            var total_content = ``;
            var Group_temp = 1;//Ord
            var right_content = ``;
            //有两把枪的情况
            var total_content2 = ``;
            var Group_temp2 = 1;
            var right_content2 = ``;
            var ID = "";
            var ID1 = "";
            var WcJobCd1 = "";
            var WcJobCd2 = "";
            var Pass1 = false;
            var Pass2 = false;
            total_content += `<div id="tightenArea_${1}" class="tighten_box_Group"> `;//开启第一个Group
            total_content2 += `<div id="tightenArea_${2}" class="tighten_box_Group"> `;//开启第二个Group
            $("#Tighten")[0].innerHTML = "";
            //右侧显示
            if (data.ResultDt.length > 0) {
                WcJobCd1 = data.ResultDt[0]["WcJobCd"];
                $.each(data.ResultDt, function (i, item) {
                    if (item.WcJobCd == WcJobCd1)//如果是第一个JOB号
                    {
                        if (item.IsPass == "Pass") {
                            Pass1 = true;
                        }
                        ID = item.ControllerID;
                        ID1 = item.ControllerPort;
                        var Group_num = item.T_sort;//Num总数
                        var max = item.G_max;//Ord总数
                        var detail_temp = item.Num;//Num
                        if (item.Ord == Group_temp)//跟上一个JOB和拧紧组相同，则不增加拧紧组只扩充detail块
                        {
                            //每个Ord第一个Num加载标题
                            if (detail_temp == 1) {
                                right_content += `<p class="tighten_box_2_ltitle">顺序:</p>
                                    <p class="tighten_box_2_ltitle" >扭矩:</p>
                                    <p class="tighten_box_2_ltitle" >转角:</p>
                                    <p class="tighten_box_2_rtitle" >状态:</p>`
                            }
                            if (item.is_OK == "NG") {
                                right_content += `<p class="tighten_box_2_num">${item.Num}</p>
                                    <p class="tighten_box_2_is_NOK" id="tighten_isOK_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.is_OK}</p>`

                            }
                            else {
                                right_content += `<p class="tighten_box_2_num">${item.Num}</p>
                                    <p class="tighten_box_2_is_OK" id="tighten_isOK_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.is_OK}</p>`
                            }
                            right_content += `<p class="tighten_box_2_num" id="tighten_TT_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.Torque}</p>
                                    <p class="tighten_box_2_num" id="tighten_AA_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.Angle}°</p>`
                        }
                        if (detail_temp == Group_num) {
                            //最后一个Num
                            Group_temp = Group_temp + 1;//刷新组编号
                            total_content += `<div > `;
                            total_content += `<p class="tighten_title_top" >枪【${item.ControllerPort}】---用【${item.TorqueSL}】Nm---拧【${item.T_sort}】次</p> `;
                            total_content += `</div>`;
                            total_content += `<div id="total_job_${WcJobCd1}">`;
                            total_content += right_content;
                            right_content = ``;
                            total_content += `</div>`;//结束右边
                            if (item.Ord == max) {
                                total_content += `</div>`;
                            }
                        }
                    }
                    else {
                        WcJobCd2 = item.WcJobCd;
                        ID = item.ControllerID;
                        ID1 = item.ControllerPort;
                        if (item.IsPass == "Pass") {
                            Pass2 = true;
                        }
                        //获得第二个JOB的区域
                        var Group_num2 = item.T_sort;//Num总数
                        var detail_temp2 = item.Num;//Num
                        var max = item.G_max;
                        if (item.Ord == Group_temp2)//跟上一个JOB和拧紧组相同，则不增加拧紧组只扩充detail块
                        {
                            //每个Ord第一个Num加载标题
                            if (detail_temp2 == 1) {
                                right_content2 += `<p class="tighten_box_2_ltitle">顺序:</p>
                                    <p class="tighten_box_2_ltitle" >扭矩:</p>
                                    <p class="tighten_box_2_ltitle" >转角:</p>
                                    <p class="tighten_box_2_rtitle">状态:</p>`
                            }
                            if (item.is_OK == "NG") {
                                right_content2 += `<p class="tighten_box_2_num">${item.Num}</p>
                                    <p class="tighten_box_2_is_NOK" id="tighten_isOK_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.is_OK}</p>`

                            }
                            else {
                                right_content2 += `<p class="tighten_box_2_num" >${item.Num}</p>
                                    <p class="tighten_box_2_is_OK" id="tighten_isOK_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.is_OK}</p>`
                            }
                            right_content2 += `<p class="tighten_box_2_num" id="tighten_TT_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.Torque}</p>
                                    <p class="tighten_box_2_num" id="tighten_AA_${ID}_${ID1}_${item.Num}_${item.Ord}">${item.Angle}°</p>`
                        }
                        if (detail_temp2 == Group_num2) {
                            //最后一个Num
                            Group_temp2 = Group_temp2 + 1;//刷新组编号
                            total_content2 += `<div > `;
                            total_content2 += `<p class="tighten_title_top" >枪【${item.ControllerPort}】---用【${item.TorqueSL}】N.m---拧【${item.T_sort}】次</p> `;
                            total_content2 += `</div>`;
                            total_content2 += `<div id="total_job_${WcJobCd2}">`;
                            total_content2 += right_content2;
                            right_content2 = ``;
                            total_content2 += `</div>`;//结束右边
                            if (item.Ord == max) {
                                total_content2 += `</div>`;
                            }
                        }
                    }
                });
                $("#Tighten").append(total_content);//为扫码添加div
                //添加ByPass按钮
                var BypassContent = "";
                if (Pass1) {
                    BypassContent = `<input id="${WcJobCd1}" class="Tighten_btn_passed" type="button" value="PASS" onclick="">`;
                }
                else {
                    BypassContent = `<input id="${WcJobCd1}" class="Tighten_btn_pass" type="button" value="PASS" onclick="byPassTighten(this)">`;
                }
                $("#Tighten").append(BypassContent);
                if (total_content2 != "" && WcJobCd2 != "") {
                    $("#Tighten").append(total_content2);//为扫码添加div
                    //添加ByPass按钮
                    var BypassContent2 = "";
                    if (Pass2) {
                        BypassContent2 = `<input id="${WcJobCd2}" class="Tighten_btn_passed" type="button" value="PASS" onclick="">`;
                    }
                    else {
                        BypassContent2 = `<input id="${WcJobCd2}" class="Tighten_btn_pass" type="button" value="PASS" onclick="byPassTighten(this)">`;
                    }
                    $("#Tighten").append(BypassContent2);
                }
                updateLog(data.Msg);
                //左侧显示
                AjaxJson('/TightOnly/ShowInfo', { VIN: data.Show, WcId: allStaticInfo.WcId }, function (data) {
                    carStart(allStaticInfo, data);
                });
            }
        }
        if (data.Code == 0) {
            uperrorLog("拧紧入站失败:【工位：" + allStaticInfo.WcNm + "】【原因：" + data.Msg + "】");
        }
        if (data.Code == -1) {
            uperrorLog("拧紧入站错误:【工位：" + allStaticInfo.WcNm + "】【原因：" + data.Msg + "】");
        }
        if (data.Code == 2) {
            //无任务自动加载下一台
            updateLog(data.Msg);
            //更新当前VIN号
            allStaticInfo["VIN"] = vin;
            var i = 0;
            setTimeout(function () {
                swal(
                    {
                        title: data.vinInfo[0].vin,
                        text: "当前车身没有拧紧任务，将在5s后自动加载后续车身",
                        confirmButtonText: "提前刷新",
                        cancelButtonText: "取消刷新",
                        timer: 5000,
                        closeOnConfirm: false,
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
    });
}
//自动入站新车
function getnewCar(allStaticInfo) {
    AjaxJson("/OT/getScanStatus2", { Vin: allStaticInfo.VIN }, function (data) {
        if (data.Code == 1) {
            var topic = "/KeyParts/AutoTighten/" + allStaticInfo.WcId;
            var msg = `1_${data.VIN}`;
            publishMqtt(msg, topic);//拧紧控制
            manualCarStart(allStaticInfo, data.VIN, false);
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


function CheckPass() {
    if (CheckFinish()) {
        updateLog("车身完成作业:【VIN：" + $("#vin").html() + "】【工位：" + allStaticInfo.WcNm + "】");
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

function CheckFinish() {
    var rst = true;
    $(".tighten_box_2_is_NOK").each(function () {
        var ID = $(this).parent()[0].id;
        var ID1 = ID.split('_')[2];
        var class1 = $("#" + ID1).attr('class');
        if (class1 == "Tighten_btn_pass") {
            rst = false;
            return false;
        }
    })
    return rst;
}


