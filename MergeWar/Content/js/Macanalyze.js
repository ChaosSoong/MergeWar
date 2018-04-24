function GetAnalyzeType(Atype) {
    switch (Atype) {
        case 1:
            $("#map").after('<form action="/Mac/Analysis" method="post"><div id="condition"><span class="spid">碰撞次数:</span> <input type="hidden" id="ids" value="" name="ids"><input type="hidden" id="txtStartTimes" value="" name="txtStartTimes"><input type="hidden" id="txtEndTimes" value="" name="txtEndTimes"><input type="number" id="size" value="2" min="2" name="size"> <ul id="contentList"></ul><input type="submit" class="analysis" value="开始分析"></div></form>');
            document.title = "碰撞分析";
            //分析获取节点提交到后台
            $("form").submit(function () {
                if (analysis()) {
                    showDown();
                    return true;
                } else {
                    return false;
                }
            })
            break;
        case 2:
            $("#map").after('<form action="/Mac/GowithSwot" method="post"> <div id="condition"><span class="spid">Mac地址:</span><input type="text" id="mac" name="mac"><input type="hidden" id="ids" value="" name="ids"><input type="hidden" id="txtStartTimes" value="" name="txtStartTimes"><input type="hidden" id="txtEndTimes" value=""name="txtEndTimes"><ul id="contentList"></ul><input type="submit" class="analysis" value="开始分析"> </div></form>');
            document.title = "伴随分析";
            //分析获取节点提交到后台
            $("form").submit(function () {
                if (analysis2()) {
                    showDown();
                    return true;
                } else {
                    return false;
                }
            })
            break;
        case 3:
            $("#map").after('<form action="/Mac/AppearDevRes" method="post"> <div id="condition"><input type="hidden" id="ids" value="" name="ids"><input type="hidden" id="txtStartTimes" value="" name="txtStartTimes"><input type="hidden" id="txtEndTimes" value=""name="txtEndTimes"><ul id="contentList"></ul><input type="submit" class="analysis" value="开始分析"> </div></form>');
            $("#contentList").css("top", "0"); $(".analysis").css("marginLeft", "65px");
            document.title = "出现设备";
            //分析获取节点提交到后台
            $("form").submit(function () {
                if (analysis3()) {
                    showDown();
                    return true;
                } else {
                    return false;
                }
            })
            break;
        case 4:
            $("#map").after('<form action="/Mac/DisappearDevRes" method="post"> <div id="condition"><input type="hidden" id="ids" value="" name="ids"><input type="hidden" id="txtStartTimes" value="" name="txtStartTimes"><input type="hidden" id="txtEndTimes" value=""name="txtEndTimes"><ul id="contentList"></ul><input type="submit" class="analysis" value="开始分析"> </div></form>');
            $("#contentList").css("top", "0"); $(".analysis").css("marginLeft", "65px");
            document.title = "消失设备";
            //分析获取节点提交到后台
            $("form").submit(function () {
                if (analysis4()) {
                    showDown();
                    return true;
                } else {
                    return false;
                }
            })
            break;
    }
}
var numval = parseInt($("#num").val());
function analysis() {
    var result = true;
    var Time = true;
    
    var ids = '', txtStartTimes = '', txtEndTimes = '';
    $("#contentList").find("li").each(function (idx, item) {
        var id = $(this).children(".sphtml").text();

        var txtStartTime = $(this).children("input[type='text']").get(0);
        txtStartTime = $(txtStartTime).val();
        txtStartTime = txtStartTime.replace(new RegExp("-", "gm"), "/");
        var StartTime = (new Date(txtStartTime)).getTime();
        StartTime = (StartTime) / 1000;

        var txtEndTime = $(this).children("input[type='text']").get(1);
        txtEndTime = $(txtEndTime).val();
        txtEndTime = txtEndTime.replace(new RegExp("-", "gm"), "/");
        var EndTime = (new Date(txtEndTime)).getTime();
        EndTime = (EndTime) / 1000;

        ids += id + '_';
        txtStartTimes += StartTime + '_';
        txtEndTimes += EndTime + '_';
    });
    $("#ids").val(ids);
    $("#txtStartTimes").val(txtStartTimes);
    $("#txtEndTimes").val(txtEndTimes);
    var num = ids.split(",");
    if (num.length > 500) {
        result = false;
        alert("设备ID不能超过500个");
    }
    if ($("#contentList li").length <= 1) {
        alert("最少选择两个区域碰撞");
        result = false;
    }
    if ($("#contentList li").length > 5) {
        alert("选择的区域碰撞不能超过5个");
        result = false;
    }
    $("#contentList").find("li input[type='text']").each(function () {
        if ($.trim($(this).val()) == "") {
            result = false;
            Time = false;
        }
    })
    if (Time == false) {
        alert("时间不能为空!")
    }
    if (Time == true && result == true) {
        return result;
    }
}

