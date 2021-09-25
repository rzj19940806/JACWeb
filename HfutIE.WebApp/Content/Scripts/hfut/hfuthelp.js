/*20161118 CreateByZhanHang
当前有两个方法：
  1：LodaChartDataB（）：最好用来画具有两个纵列的柱状图，传入三个参数（data:画图元数据，positionid：要填充的div的id，xVar横坐标，yjson：纵坐标）
     纵坐标yjson定义如：var yJson={yVar：["报警次数","持续时间"]}；title_txt：主标题，subtitle：副标题  用此方法之前需要添加
     相应的javascript应用
  2：timeInterval（）：传入两个参数（strStart：形式为“00:00:00”，strEnd ：形式为“00:00:00”）
     最后返回一个xx小时xx分钟xx秒的时间间隔字符串
*/
 
var Hfut = {};
//定义两个二级命名空间，分别处理图标，时间
Hfut.Chart = {};
Hfut.TimeOp = {};
Hfut.ArrayHelp = {};
//andon统计
Hfut.Chart.LodaChartDataB = function (type, data, positionid, xVar, yJson, title_txt, subtitle) {
    var ylength = yJson.yVar.length;
    var length = data.length;
    //取x轴数据
    var jsonx = "{categories:[";
    for (var i = 0; i < length; i++) {
        var dd = "'" + data[i][xVar] + "'";
        jsonx += dd;
        if (i != length - 1) {
            jsonx = jsonx + ",";
        }
    }
    jsonx = jsonx + "],crosshair: true}";
   
    var json = eval('(' + jsonx + ')');               //需要将取得的x轴数据转换成json型
    //取y轴数据
    var jsonys = new Array();
    for (var t = 0; t < ylength; t++) {      
        var jsony = "[";
        for (var j = 0; j < length; j++) {
            jsony += parseFloat(data[j][yJson.yVar[t]]);        //
            if (j != length - 1) {
                jsony = jsony + ",";
            }
        }
        jsony = jsony + "]";
        var json1 = eval('(' + jsony + ')');
        
        //定义y轴对象
        var objy = {
            name: yJson.yVar[t],
            data: json1,
            unit: yJson.yUnit[t],
            yAxis: t
          };
        jsonys.push(objy);
    }
    //设置图表属性
    var chart = new Highcharts.Chart({
        chart: {
            renderTo: positionid,                    //图表位置
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: type,       //图标类型：柱状图
            zoomType: 'x'
        },
        title: {                                //主标题
            text: title_txt
        },
        subtitle: {                             //副标题
            text: subtitle
        },
        xAxis: json,                            //取用上面获得的x轴数据
        //设置y轴
        yAxis: [{                                //Y轴坐标点
            min: 0,
            title: {
                text: '次数 (次)'               //Y轴名称
            },
            allowDecimals: false
        }, {
            min: 0,
            opposite: true,
            title: {
                text: '秒数 (s)'               //Y轴名称
            }

        }],
        tooltip: {                              //工具栏
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
     '<td style="padding:0"><b>{point.y}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                }
            }
        },
        series: jsonys
    });
}
//一次通过率
Hfut.Chart.LodaChartDataC = function (type, data, positionid, xVar, yJson, title_txt, subtitle) {
    var ylength = yJson.yVar.length;
    var length = data.length;
    //取x轴数据
    var jsonx = "{categories:[";
    for (var i = 0; i < length; i++) {
        var dd = "'" + data[i][xVar] + "'";
        jsonx += dd;
        if (i != length - 1) {
            jsonx = jsonx + ",";
        }
    }
    jsonx = jsonx + "],crosshair: true}";

    var json = eval('(' + jsonx + ')');               //需要将取得的x轴数据转换成json型
    //取y轴数据
    var jsonys = new Array();
    for (var t = 0; t < ylength; t++) {
        var jsony = "[";
        for (var j = 0; j < length; j++) {
            jsony += parseFloat(data[j][yJson.yVar[t]]);        //
            if (j != length - 1) {
                jsony = jsony + ",";
            }
        }
        jsony = jsony + "]";
        var json1 = eval('(' + jsony + ')');

        //定义y轴对象
        var objy = {
            name: yJson.yVar[t],
            data: json1,
        };
        jsonys.push(objy);
    }
    //设置图表属性
    var chart = new Highcharts.Chart({
        chart: {
            renderTo: positionid,                    //图表位置
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: type,        //图标类型：柱状图
            zoomType: 'x'
        },
        title: {                                //主标题
            text: title_txt
        },
        subtitle: {                             //副标题
            text: subtitle
        },
        xAxis: json,                            //取用上面获得的x轴数据
        //设置y轴
        yAxis: {
            //Y轴坐标点
            max: 100, // 定义Y轴 最大值  
            min: 0,
            title: {
                text: '一次通过率(%)'               //Y轴名称
            }
        },
        tooltip: {                              //工具栏
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
     '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
        borderWidth: 0,
        dataLabels: {
            enabled: true,
            format: '{point.y:.1f}%'
        }
    }
        },
        series: jsonys
    });
}
//设备OEE
Hfut.Chart.LodaChartDataD = function (type, data, positionid, xVar, yJson, title_txt, subtitle) {
    var ylength = yJson.yVar.length;
    var length = data.length;
    //取x轴数据
    var jsonx = "{categories:[";
    for (var i = 0; i < length; i++) {
        var dd = "'" + data[i][xVar] + "'";
        jsonx += dd;
        if (i != length - 1) {
            jsonx = jsonx + ",";
        }
    }
    jsonx = jsonx + "],crosshair: true}";

    var json = eval('(' + jsonx + ')');               //需要将取得的x轴数据转换成json型
    //取y轴数据
    var jsonys = new Array();
 
    for (var t = 0; t < ylength; t++) {
        var jsony = "[";
        for (var j = 0; j < length; j++) {
            jsony += parseFloat(data[j][yJson.yVar[t]]);        //
            if (j != length - 1) {
                jsony = jsony + ",";
            }
        }
        jsony = jsony + "]";
        var json1 = eval('(' + jsony + ')');

        //定义y轴对象
        var objy = {
            name: yJson.yVar[t],
            data: json1,
        };
        jsonys.push(objy);
    }
    //设置图表属性
    var chart = new Highcharts.Chart({
        //_____________________
        chart: {
            renderTo: positionid,                    //图表位置
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: type,       //图标类型：柱状图
            zoomType: 'x'
             },
    title: {
        text: title_txt
    },
    subtitle: {
            text: document.ontouchstart === undefined ?
                '框选放大' :
                '手势放大'
    },
    xAxis: json, 
    yAxis: {
            max: 100,
            min: 0,
            title: {
            text: '设备OEE (%)'
            }
    },
    tooltip: {
            headerFormat: '<span>{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y:.1f} %</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
    },
    plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:.1f}%'
                }
            }
    },
    series: jsonys 
    });
}
//产品故障次数
Hfut.Chart.LodaChartDataE = function (type, data, positionid, xVar, yJson, title_txt, subtitle) {
    var ylength = yJson.yVar.length;
    var length = data.length;
    //取x轴数据
    var jsonx = "{categories:[";
    for (var i = 0; i < length; i++) {
        var dd = "'" + data[i][xVar] + "'";
        jsonx += dd;
        if (i != length - 1) {
            jsonx = jsonx + ",";
        }
    }
    jsonx = jsonx + "],crosshair: true}";

    var json = eval('(' + jsonx + ')');               //需要将取得的x轴数据转换成json型
    //取y轴数据
    var jsonys = new Array();
    for (var t = 0; t < ylength; t++) {
        var jsony = "[";
        for (var j = 0; j < length; j++) {
            jsony += parseFloat(data[j][yJson.yVar[t]]);        //
            if (j != length - 1) {
                jsony = jsony + ",";
            }
        }
        jsony = jsony + "]";
        var json1 = eval('(' + jsony + ')');

        //定义y轴对象
        var objy = {
            name: yJson.yVar[t],
            data: json1,
        };
        jsonys.push(objy);
    }
    //设置图表属性
    var chart = new Highcharts.Chart({
        chart: {
            renderTo: positionid,                    //图表位置
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: type        //图标类型：柱状图
        },
        title: {                                //主标题
            text: title_txt
        },
        subtitle: {                             //副标题
            text: subtitle
        },
        xAxis: json,                            //取用上面获得的x轴数据
        //设置y轴
        yAxis: {
            //Y轴坐标点
            min: 0,
            title: {
                text: '产品故障次数(次)'               //Y轴名称
            },
            allowDecimals: false
        },
        tooltip: {                              //工具栏
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
     '<td style="padding:0"><b>{point.y}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                  enabled: true,
                    format: '{point.y}次'
                 }
            }
        },
        series: jsonys
    });
}
//设备MTTR
Hfut.Chart.LodaChartDataF= function (type, data, positionid, xVar, yJson, title_txt, subtitle,unit,ytext) {
    var ylength = yJson.yVar.length;
    var length = data.length;
    //取x轴数据
    var jsonx = "{categories:[";
    for (var i = 0; i < length; i++) {
        var dd = "'" + data[i][xVar] + "'";
        jsonx += dd;
        if (i != length - 1) {
            jsonx = jsonx + ",";
        }
    }
    jsonx = jsonx + "],crosshair: true}";

    var json = eval('(' + jsonx + ')');               //需要将取得的x轴数据转换成json型
    //取y轴数据
    var jsonys = new Array();

    for (var t = 0; t < ylength; t++) {
        var jsony = "[";
        for (var j = 0; j < length; j++) {
            jsony += parseFloat(data[j][yJson.yVar[t]]);        //
            if (j != length - 1) {
                jsony = jsony + ",";
            }
        }
        jsony = jsony + "]";
        var json1 = eval('(' + jsony + ')');

        //定义y轴对象
        var objy = {
            name: yJson.yVar[t],
            data: json1,
        };
        jsonys.push(objy);
    }
    //设置图表属性
    var chart = new Highcharts.Chart({
        //_____________________
        chart: {
            renderTo: positionid,                    //图表位置
            plotBackgroundColor: null,
            plotBorderWidth: null,
            type: type,       //图标类型：柱状图
            zoomType: 'x'
        },
        title: {
            text: title_txt
        },
        subtitle: {
            text: document.ontouchstart === undefined ?
                '框选放大' :
                '手势放大'
        },
        xAxis: json,
        yAxis: {
            min: 0,
            title: {
                text: ytext
            }
        },
        tooltip: {
            headerFormat: '<span>{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y}' + unit + '</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y}' + unit
                }
            }
        },
        series: jsonys
    });
}
 
