//侧边栏的显示/隐藏
$(function () {
    $a = $(window).height();
    $("#left").height($a);
    $("#btn").click(function () {
        $("#left").animate({ left: '-66px' });
        $("#btnb").delay(500).animate({ left: '0', zIndex: 555 });
        $("#contentbody").animate({ width: '100%' });
        $("#left").delay(600).css({ display: 'none' });
    });
    $("#btnb").click(function () {
        $("#contentbody").animate({ width: '95%' });
        $("#btnb").animate({ left: '-66px' });
        $("#left").delay(600).animate({ left: '0' });
        $("#left").delay(610).css({ display: 'inline-block' });

    });
});
//顶部系统时间与星期几显示
function getCurDate() {
    var d = new Date();
    var week;
    switch (d.getDay()) {
        case 1: week = "星期一"; break;
        case 2: week = "星期二"; break;
        case 3: week = "星期三"; break;
        case 4: week = "星期四"; break;
        case 5: week = "星期五"; break;
        case 6: week = "星期六"; break;
        default: week = "星期天";
    }
    var years = d.getFullYear();
    var month = add_zero(d.getMonth() + 1);
    var days = add_zero(d.getDate());
    var hours = add_zero(d.getHours());
    var minutes = add_zero(d.getMinutes());
    var seconds = add_zero(d.getSeconds());
    var ndate = years + "-" + month + "-" + days + "    " + hours + ":" + minutes + ":" + seconds + "   " + week;
    time.innerHTML = ndate;
}
function add_zero(temp) {
    if (temp < 10) return "0" + temp;
    else return temp;
};
setInterval("getCurDate()", 100);



//向上向下反复点击伸缩
function myFunction(pe, e) {
    $('.' + e).toggle(100);
    if ($(pe).val() == "显示") {
        $(pe).val("隐藏");
    } else {
        $(pe).val("显示");
    }
};
function myFunction1(e, b, c) {
    $('#' + b).text($("." + c).text());
    $('#' + e).toggle(100);
};
//重置搜索条件
function Reset() {
    $(".contentConditions").find("input[type='text']").val("");
    $("select").each(function () {
        $(this).get(0).selectedIndex = 0;
    });
    $("#sp").html($(e).find("span")[0].innerText);
    $("#sp").attr("value", $(e).attr("value"));
};
function PublicFun(type, url) {
    switch (type) {
        case 1:
            location.href = "/" + url;
            break;
        case 2:
            if (confirm("该操作一旦执行无法撤回，是否继续？")) {
                location.href = "/" + url;
            }
            break;
    }
}
function AlertShow(width, height, url) {
    layer.open({
        title: false,
        type: 2,
        anim: 0,
        shade: 0.5,
        area: [width + 'px', height + 'px'],
        //offset: [y + 'px', x + 'px'],
        closeBtn: false,
        shadeClose: true, //点击遮罩关闭
        content: url
    });
}



