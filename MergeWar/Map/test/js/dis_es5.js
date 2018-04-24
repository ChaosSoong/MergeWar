function duandian(p1, p2, dis) {
    if (p2[0] - p1[0] == 0) {
        var point1 = [p1[0] + dis, p1[1]];
        var point2 = [p1[0] - dis, p1[1]];
        return [point1, point2];
    }
    if (p2[1] - p1[1] == 0) {
        var point1 = [p1[0], p1[1] + dis];
        var point2 = [p1[0], p1[1] - dis];
        return [point1, point2];
    } else {

        var b = p1[1] + dis / (Math.sqrt(Math.pow(p2[1] - p1[1], 2) / Math.pow(p2[0] - p1[0], 2)) + 1);
        var a = p1[0] - dis / (Math.sqrt(Math.pow(p2[1] - p1[1], 2) / Math.pow(p2[0] - p1[0], 2)) + 1) * (p2[1] - p1[1]) / (p2[0] - p1[0]);
        var a1 = 2 * p1[0] - a;
        var b1 = 2 * p1[1] - b;
        var point1 = [a1, b1];
        var point2 = [a, b];
        return [point1, point2];
    }
}

function pingxing(a, b, c, dis) {
    var line1 = [a, b, c + dis * Math.sqrt(a * a + b * b)];
    var line2 = [a, b, c - dis * Math.sqrt(a * a + b * b)];
    return [line1, line2];
}

function getABC(p1, p2) {
    var a = p2[1] - p1[1];
    var b = p1[0] - p2[0];
    var c = -a * p1[0] - b * p1[1];
    return [a, b, c];
}

function jiaodian(line1, line2) {
    var a1 = line1[0];
    var b1 = line1[1];
    var c1 = line1[2];
    var a2 = line2[0];
    var b2 = line2[1];
    var c2 = line2[2];
    var y = (a1 * c2 - c1 * a2) / (a2 * b1 - a1 * b2);
    var x = -(b1 * c2 - c1 * b2) / (a2 * b1 - a1 * b2);
    return [x, y];
}

function changeXY(p1, p2, p3, p4) {
    var a = getABC(p1, p2);
    var b = getABC(p3, p4);
    var jiao = jiaodian(a, b);
    if (jiao[0] > p1[0] && jiao[0] < p2[0] || jiao[1] > p1[1] && jiao[1] < p2[1]) {
        return false;
    }
    return true;
}

// function AngleBisector(p1, p2, p3, dis) {
//     var aa = p2[0] + dis / Math.sin(angle / 2) * (1 / Math.sqrt(1 + Math.pow(tmp, 2)));
//     var bb = p2[0] + tmp * dis / Math.sin(angle / 2) * (1 / Math.sqrt(1 + Math.pow(tmp, 2)));
//     var aa1 = 2 * p2[0] - aa;
//     var bb1 = 2 * p2[0] - bb;
//     if (p2[0] + (aa - p2[0]) * (p1[0] - p2[0]) / (p1[0] - p2[0]) > 0) {
//         return [aa, bb, aa1, bb1];
//     } else {
//         return [aa1, bb1, aa, bb];
//     }
// }

function hcq(pointlist, dis) {
    var myline = [];
    var len = pointlist.length;
    var uplist = [];
    var downlist = [];
    for (var k = 0; k < len - 1; k++) {
        // var [a, b, c] = getABC(pointlist[k], pointlist[k + 1]);
        var abc = getABC(pointlist[k], pointlist[k + 1]);
        var a = abc[0], b = abc[1], c = abc[2];
        // var [up, down] = pingxing(a, b, c, dis);
        var updown = pingxing(a, b, c, dis);
        var up = updown[0], down = updown[1];
        uplist.push(up);
        downlist.push(down);
    }

    var len_up = uplist.length;
    var xlist = [];
    var ylist = [];
    for (var i = 0; i < len_up - 1; i++) {
        // var [xx, yy] = jiaodian(uplist[i], uplist[i + 1]); //[a,b]
        var xxyy = jiaodian(uplist[i], uplist[i + 1]);
        var xx = xxyy[0], yy = xxyy[1];
        xlist.push(xx);
        ylist.push(yy);
    }
    // var [p1, p2] = duandian(pointlist[0], pointlist[1], dis);
    // var [p3, p4] = duandian(pointlist[pointlist.length - 1], pointlist[pointlist.length - 2], dis);
    var p1p2 = duandian(pointlist[0], pointlist[1], dis);
    var p3p4 = duandian(pointlist[pointlist.length - 1], pointlist[pointlist.length - 2], dis);
    var p1 = p1p2[0], p2 = p1p2[1], p3 = p3p4[0], p4 = p3p4[1];
    xlist.unshift(p1[0]);
    xlist.push(p3[0]);
    xlist.push(p4[0]);
    ylist.unshift(p1[1]);
    ylist.push(p3[1]);
    ylist.push(p4[1]);

    for (var j = len_up - 2; j >= 0; j--) {
        // var [xx, yy] = jiaodian(downlist[j], downlist[j + 1]);
        var xxyy = jiaodian(downlist[j], downlist[j + 1]);
        var xx = xxyy[0], yy = xxyy[1];
        xlist.push(xx);
        ylist.push(yy);
    }

    xlist.push(p2[0]);
    ylist.push(p2[1]);
    if (changeXY([xlist[0], ylist[0]], [xlist[1], ylist[1]], [xlist[xlist.length - 2], ylist[xlist.length - 2]], [xlist[xlist.length - 1], ylist[xlist.length - 1]])) {
        // return [xlist, ylist];
    } else {
        var t1, t2;
        t1 = xlist[0];
        xlist[0] = xlist[xlist.length - 1];
        xlist[xlist.length - 1] = t1;
        t2 = ylist[0];
        ylist[0] = ylist[ylist.length - 1];
        ylist[ylist.length - 1] = t2;
        // return [xlist, ylist];
    }
    if (changeXY([xlist[xlist.length / 2 - 2], ylist[ylist.length / 2 - 2]], [xlist[xlist.length / 2 - 1], ylist[ylist.length / 2 - 1]], [xlist[xlist.length / 2], ylist[ylist.length / 2]], [xlist[xlist.length / 2 + 1], ylist[ylist.length / 2 + 1]])) {
        // return [xlist, ylist];
    } else {
        var t11, t22;
        t11 = xlist[xlist.length / 2 - 1];
        xlist[xlist.length / 2 - 1] = xlist[xlist.length / 2];
        xlist[xlist.length / 2] = t11;
        t22 = ylist[ylist.length / 2 - 1];
        ylist[ylist.length / 2 - 1] = ylist[ylist.length / 2];
        ylist[ylist.length / 2] = t22;
        // return [xlist, ylist];
    }
    return [xlist, ylist];
}
