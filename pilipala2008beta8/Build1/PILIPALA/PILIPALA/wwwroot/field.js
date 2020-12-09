/* 查看文章，节流控制（AJAX） */
function showPost(ID, push = true) {

    debounce(showPost_origin(ID, push), 1000);

}

/* 查看文章，无防护（AJAX） */
function showPost_origin(ID, push) {
    $.ajax({
        type: "post",
        url: "/@/" + ID,
        data: "",
        dataType: "html",/* html返回类型 */
        beforeSend: function () {
            $("#CardCol").append('<div class="LoadLine"></div>');
        },
        success: function (result) {
            /* 刷入新内容 */
            $("#CardCol>.Col").html(result);

            /* 更改URL地址 */

            const state = { 'ID': ID };
            if (push == true) {
                history.pushState(state, '', '/' + (ID == -1 ? "" : ID));
            }

            refre_UVCount(ID);/* 刷新UVCount计数 */
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

/* 技术验证型评论提交（不安全） */
function Captcha(PostID, HEAD, Content, User, Email, WebSite) {
    axios({
        method: "post",
        url: "/guest/CommentLakeCaptcha",
        data: Qs.stringify({
            PostID: PostID,
            HEAD: HEAD,
            Content: Content,
            User: User,
            Email: Email,
            WebSite: WebSite,
        }),
    })
        .then((response) => {
            showPost(PostID);
            console.log(result.d);
        })
        .catch(function (error) {
            console.log(error); //请求失败处理
        });
}

/* 刷新StarCount计数（AJAX） */
function refre_StarCount(ID) {
    if ($.cookie('isStar' + ID) == 'true') {
        /* 如果cookie显示目前文本已经点赞 */
        $.cookie('isStar' + ID, 'false', { expires: 1 });/* 设置为取消点赞，并设置cookie时效（天） */

        axios({
            method: "post",
            url: "/guest/Decrease_StarCount_by_PostID",
            data: Qs.stringify({
                PostID: ID,
            }),
        })
            .then((response) => {
                starOpacity050();/* 透明度0.5 */
                $(".StarCount").text(response.data);
                console.log(response);
            })
            .catch(function (error) {
                console.log(error); //请求失败处理
            });
    } else {
        $.cookie('isStar' + ID, 'true', { expires: 1 });

        axios({
            method: "post",
            url: "/guest/Increase_StarCount_by_PostID",
            data: Qs.stringify({
                PostID: ID,
            }),
        })
            .then((response) => {
                starOpacity100();/* 透明度1 */
                $(".StarCount").text(response.data);
                console.log(response);
            })
            .catch(function (error) {
                console.log(error); //请求失败处理
            });
    }
};

/* 刷新UVCount计数（AJAX） */
function refre_UVCount(ID) {
    if ($.cookie('isSaw' + ID) == 'true') {/* 如果cookie显示目前文本已经浏览不做处理 */ }
    else {
        /* 未被浏览 */
        $.cookie('isSaw' + ID, 'true', { expires: 1 });

        axios({
            method: "post",
            url: "/guest/Increase_UVCount_by_PostID",
            data: Qs.stringify({
                PostID: ID,
            }),
        })
            .then((response) => {
                $(".UVCount").text(response.data);
                console.log(response);
            })
            .catch(function (error) {
                console.log(error); //请求失败处理
            });
    }
};