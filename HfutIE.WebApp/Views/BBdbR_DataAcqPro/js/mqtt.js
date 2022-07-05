var ref2 = null;
var data2;
//设备报警信息
function mQInfo() {
   // client = new Paho.MQTT.Client("127.0.0.1", 8083, "DishMachine"); //建立客户端实例
	client = new Paho.MQTT.Client("10.108.156.4", 8083, "DishMachine11"); //建立客户端实例
    client.connect({
        onSuccess: onConnect
    }); //连接服务器并注册连接成功处理事件
    function onConnect() {
        console.log("onConnected");
        topic1 = '/3666/366601001/deviceToEdge/deviceAlarm'; //订阅的设备报警主题
        client.subscribe(topic1); //订阅主题
        console.log("subscribed");
        topic2 = '/3666/366601001/deviceToEdge/deviceStatus'; //订阅的系统信息主题
        client.subscribe(topic2); //订阅主题
        console.log("subscribed");
        topic3 = '/3666/366601001/edgeToDevice/uploadData'; //订阅的上报温度主题
        client.subscribe(topic3); //订阅主题
		topic4 = '/BMS/QueHuoInfo'; //订阅的缺货信息
		client.subscribe(topic4); //订阅主题
        console.log("subscribed");
        //发送消息
    }
    client.onConnectionLost = onConnectionLost; //注册连接断开处理事件
    client.onMessageArrived = onMessageArrived; //注册消息接收处理事件
    function onConnectionLost(responseObject) {
        if (responseObject.errorCode !== 0) {
            console.log("onConnectionLost:" + responseObject.errorMessage);
            console.log("连接已断开");
        }
    }
    //
    function onMessageArrived(message) {
        // console.log("已收到");
        // console.log("收到消息:" + message.payloadString);
        // console.log("主题：" + message.destinationName);
        //设备报警
        if ('/3666/366601001/deviceToEdge/deviceAlarm' == message.destinationName) {
            let temp1 = jQuery.parseJSON(message.payloadString);
            let alarmCode1 = temp1.alarm;
            let text2 = "";
            let date = new Date;
            let timee = date.getTime();
            for (let i = 0; i < alarmCode1.length; i++) {
                let alarmCode = alarmCode1[i].alarm_code;
                let alarmName = alarmCode1[i].alarm_name;
                let level = alarmCode1[i].level;
                let possibleCause = alarmCode1[i].possible_cause;
                if (possibleCause == null) {
                    possibleCause = "未知";
                }
                let recommendAction = alarmCode1[i].recommend_action;
                if (recommendAction == null) {
                    recommendAction = "暂无";
                }
                let text1 = recommendAction.length;
                let strr = "";
				strr = "<div class=\"sfzcll_box clearfix\" id=" + alarmCode + " name=" + timee + ">\n" +
				    "<div class=\"sfzcll_smallBk\">\n" +
				    "<div class=\"ygy one\">\n" +
				    "<p>" + alarmName + "</p>\n" +
				    "</div>\n" +
				    "</div>\n" +
				    "<div class=\"sfzcll_smallBk\">\n" +
				    "<div class=\"ygy one\">\n" +
				    "<p>" + level + "</p>\n" +
				    "</div>\n" +
				    "</div>\n" +
				    "<div class=\"sfzcll_smallBk\">\n" +
				    "<div class=\"ygy one\">\n" +
				    "<p>" + possibleCause + "</p>\n" +
				    "</div>\n" +
				    "</div>\n" +
				    "<div class=\"sfzcll_smallBk\">\n" +
				    "<div class=\"ygy one\">\n" +
				    "<p>" + recommendAction + "</p>\n" +
				    "</div>\n" +
				    "</div>\n" +
				    "</div>"
					//超过5个字就滚动
                // if (text1 <= 5) {
                //     strr = "<div class=\"sfzcll_box clearfix\" id=" + alarmCode + " name=" + timee + ">\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p>" + alarmName + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p>" + level + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p>" + possibleCause + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p>" + recommendAction + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "</div>"
                // } else {
                //     strr = "<div class=\"sfzcll_box clearfix\" id=" + alarmCode + " name=" + timee + ">\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p id=\"alanames\">" + alarmName + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p id=\"level\">" + level + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\">\n" +
                //         "<p id=\"possibleCause\">" + possibleCause + "</p>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "<div class=\"sfzcll_smallBk\">\n" +
                //         "<div class=\"ygy one\" style=\"height: 45px; !important\">\n" +
                //         "<marquee id=\"recommendAction\">" + recommendAction + "</marquee>\n" +
                //         "</div>\n" +
                //         "</div>\n" +
                //         "</div>"				
                // }
                text2 += strr;
            } //for循环结束

            $(".lei1").append(text2);
            $("#noInfo").remove();

            //超出两个循环滚动
            let s111 = $(".sfzcll_box");
            if (s111.length > 2) {
                if (ref2) clearInterval(ref2);
                loadScrollTable3('DeviceAlarmList');
            }
            let afterInfo = $(".sfzcll_box");
            //定时器
            setTimeout(function() {
                let clearname = $("div[name=" + timee + "]");
                clearname.remove();
                let afterInfo = $(".sfzcll_box");
                if (afterInfo.length == 0) {

                    if (ref2) clearInterval(ref2);
                    let appendNoinfo = "<div id=\"noInfo\">暂 无 数 据</div>"
                    $("#DeviceAlarmList").append(appendNoinfo);
                    $(".lei1").css({
                            marginTop: 0
                        })
                        // 关闭setinterval
                }
            }, 120000);
            // clearAlarm();
        } //设备报警信息处理完毕
        //系统状态
        if ('/3666/366601001/deviceToEdge/deviceStatus' == message.destinationName) {
            let temp1 = jQuery.parseJSON(message.payloadString);
            let systemStatus1 = temp1.status;
            let systemStatus = systemStatus1.system.system_status;
            switch (systemStatus) {
                case "1":
                    systemStatus = "维修";
                    break;
                case "2":
                    systemStatus = "故障停止";
                    break;
                case "3":
                    systemStatus = "未准备好";
                    break;
                case "4":
                    systemStatus = "准备好";
                    break;
                case "5":
                    systemStatus = "待机";
                    break;
                case "6":
                    systemStatus = "停止";
                    break;
                case "7":
                    systemStatus = "运行";
                    break;
				default:
				    systemStatus = "状态未知";
				    break;
            }
            let systemStep = systemStatus1.system.system_step;
            $(".systemstatus").html(systemStatus);
            $(".systemstep").html(systemStatus);
        } //系统状态信息处理完毕
        if ('/3666/366601001/edgeToDevice/uploadData' == message.destinationName) {
            let data1 = jQuery.parseJSON(message.payloadString)
            let data = data1.data;
			let temperature =0;
			for(let i=0;i<data.length;i++){
				if(data[i].upload_name=="temperature"){
					temperature = data[i].upload_value;
					 $('.tempu').html(temperature+"  °C");
				}
			}           
        }
	  //缺货
	  if('/BMS/QueHuoInfo' == message.destinationName){
		    let data = jQuery.parseJSON(message.payloadString);
			let date = new Date;
			let timee1 = date.getTime();
			if(data.length!=0){
				let str = "";
				for(let i=0;i<data.length;i++){		
					let doc = "<li class=\"clearfix\" name="+timee1+"> <span class=\"pulll_left\">"+data[i]["TableCode"]+"</span>\n"
					   +"<span class=\"pulll_right\">"+data[i]["FoodName"]+"</span>\n"
					+"</li>";
					str += doc;		
				}
				$("#ul2").append(str);
				$("#noInfo1").remove();
				//clearScrollTable113('QH');//计时器停止计时
				loadScrollTable113('QH');
				//SetScrollTable113('QH');//计时器重新计时
			} 
			//定时器
			setTimeout(function() {
			    let clearname1 = $("li[name=" + timee1 + "]");
			    clearname1.remove();
				//loadScrollTable113('QH');
				console.log("移走一次");
			    let quehuoInfo = document.getElementById('QH').getElementsByTagName('li');
			    if (quehuoInfo.length == 0) {	
			        let appendNoinfo = "<div id=\"noInfo1\">暂 无 数 据</div>";					
			        $("#QH").append(appendNoinfo);
					$("#ul2").css({
					        marginTop: 0
					    })
			    }
			}, 60000);
		}
    } //message处理结束
	function QueHuo(data){
		
	}
}
mQInfo();
