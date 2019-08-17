/* 函数节流 */
function throttle(fn, wait) {
    let lastTime = 0
    return function () {
        let nowTime = new Date.getTime()
        if (nowTime - lastTime > wait) {
            fn.apply(this, arguments)
            lastTime = nowTime
        }
    }
}

/* 函数防抖 */
function debounce(fn, dealy) {
    let timer = null
    return function () {
        clearTimeout(timer)
        timer = setTimeout(function () {
            fn.apply(this, arguments)
        }, dealy)
    }
}

/* 返回顶部 */
function up() {
    $('body,html').animate({ scrollTop: 0 }, 200);
}

/* 二段淡入 */
function fadeInX2(obj,fn) {
    $(obj).fadeTo(300, 0.3, function () {
        fn.apply();
        $(obj).fadeTo(160, 0.6,fn);
    });
}

/* 二段淡出 */
function fadeOutX2(obj,fn) {
    $(obj).fadeTo(160, 0.5, function () {
        fn.apply();
        $(obj).fadeOut(100);
    });
}

/* 二段右滑 */
function slideRight2X(obj) {
    $(obj).animate({
        opacity: 1,
        left: '-30px'
    }, 300, function () {
        $(obj).animate({
            left: '12px'
        }, 160);
    });
}

/* 二段左滑 */
function slideLeft2X(obj) {
    $(obj).animate({
        left: '-30px'
    }, 160, function () {
        $(obj).animate({
            opacity: 0,
            left: '-400px'
        }, 100);
    });
}