function analysis2() {
    var reg = new RegExp(/[A-Fa-f\d]{2}[-:][A-Fa-f\d]{2}[-:][A-Fa-f\d]{2}[-:][A-Fa-f\d]{2}[-:][A-Fa-f\d]{2}[-:][A-Fa-f\d]{2}/);
    var result = true;
    var Time = true;
    var ids = '', txtStartTimes = '', txtEndTimes = '';
    $("#contentList").find("li").each(function (idx, item) {
        var id = $(this).children(".sphtml").text();
       
        var txtStartTime = $(this).children("input[type='text']").get(0);
        txtStartTime = $(txtStartTime).val();
        txtStartTime = txtStartTime.replace(new RegExp("-", "gm"), "/");
        var StartTime = (new Date(txtStartTime)).getTime();
        StartTime = (StartTime) / 1000;

        var txtEndTime = $(this).children("input[type='text']").get(1);
        txtEndTime = $(txtEndTime).val();
        txtEndTime = txtEndTime.replace(new RegExp("-", "gm"), "/");
        var EndTime = (new Date(txtEndTime)).getTime();
        EndTime = (EndTime) / 1000;

        ids += id + '_';
        txtStartTimes += StartTime + '_';
        txtEndTimes += EndTime + '_';
    });
    $("#ids").val(ids);
    $("#txtStartTimes").val(txtStartTimes);
    $("#txtEndTimes").val(txtEndTimes);
    $("#contentList").find("li input[type='text']").each(function () {
        if ($.trim($(this).val()) == "") {
            Time = false;
        }
    })
    var num = ids.split(",");
    if (num.length > 500) {
        result = false;
        alert("设备ID不能超过500个");
    }
    if ($("#mac").val() == "") {
        result = false;
        alert("请输入MAC地址");
    } else if (!reg.test($("#mac").val())) {
        result = false;
        alert("设备MAC地址由字母或数字和-或:组成");
    }
    if ($("#contentList li").length > 5) {
        alert("选择的区域碰撞不能超过5个");
        result = false;
    }
    if (Time == false) {
        alert("时间不能有空项")
    }
    if (Time == true && result == true) {
        return result;
    }
}
function analysis3() {
    var result = true;
    var ids = '', txtStartTimes = '', txtEndTimes = '';
    $("#contentList").find("li").each(function (idx, item) {
        var id = $(this).children(".sphtml").text();
       
        var txtStartTime = $(this).children("input[type='text']").get(0);
        txtStartTime = $(txtStartTime).val();
        txtStartTime = txtStartTime.replace(new RegExp("-", "gm"), "/");
        var StartTime = (new Date(txtStartTime)).getTime();
        StartTime = (StartTime) / 1000;

        var txtEndTime = $(this).children("input[type='text']").get(1);
        txtEndTime = $(txtEndTime).val();
        txtEndTime = txtEndTime.replace(new RegExp("-", "gm"), "/");
        var EndTime = (new Date(txtEndTime)).getTime();
        EndTime = (EndTime) / 1000;

        ids += id + '_';
        txtStartTimes += StartTime + '_';
        txtEndTimes += EndTime + '_';
    });
    $("#ids").val(ids);
    $("#txtStartTimes").val(txtStartTimes);
    $("#txtEndTimes").val(txtEndTimes);
    var num = ids.split(",");
    if (num.length > 500) {
        result = false;
        alert("设备ID不能超过500个");
    }
    if ($("#contentList li").length != 1) {
        result = false;
        alert("只能选择一个区域进行分析");
    } else if ($("#txtEndTime1R").val() == "") {
        result = false;
        alert("结束时间不能为空");
    }
    return result;
}
function analysis4() {
    var result = true;
    var ids = '', txtStartTimes = '', txtEndTimes = '';
    $("#contentList").find("li").each(function (idx, item) {
        var id = $(this).children(".sphtml").text();
        
        var txtStartTime = $(this).children("input[type='text']").get(0);
        txtStartTime = $(txtStartTime).val();
        txtStartTime = txtStartTime.replace(new RegExp("-", "gm"), "/");
        var StartTime = (new Date(txtStartTime)).getTime();
        StartTime = (StartTime) / 1000;

        var txtEndTime = $(this).children("input[type='text']").get(1);
        txtEndTime = $(txtEndTime).val();
        txtEndTime = txtEndTime.replace(new RegExp("-", "gm"), "/");
        var EndTime = (new Date(txtEndTime)).getTime();
        EndTime = (EndTime) / 1000;

        ids += id + '_';
        txtStartTimes += StartTime + '_';
        txtEndTimes += EndTime + '_';
    });
    $("#ids").val(ids);
    $("#txtStartTimes").val(txtStartTimes);
    $("#txtEndTimes").val(txtEndTimes);
    var num = ids.split(",");
    if (num.length > 500) {
        result = false;
        alert("设备ID不能超过500个");
    }
    if ($("#contentList li").length != 1) {
        result = false;
        alert("只能选择一个区域进行分析");
    } else if ($("#txtEndTime1R").val() == "") {
        result = false;
        alert("结束时间不能为空");
    }
    return result;
}
function del(t) {
    map.removeOverlay(overlay);
    map.removeOverlay(overlayi);
    $(t).parent().remove();
    if ($("#contentList").children().length === 0) {
        $("#map").width("100%");
        $(".BMapLib_Drawing_panel").css("marginRight", "0");
        $("#condition").css("display", "none");
    }
}
var distance = 0.002
$('#button').click(function () {
    distance = Number($("#input").val());
});
// 创建Map实例
var map = new BMap.Map("map", {
    enableMapClick: false
});

