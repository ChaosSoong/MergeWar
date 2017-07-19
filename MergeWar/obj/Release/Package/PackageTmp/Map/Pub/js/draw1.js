//样式;
var style={strokeColor:"red", fillColor:"red",strokeWeight:3, strokeOpacity:0.8,fillOpacity:0.2,strokeStyle:'solid'},overTime;
//读取画线的数据;
showData('line');
//读取多边形的数据;
showData('poly');
//读取长方形的数据;
showData('tangle');
//读取圆的数据;
showData('circle');

//根据type获得后台的数据
function showData(type) {
	$.ajax({
		type:'post',
		url:strProto + lpyhtj + '/CommonMap/show',
		data:{
			type:type,
		},
		success:function  (data) {
			var data=$.parseJSON(data);
			if (type=='circle') {
				for (var i = 0; i < data.length; i++) {
					drawCircle(data[i]);
				};
			} else{
				for (var i = 0; i < data.length; i++) {
					drawParent(data[i],type);
				};
			};
		}
	});
}

//画圆
function  drawCircle(data) {
	var center=new BMap.Point(data.lng,data.lat);
	var radius=data.radius;
	style.strokeColor=data.color;
	style.fillColor=data.color;
	style.strokeWeight=data.strokeWeight;
	style.strokeOpacity=data.strokeOpacity;
	style.fillOpacity=data.fillOpacity;
	style.strokeStyle=data.strokeStyle;
	var oval=new BMap.Circle(center,radius,style);
	addContext(oval,data.id,'circle');
	map.addOverlay(oval);
}

//画覆盖物
function drawParent(data,type) {
	style.strokeColor=data[1].color;
	style.fillColor=data[1].color;
	style.strokeWeight=data[1].strokeWeight;
	style.strokeOpacity=data[1].strokeOpacity;
	style.fillOpacity=data[1].fillOpacity;
	style.strokeStyle=data[1].strokeStyle;	
	var lineData=[];
	for (var i = 0; i < data.length; i++) {
		lineData.push(getPoint(data[i]));
	};

	if (type=='line') {
		var oval = new BMap.Polyline(lineData,style);
		map.addOverlay(oval);
	} else if (type=='poly'||type=='tangle'){
		var oval=new BMap.Polygon(lineData,style);
		map.addOverlay(oval);
	};
	addContext(oval,data[0].parent,type);
}

//返回百度地图的点
function getPoint (data) {
	var lng=data.lng,lat=data.lat;
	return new BMap.Point(lng,lat);
}
function addContext (marker,id,type) {
	var markerMenu = new BMap.ContextMenu();
	//添加右键移除
	// markerMenu.addItem(new BMap.MenuItem('移除', function (e) {
 //        	layer.confirm('您确定要删除此标注吗？',function  (index) {
 //        		map.removeOverlay(marker);
 //        		var parent;
 //            	$.post('delete.php',{id:id,type:type},function  (data) {
 //            		if (data>=1) {
 //            			layer.msg('删除成功！',{icon:1,time:1000,shade:['0.7','#575757']});
 //            		};
 //            	});
 //        		layer.close(index);
 //        	});
 //    }, 1));
    marker.addContextMenu(markerMenu);
}