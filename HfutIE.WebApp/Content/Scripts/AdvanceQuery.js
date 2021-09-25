//高级检索添加条件
function btn_addJs(tableId,tableName) {
    //行号是从0开始，最后一行是新增、删除、保存按钮行 故减去2
    var lengthN = $("#"+tableId+" tr").length;
    var rownum = $("#" + tableId + " tr").length - 1;
    //var chk="<input type='checkbox' id='chk_"+rownum+"' name='chk_"+rownum+"'/>";
    var text="<input type='text' id='txt_"+rownum+"' name='txt_"+rownum+"' class='txt' style='width: 200px'/>";
        
    //动态添加可以进行筛选的条件————————————————
    var colNames = $("#" + tableName + "").jqGrid('getGridParam', 'colNames');
    //获取列字段
    var colModel = $("#" + tableName + "").jqGrid('getGridParam', 'colModel');
    var table = "";
    var newColumnName = [];
    var newColumnValue = [];
    for (var i = 0; i < colNames.length; i++) {
        var columnHidden = colModel[i].hidden;
        var columnName = colModel[i].name;
        if (columnHidden == false && columnName != "rn") {
            newColumnName.push(colNames[i]);
            newColumnValue.push(columnName);
        }
        console.info(columnName);
    }
        
    var option = "";
    for(var i=0;i<newColumnName.length;i++)
    {
        option = option + "<option value='" + newColumnValue[i] + "'>" + newColumnName[i] + "</option>";
    }
    var typ = "<select id='sel_" + rownum + "' class='select' style='width: 140px'>" + option + "</select>";
    //————————————————————

    //添加比较逻辑+————————————————————
    var syms = "";
    var selectoptsVaule = ['equal', 'not equal', 'less', 'less or equal', 'greater', 'greater or equal', 'contains', 'does not contain','starts','ends']
    var selectoptsName = ['等于', '不等于', '小于', '小于或等于', '大于', '大于或等于', '包含', '不包含','以**开始','以**结束'];
    for (var i = 0; i < selectoptsName.length; i++)
    {
        syms = syms + "<option value='" + selectoptsVaule[i] + "'>" + selectoptsName[i] + "</option>";
    }
    var sym = "<select id='sym_" + rownum + "' class='select' style='width: 100px'>" + syms + "</select>";
    //————————————————————

    //添加布尔逻辑
    var bl = "";
    var blValue = ['and','or'];
    var blName = ['并且', '或者'];
    for (var i = 0; i < blName.length; i++) {
        bl = bl + "<option value='" + blValue[i] + "'>" + blName[i] + "</option>";
    }
    var bl = "<select id='bl_" + rownum + "' class='select' style='width: 60px'>" + bl + "</select>";

    var btn = "<input id=btn_" + lengthN + " type='button' value='-' style='height: 15px;vertical-align:middle;text-align:justify;'  onclick='btnDeleteRowJS(this," + tableId + ")' />"
    var row = "<tr><th>检索条件：</th>";
    row = row + "<td>" + typ + "</td><td>" + sym + "</td><td>" + text + "</td><td>" + bl + "</td><td>" + btn + "</td></tr>";
    $(row).insertAfter($("#" + tableId + " tr:eq(" + rownum + ")"));
        
    change();
        
}
function change() {

    var njqgridHight = $(".ui-jqgrid-bdiv").height();
    var newHeight = (njqgridHight - 30);
    $(".ui-jqgrid .ui-jqgrid-bdiv").css("cssText", "height: " + newHeight + "px!important;");

}
//每次只能删除一行，删除多行时出错
function btnDeleteRowJS(obj,tableId) {
    if (obj != 0) {
        var index= obj.parentElement.parentElement.rowIndex
        var num = obj.id;
        var tableId= tableId.id;
        if (index > 0)
        {
            $("#" + tableId + " tr:eq(" + index + ")").remove();
            var njqgridHight = $(".ui-jqgrid-bdiv").height();
            var newHeight = (njqgridHight +30);
            $(".ui-jqgrid .ui-jqgrid-bdiv").css("cssText", "height: " + newHeight + "px!important;");
        }
               
    }
}
//根据多条件进行高级检索
function btn_queryJs(tableId) {
    var tr = $("#" + tableId + " tr");
    var conditon1 = [];
    var n=0;
    var whereCondition = " ";
    //把值为空的过滤
    for (var i = 0; i < tr.length; i++) {
        var tdArr = tr.eq(i).find("td");
        var inputValue = tdArr.eq(2).find('input').val();// 输入的检索值
        if (inputValue != "")
        {
            conditon1[n] = n;
            n++
        }
    }
    for (var i = 0; i < conditon1.length; i++) {
        var tdArr = tr.eq(i).find("td");
        var typ = tdArr.eq(0).find('select').val();//需要查询的数据库字段
        var sym = tdArr.eq(1).find('select').val();//查询字段的比较符号
        var inputValue = tdArr.eq(2).find('input').val();// 输入的检索值
        var blValue = tdArr.eq(3).find('select').val();// 语句的布尔逻辑
        if (i + 1 >= conditon1.length) {
            blValue = "";
        }
        whereCondition =" "+ whereCondition + " ";
        //['equal', 'not equal', 'less', 'less or equal', 'greater', 'greater or equal', 'contains', 'does not contain'
        if (sym == "equal") {
            whereCondition = whereCondition + typ + "='" + inputValue + "' " + blValue;
        }
        else if (sym == "not equal") {
            whereCondition = whereCondition + typ + "<>'" + inputValue + "' " + blValue;
        }
        else if (sym == "less") {
            whereCondition = whereCondition + typ + "<'" + inputValue + "' " + blValue;
        }
        else if (sym == "less or equal") {
            whereCondition = whereCondition + typ + "<='" + inputValue + "' " + blValue;
        }
        else if (sym == "greater") {
            whereCondition = whereCondition + typ + ">'" + inputValue + "' " + blValue;
        }
        else if (sym == "greater or equal") {
            whereCondition = whereCondition + typ + ">='" + inputValue + "' " + blValue;
        }
        else if (sym == "contains") {
            whereCondition = whereCondition + "" + typ + " like '%" + inputValue + "%'" + blValue;
        }
        else if (sym == "does not contain") {
            whereCondition = whereCondition + "" + typ + " not like '%" + inputValue + "%'" + blValue;
        }
        else if (sym == "starts") {
            whereCondition = whereCondition + "" + typ + " like '" + inputValue + "%'" + blValue;
        }
        else if (sym == "ends") {
            whereCondition = whereCondition + "" + typ + " like '%" + inputValue + "'" + blValue;
        }
    }
    return whereCondition;

       

}