// 设置地图背景色
map.getContainer().style.background = '#1e1f2d';
// 初始化地图,设置中心点坐标和地图级别
var point = new BMap.Point(114.067837, 22.556793);
map.centerAndZoom(point, 14);
map.enableScrollWheelZoom(); // 启用滚轮放大缩小。

map.addControl(new BMap.NavigationControl({
    anchor: BMAP_ANCHOR_BOTTOM_LEFT
}));
// 定义一个控件类，即function
function myControl() {
    // 设置默认停靠位置和偏移量
    this.defaultAnchor = BMAP_ANCHOR_TOP_LEFT;
    this.defaultOffset = new BMap.Size(10, 10);
}
// 通过JavaScript的prototype属性继承于BMap.Control
myControl.prototype = new BMap.Control();
// 自定义控件必须实现initialize方法，并且将控件的DOM元素返回
// 在本方法中创建个div元素作为控件的容器，并将其添加到地图容器中
myControl.prototype.initialize = function (map) {
    // 创建一个DOM元素
    var div = document.createElement("div");
    // 添加文字说明
    var input = document.createElement("input");
    input.placeholder = "按回车键搜索";
    input.id = "dataInput";
    div.appendChild(input);
    // 绑定事件
    $(function () {
        $('#dataInput').bind('keypress', function (event) {
            if (event.keyCode == "13") {
                GetAddressLM();
            }
        });
    });
    // 添加DOM元素到地图中
    map.getContainer().appendChild(div);
    // 将DOM元素返回
    return div;
}
// 创建控件实例
var myZoomCtrl = new myControl();
// 添加到地图当中
map.addControl(myZoomCtrl);
var polygons = [];
var mkr = new BMap.Marker(point);

var overlays = [];
var overlaycomplete = function (e) {
    overlays.push(e.overlay);
};
var styleOptions = {
    strokeColor: "blue", //边线颜色。
    fillColor: "blue", //填充颜色。当参数为空时，圆形将没有填充效果。
    strokeWeight: 3, //边线的宽度，以像素为单位。
    strokeOpacity: 0.8, //边线透明度，取值范围0 - 1。
    fillOpacity: 0.2, //填充的透明度，取值范围0 - 1。
    strokeStyle: 'solid' //边线的样式，solid或dashed。
}
//实例化鼠标绘制工具
var drawingManager = new BMapLib.DrawingManager(map, {
    isOpen: false, //是否开启绘制模式
    enableDrawingTool: true, //是否显示工具栏
    drawingToolOptions: {
        anchor: BMAP_ANCHOR_TOP_RIGHT, //位置
        offset: new BMap.Size(20, 5), //偏离值
    },
    circleOptions: styleOptions, //圆的样式
    polylineOptions: styleOptions, //线的样式
    polygonOptions: styleOptions, //多边形的样式
    rectangleOptions: styleOptions //矩形的样式
});
//添加鼠标绘制工具监听事件，用于获取绘制结果
drawingManager.addEventListener('overlaycomplete', overlaycomplete);
var MAX = 100000;
var markers = [];
var ptpt = null;
var pts = [];
$.get("/Public/Generate", function (result) {
    // console.log(result);
    // 点聚合示例
    //var json = eval("(" + result + ")");
    var json = JSON.parse(result);
    for (var i = 0; i < json.length; i++) {
        ptpt = new BMap.Point(json[i].J, json[i].W);
        pts.push(ptpt);
        var marker = new BMap.Marker(ptpt, {
            title: json[i].N,
        });
        var l = new BMap.Label(json[i].ID);
        marker.setLabel(l);
        l.setStyle({ display: "none" });
        markers.push(marker);
        //     marker.hide(); //隐藏标注
    }
    // 最简单的用法，生成一个marker数组，然后调用markerClusterer类即可。
    var markerClusterer = new BMapLib.MarkerClusterer(map, {
        markers: markers
    });
    closeDown();
    // markerClusterer.clearMarkers(); //清除标记
});