/* ajax查看文本 */
function showText(text_id) {
    $.ajax({
        type: "post",
        url: "/pala_custom/theme/field2/cut/内容.cshtml?text_id=" + text_id,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".CardCol").html($(result).find(".CardCol").html());

            refre_countPv(text_id);/* 刷新count_pv计数 */

            if ($.cookie('isStar' + text_id) == 'true') {/* 如果cookie显示目前文本已点赞 */
                starOpacity100();/* 透明度1 */
                $(".Star").text($(result).find(".Star").text());
            } else {
                starOpacity050();/* 透明度0.5 */
                $(".Star").text($(result).find(".Star").text());
            }
        }
    });
};

/* ajax返回首页 */
function showHome() {
    $.ajax({
        type: "post",
        url: "/Default.aspx",
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".CardCol").html($(result).find(".CardCol").html());/* 以ajax异步请求到的页面.CardCol替换原有.CardCol */
        }
    });
};

/* ajax刷新count_star计数 */
function refre_countStar(text_id) {
    if ($.cookie('isStar' + text_id) == 'true') {/* 如果cookie显示目前文本已经点赞 */
        $.cookie('isStar' + text_id, 'false', { expires: 1 });/* 设置为取消点赞，并设置cookie时效（天） */
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/subs_countStar",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity050();/* 透明度0.5 */
                $(".Star").text(result.d);
            }
        });
    } else {
        $.cookie('isStar' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/plus_countStar",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity100();/* 透明度1 */
                $(".Star").text(result.d);
            }
        });
    }
};

/* ajax刷新count_pv计数 */
function refre_countPv(text_id) {
    if ($.cookie('isSaw' + text_id) == 'true') {/* 如果cookie显示目前文本已经浏览不做处理 */ }
    else {/* 未被浏览 */
        $.cookie('isSaw' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/plus_countPv",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".Pv").text(result.d);
            }
        });
    }
};

/* ajax推进式文本列表加载 */
function loadText() {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&row=" + $(".content2").length,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $("a").remove(".LoadPostBtn");
            $(".CardCol").html($(".CardCol").html() + $(result).find(".CardCol").html());
        }
    });
}

/* markdown转html */
function mkdConvert(mkdText) {
    var converter = new showdown.Converter();
    $(".CardCol>.Card>.contain>.Content").html(converter.makeHtml(mkdText));
};

/*星星熄灭/点亮*/
function starOpacity050() {
    $(".AtBox").append("<style id=\"Star_tempstyle\">.CoBox>.AtBox>.Star:before{opacity: 0.5}</style>");
}
function starOpacity100() {
    $("#Star_tempstyle").remove();
}

/*将字数估计的提示输出至.Tip*/
function putWordCount() {
    count = wordCount($(".CardCol>.Card>.contain>.Content").text());
    time = Math.ceil(count / 400);
    /*空格为全角*/
    $(".CardCol>.Card>.contain>.Tip").text("约 " + count + "字　阅读需要 " + time + "分钟");
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
    return Math.ceil(count / 10)*10;
}
