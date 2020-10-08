/* 查看文章，节流控制（AJAX） */
function showPost(ID) {

    debounce(showPost_origin(ID), 1000);

}

/* 查看文章，无防护（AJAX） */
function showPost_origin(ID) {
    $.ajax({
        type: "post",
        url: "/user/theme/field3/com/内容.cshtml?ID=" + ID,
        data: "",
        dataType: "html",/* html返回类型 */
        beforeSend: function () {
            $("#CardCol").append('<div class="LoadLine"></div>');
        },
        success: function (result) {

            $("#CardCol>.Col").html($(result).find("#CardCol>.Col").html());
            refre_UVCount(ID);/* 刷新UVCount计数 */

            if ($.cookie('isStar' + ID) == 'true') {/* 如果cookie显示目前文本已点赞 */
                starOpacity100();/* 透明度1 */
                $(".StarCount").text($(result).find(".StarCount").text());
            } else {
                starOpacity050();/* 透明度0.5 */
                $(".StarCount").text($(result).find(".StarCount").text());
            }

        },
        complete: function () {
            $('.LoadLine').attr('style', 'animation: LoadLine 0.6s cubic-bezier(0.5, 0.4, 0.5, 1)');
            setTimeout(
                function () {
                    $(".LoadLine").fadeOut(200, function () { $(".LoadLine").remove(); });
                }, 480);
        }
    });
};

/* 返回首页，节流（AJAX） */
function showHome() {

    throttle(showHome_origin(), 1000);

}

/* 返回首页，无防护（AJAX） */
function showHome_origin() {
    $.ajax({
        type: "post",
        url: "/index.cshtml",
        data: "",
        dataType: "html",/* html返回类型 */
        beforeSend: function () {
            $("#CardCol").append('<div class="LoadLine"></div>');
        },
        success: function (result) {
            $("#CardCol>.Col").html($(result).find("#CardCol>.Col").html());/* 以ajax异步请求到的页面#CardCol>.Col替换原有#CardCol>.Col */
        },
        complete: function () {
            $('.LoadLine').attr('style', 'animation: LoadLine 0.6s cubic-bezier(0.5, 0.4, 0.5, 1)');
            setTimeout(
                function () {
                    $(".LoadLine").fadeOut(200, function () { $(".LoadLine").remove(); });
                }, 480);
        }
    });
};

/* 刷新StarCount计数（AJAX） */
function refre_StarCount(ID) {
    if ($.cookie('isStar' + ID) == 'true') {
        /* 如果cookie显示目前文本已经点赞 */
        $.cookie('isStar' + ID, 'false', { expires: 1 });/* 设置为取消点赞，并设置cookie时效（天） */
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/system/serv/SysServ.asmx/StarCount_subs",
            data: "{ID:" + ID + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity050();/* 透明度0.5 */
                $(".StarCount").text(result.d);

                console.log(result.d);
            }
        });
    } else {
        $.cookie('isStar' + ID, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/system/serv/SysServ.asmx/StarCount_plus",
            data: "{ID:" + ID + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity100();/* 透明度1 */
                $(".StarCount").text(result.d);

                console.log(result.d);
            }
        });
    }
};

/* 刷新UVCount计数（AJAX） */
function refre_UVCount(ID) {
    if ($.cookie('isSaw' + ID) == 'true') {/* 如果cookie显示目前文本已经浏览不做处理 */ }
    else {
        /* 未被浏览 */
        $.cookie('isSaw' + ID, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/system/serv/SysServ.asmx/UVCount_plus",
            data: "{ID:" + ID + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".UVCount").text(result.d);
            }
        });
    }
};

/* 推进式文本列表加载（AJAX） */
function loadPost() {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&row=" + $(".content2").length,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $("a").remove(".LoadPostBtn");
            $("#CardCol>.Col").html($("#CardCol>.Col").html() + $(result).find("#CardCol>.Col").html());
        }
    });
}

/* markdown转html */
function mkdConvert(mkdText) {
    var converter = new showdown.Converter();
    $("#CardCol>.Col>.Card>.contain>.Content").html(converter.makeHtml(mkdText));
};

/*星星熄灭/点亮*/
function starOpacity050() {
    $(".AtBox").append("<style id=\"StarCount_tempstyle\">.CoBox>.AtBox>.StarCount:before{opacity: 0.5}</style>");
}
function starOpacity100() {
    $("#StarCount_tempstyle").remove();
}

/*将字数估计的提示输出至.Tip*/
function putWordCount() {
    count = wordCount($("#CardCol>.Col>.Card>.contain>.Content").text());
    time = Math.ceil(count / 400);
    /*空格为全角*/
    $(".ReadTimeTip>.con").text("约 " + count + " 字，阅读成本 " + time + " 分钟");
}

/*字数估计函数*/
function wordCount(data) {
    var pattern = /[a-zA-Z0-9_\u0392-\u03c9]+|[\u4E00-\u9FFF\u3400-\u4dbf\uf900-\ufaff\u3040-\u309f\uac00-\ud7af]+/g;
    var m = data.match(pattern);
    var count = 0;
    if (m == null) { return count; }
    for (var i = 0; i < m.length; i++) {
        if (m[i].charCodeAt(0) >= 0x4E00) {
            count += m[i].length;
        } else {
            count += 1;
        }
    }
    /*向上取整到10位*/
    return Math.ceil(count / 10) * 10;
}