Hfut.TimeOp.timeInterval = function (strStart, strEnd) {
    var hour = 0, min = 0, sce = 0;
    var strArray1 = strStart.split(":");
    var hour1 = parseInt(strArray1[0]);
    var min1 = parseInt(strArray1[1]);
    var sce1 = parseInt(strArray1[2]);
    var strArray2 = strEnd.split(":");
    var hour2 = parseInt(strArray2[0]);
    var min2 = parseInt(strArray2[1]);
    var sce2 = parseInt(strArray2[2]);
    if (hour1 > hour2) {
        return "起始时间大于结束时间，请重新选择";
    } else {
        if (hour1 === hour2) {
            //判断分钟
            if (min1 > min2) {
                return "起始时间大于结束时间，请重新选择";
            } else {
                if (min1 === min2) {
                    //判断秒
                    if (sce1 > sce2) {
                        return "起始时间大于结束时间，请重新选择";
                    } else {
                        //计算秒就行
                        sce = sce2 - sce1;
                    }
                } else {
                    //不要小时的计算
                    if (sce1 > sce2) {
                        sce = sce2 + 60 - sce1;
                        min2 = min2 - 1;
                    } else {
                        sce = sce2 - sce1;
                    }
                    min = min2 - min1;
                }
            }
        } else {

            if (sce1 > sce2) {
                sce = sce2 + 60 - sce1;
                if (min2 === 0) {
                    min2 = 59;
                    hour2 = hour2 - 1;
                }
            } else {
                sce = sce2 - sce1;
            }
            if (min1 > min2) {
                min = min2 + 60 - min1;
                hour2 = hour2 - 1;
            } else {
                min = min2 - min1;
            }
            hour = hour2 - hour1;
        }
    }
    return hour + "小时" + min + "分钟" + sce + "秒";
}
Hfut.ArrayHelp.GroupTrans = function (data, groupfield, tocolumnfield, numfield) {   
    var returnarr = [];
    alter("1");
    for (var i = 0; i < data.length; i++) {
        var id = data[i][groupfield];
        if (returnarr.findobjbyattr(groupfield, id) == null) {
            var obj = {};
            obj[groupfield] = id;
            var idarray = data.findobjsbyattr(groupfield, id);
            for (var j = 0; j < idarray.length; j++) {
                var p = idarray[j][tocolumnfield];
                var v=idarray[j][numfield];
                obj[p] = v;
            }
            returnarr.push(obj);
        }
    }
    alter("3");
    return returnarr;
}