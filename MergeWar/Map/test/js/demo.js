var map = new BMap.Map("container"); // 创建地图实例  
var point = new BMap.Point(116.404, 39.915); // 创建点坐标  
map.centerAndZoom(point, 6); // 初始化地图，设置中心点坐标和地图级别
var opts = { anchor: BMAP_ANCHOR_TOP_RIGHT, offset: new BMap.Size(10, 10) } //控件位置
map.addControl(new BMap.NavigationControl());
map.addControl(new BMap.GeolocationControl());
// map.addControl(new BMap.OverviewMapControl());
map.enableScrollWheelZoom(); // 启用滚轮放大缩小。  
map.enableKeyboard(); // 启用键盘操作。 

// 定义自定义覆盖物的构造函数  
function SquareOverlay(center, length, color) {
    this._center = center;
    this._length = length;
    this._color = color;
}
// 继承API的BMap.Overlay    
SquareOverlay.prototype = new BMap.Overlay();
// 实现初始化方法  
SquareOverlay.prototype.initialize = function(map) {
    // 保存map对象实例   
    this._map = map;
    // 创建div元素，作为自定义覆盖物的容器   
    var div = document.createElement("div");
    div.style.position = "absolute";
    // 可以根据参数设置元素外观   
    div.style.width = this._length + "px";
    div.style.height = this._length + "px";
    div.style.background = this._color;
    // 将div添加到覆盖物容器中   
    map.getPanes().markerPane.appendChild(div);
    // 保存div实例   
    this._div = div;
    // 需要将div元素作为方法的返回值，当调用该覆盖物的show、   
    // hide方法，或者对覆盖物进行移除时，API都将操作此元素。   
    return div;
}

// 实现绘制方法   
SquareOverlay.prototype.draw = function() {
    // 根据地理坐标转换为像素坐标，并设置给容器    
    var position = this._map.pointToOverlayPixel(this._center);
    this._div.style.left = position.x - this._length / 2 + "px";
    this._div.style.top = position.y - this._length / 2 + "px";
}

// 实现显示方法    
SquareOverlay.prototype.show = function() {
        if (this._div) {
            this._div.style.display = "";
        }
    }
    // 实现隐藏方法  
SquareOverlay.prototype.hide = function() {
    if (this._div) {
        this._div.style.display = "none";
    }
}

// 添加自定义方法   
SquareOverlay.prototype.toggle = function() {
    if (this._div) {
        if (this._div.style.display == "") {
            this.hide();
        } else {
            this.show();
        }
    }
}

// 添加自定义覆盖物   
var mySquare = new SquareOverlay(map.getCenter(), 100, "red");
// mySquare.hide();
// map.addOverlay(mySquare);
var marker;

function addMarker(point, index) { // 创建图标对象   
    var myIcon = new BMap.Icon("markers.png", new BMap.Size(23, 25), {
        // 指定定位位置。   
        // 当标注显示在地图上时，其所指向的地理位置距离图标左上    
        // 角各偏移10像素和25像素。您可以看到在本例中该位置即是   
        // 图标中央下端的尖角位置。    
        offset: new BMap.Size(10, 25),
        // 设置图片偏移。   
        // 当您需要从一幅较大的图片中截取某部分作为标注图标时，您   
        // 需要指定大图的偏移位置，此做法与css sprites技术类似。    
        imageOffset: new BMap.Size(0, 0 - index * 25) // 设置图片偏移    
    });
    // 创建标注对象并添加到地图   
    marker = new BMap.Marker(point, { icon: myIcon });
    map.addOverlay(marker);
}
// // 随机向地图添加10个标注    
// var bounds = map.getBounds();
// var lngSpan = bounds.ul.lng - bounds.Ll.lng;
// var latSpan = bounds.ul.lat - bounds.Ll.lat;
// console.log(lngSpan);
// for (var i = 0; i < 10; i++) {
//     var point = new BMap.Point(bounds.Ll.lng + lngSpan * (Math.random() * 0.7 + 0.15),
//         bounds.Ll.lat + latSpan * (Math.random() * 0.7 + 0.15));
//     addMarker(point, i);
// }

// marker.addEventListener("click", function(e) {
//     console.log(e);
//     console.log("您点击了标注");
// });

// marker.enableDragging();
// marker.addEventListener("dragend", function(e) {
//     console.log("当前位置：" + e.point.lng + ", " + e.point.lat);
// })
var ps = [];
for (var i = 0; i < 100000; i++) {
    ps.push(new BMap.Point(116.404 + Math.random() * 10, 39.915 + Math.random() * 10));
}

var points = new BMap.PointCollection(ps, { ShapeType: BMAP_POINT_SHAPE_WATERDROP });
map.addOverlay(points);
points.addEventListener("click", e => {
    alert(e.point.lat + "," + e.point.lng);
})
var context = new BMap.ContextMenu();
context.addItem(new BMap.MenuItem("demo", function() { alert(1); }));
context.addItem(new BMap.MenuItem("demo1", function() { alert(2); }, { iconUrl: "markers.png" }));
map.addContextMenu(context); //添加右键菜单


// var panorama = new BMap.Panorama("quan");