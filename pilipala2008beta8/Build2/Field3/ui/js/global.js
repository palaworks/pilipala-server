/* 函数防抖（请求触发后，在指定时间内无额外触发才允许被触发） */
function debounce(fn, delay) {
    var timeout = null;
    return function (e) {
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            fn.apply(this, arguments);
        }, delay);
    };
}

/* 函数节流（触发一次，在指定时间后才能二次触发） */
var throttle = function (fn, delay) {
    var prev = Date.now();
    return function () {
        var context = this;
        var args = arguments;
        var now = Date.now();
        if (now - prev >= delay) {
            fn.apply(context, args);
            prev = Date.now();
        }
    }
}

/* 返回顶部，并隐藏滑条 */
function up() {
    $('body,html').animate({
        scrollTop: 0
    }, 200);
    pcNavList.barSeen = false;

    AvaOutline.style = {
        'border-color': 'rgba(0,0,0,0)'
    }
    CoOutline.style = {
        'border-color': 'rgba(0,0,0,0)'
    }
}

/* 返回顶部 */
function upBtn() {
    $('body,html').animate({
        scrollTop: 0
    }, 200);
}

/* 二段淡入 */
function fadeInX2(obj, fn) {
    $(obj).fadeTo(400, 0.3, function () {
        fn.apply();
        $(obj).fadeTo(320, 0.6, fn);
    });
}

/* 二段淡出 */
function fadeOutX2(obj, fn) {
    $(obj).fadeTo(260, 0.5, function () {
        fn.apply();
        $(obj).fadeOut(200);
    });
}

/* 二段右滑 */
function slideRight2X(obj) {
    $(obj).animate({
        opacity: 1,
        left: '-180px'
    }, 400, function () {
        $(obj).animate({
            left: '12px'
        }, 320);
    });
}

/* 二段左滑 */
function slideLeft2X(obj) {
    $(obj).animate({
        left: '-30px'
    }, 260, function () {
        $(obj).animate({
            opacity: 0,
            left: '-800px'
        }, 200);
    });
}