/**
 * CreateByZhanHang
 * 合肥工业大学智能制造系统及物联装备研究所 Copyright © HfutIE 2017
 */
 
$(function () {
    //向数组里添加根据属性值找对象的方法
    Array.prototype.findobjbyattr = function (attr, val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i][attr] == val)
                return this[i];
        }
        return null;
    };
    //向数组里添加根据属性值找对象数组的方法
    Array.prototype.findobjsbyattr = function (attr, val) {
        var re = [];
        for (var i = 0; i < this.length; i++) {
            if (this[i][attr] == val) { re.push(this[i]); }
        }
        return re;
    };
    //向数组里添加判断是否存在某元素值的方法[数组中为普通元素，不是对象]
    Array.prototype.contains = function (needle) {
        for (i in this) {
            if (this[i] == needle) return true;
        }
        return false;
    }
    //像数组中添加根据属性值删除对象的方法
    Array.prototype.remove = function (attr, val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i][attr] == val) {
                if (i == 0) {
                    this.shift(); //删除并返回数组的第一个元素
                    return;
                }
                else if (i == length - 1) {
                    this.pop();  //删除并返回数组的最后一个元素
                    return;
                }
                else {
                    this.splice(i, 1); //删除下标为i的元素
                    return;
                }
            }
        }
    }
})
//基础数据 
function LoadData(data, gridtableid, afterPositionId) {
    this.data=data;
    this.transData=GroupTrans(data, "Xvar", "Yremark", "Yvar");//数组对象格式转换 根据横坐标属性分类统计
    this.Yremark = GetEleGroup(data, "Yremark");
    /**
    *得到横坐标的数据数组
    */
    this.GetXData=function(){
        var xvar = []; 
        this.transData.forEach(function (v) { xvar.push(v.Xvar); });
        return xvar;
    };
    /**
    *得到纵坐标的对象数据数组
    */
    this.GetYData = function () {
        var yvar = [];
        for (let i = 0; i < this.Yremark.length; i++) {
            var ydata = [];
           //this.transData.forEach(function (v) { ydata.push(v[this.Yremark[i]]); });
            for (let j = 0; j < this.transData.length; j++) {
                ydata.push(this.transData[j][this.Yremark[i]]);
            }
            var obj = {};
            obj.name = this.Yremark[i];
            obj.data = ydata;
            yvar.push(obj);
        }
        return yvar;
    };
    //ColumnObj:[{ShowName:"",AttrName:""},{ShowName:"",AttrName:""}]
    this.GetYDataByColumn = function (ColumnObj) {
        var yvar = [];
        var ydata = [];
        for (let j = 0; j < ColumnObj.length; j++) {
            ydata=  GetAllEle(data, ColumnObj[j]["AttrName"])
            var obj = {};
            obj.name = ColumnObj[j]["ShowName"];
            obj.data = ydata;
            yvar.push(obj);
        }       
        return yvar;
    };
    /**
    *需要转换的数据，要先去除原有的Grid行列信息，重新加载
    *从data中找到加载到grid中的列信息
    *需要加载的列名
    *XvarName：转换后的横坐标值，gridData：Grid对象的列信息，afterPositionId：要加载表格的下一位置元素id
    */
    this.GridLoadTransData = function (XvarName, gridData) {
        // 先移除之前的
        gridData.colModel.splice(0, gridData.colModel.length);//清空数组 
        gridData.colModel.push({ label: XvarName, name: "Xvar", index: "Xvar", width: 150 });
        for (let i = 0; i < this.Yremark.length; i++){
            gridData.colModel.push({ label: this.Yremark[i], name: this.Yremark[i], index: this.Yremark[i], width: 150 });
        }
        $("#gbox_" + gridtableid).remove();
        $("#" + afterPositionId).before("<table id='" + gridtableid + "'></table>");
        $("#" + gridtableid).jqGrid(gridData);
        //加载数据
        for (let i = 0; i < this.transData.length; i++) {
            $("#" + gridtableid).jqGrid('addRowData', i + 1, this.transData[i]);
        }
    }
    //不需要转换的数据,则将数据直接加载到Grid表格中
    this.GridLoadData = function () {
        $("#" + gridtableid).jqGrid("clearGridData");//清空表格中本来数据
        for (let i = 0; i < data.length; i++) {
            $("#" + gridtableid).jqGrid('addRowData', i + 1, data[i]);//加载数据
        }
    }
    //在grid表格中加载新列信息
    this.GridLoadNewColumn = function (gridData, ColumnArr) {
        gridData.colModel.splice(0, gridData.colModel.length);//清空数组 
        for (let i = 0; i < ColumnArr.length; i++) {
            gridData.colModel.push(ColumnArr[i]);
        }
        $("#gbox_" + gridtableid).remove();
        $("#" + afterPositionId).before("<table id='" + gridtableid + "'></table>");
        $("#" + gridtableid).jqGrid(gridData);
    }
    /**
 * 画图
 *  不要为xAxis和series属性赋值
 */
    this.Drawing = function (drawObj) {
        var xdata = this.GetXData();
        var ydata = this.GetYData();
        drawObj["xAxis"] = { categories: xdata };
        drawObj["series"] = ydata;
        var chart = new Highcharts.Chart(drawObj);
    }
    this.DrawingByColumn = function (ColumnObj,drawObj) {
        var xdata = this.GetXData();
        var ydata = this.GetYDataByColumn(ColumnObj);
        drawObj["xAxis"] = { categories: xdata };
        drawObj["series"] = ydata;
        var chart = new Highcharts.Chart(drawObj);
    }
}
//数据格式 {p_line_key:"",key:"",code:"",name:"",rowId:0}
function SelectData() {
    this.selectedData = [];
    //获取所有选中数据的产线主键，返回值为数组
    this.lineKey = function () {
        var arr1 = [];
        this.selectedData.forEach(function (v) {
            if (!arr1.contains(v.p_line_key)) {
                arr1.push(v.p_line_key);
            }
        });
        return arr1;
    }
    //根据产线主键找到选中的数据key
    this.keyByLineKey = function (lineKeyValue) {
        var arr1 = [];
        this.selectedData.forEach(function (v) { if (v.p_line_key == lineKeyValue) { arr1.push(v.key); } });
        return arr1;
    }
    //找到所有选中的数据key
    this.selectedKey = function () {
        var arr1 = [];
        this.selectedData.forEach(function (v) { arr1.push(v.key); });
        return arr1;
    }
    //根据产线主键找到选中的grid行号
    this.rowsIdBylineKey = function (lineKeyValue) {
        var arr1 = [];
        this.selectedData.forEach(function (v) { if (v.p_line_key == lineKeyValue) { arr1.push(v.rowId); } });
        return arr1;
    }
}
function GridData() {
    var gridData = {
        datatype: "json",
        ExpandColumn: "Code",
        height: $(window).height() - 142,
        autowidth: true,
        colModel: [],
       rowNum: 200,
        rowList: [200, 500, 1000,10000],
        pager: "#gridPager",
        sortname: 'addr_name',
        sortorder: 'desc',
        rownumbers: true,
        gridview: true,
        shrinkToFit: false
    };
    return gridData;
}
/**数组对象格式转换 根据横坐标属性分类统计
 * data: [{line:A,day:2016,y:100},{line:A,day:2017,y:20},{line:B,day:2016,y:30},{line:B,day:2017,y:10}]
 *  GroupTrans(data, "day", "line", "y")
 *  结果：[{day:2016,A:100,B:30},{day:2017,A:20,B:10}]
 */
function GroupTrans(data, groupfield, tocolumnfield, numfield) {   
    var returnarr = [];
    for (let i = 0; i < data.length; i++) {
        var id = data[i][groupfield];
        if (returnarr.findobjbyattr(groupfield, id) == null) {
            var obj = {};
            obj[groupfield] = id;
            var idarray = data.findobjsbyattr(groupfield, id);
            for (let j = 0; j < idarray.length; j++) {
                var p = idarray[j][tocolumnfield];
                var v=idarray[j][numfield];
                obj[p] = v;
            }
            returnarr.push(obj);
        }
    }
    return returnarr;
}
//从一组对象数组中找出某个属性的分组集合
function GetEleGroup(data, groupfield) {
    var returnarr = [];
    for (let i = 0; i < data.length; i++) {
        var id = data[i][groupfield];     
        if (!returnarr.contains(id)) {
            returnarr.push(id);
        }
    }
    return returnarr;
}
//从一组对象数组中找出某个属性的所有数据集合
function GetAllEle(data, groupfield) {
    var returnarr = [];
    for (let i = 0; i < data.length; i++) {
        var id = data[i][groupfield];
         returnarr.push(id);
    }
    return returnarr;
}
 
 
