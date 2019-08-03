//用于控制NaviCardBox显示状态的js文件

//显示NaviCardBox
function ShowNaviBox() {
	$(".ShowNaviBtn").css("transform","rotateX(180deg)"); //按钮方向改变
	$(".NaviCardBox").fadeIn(100);

}

//隐藏NaviCardBox
function HideNaviBox() {
	$(".ShowNaviBtn").css("transform","rotateX(0deg)"); //按钮方向改变
	$(".NaviCardBox").fadeOut(100);

}

//监听窗口变化以调整NaviCardBox显示状态
window.onresize = function () {
	if (document.body.clientWidth >= 1025) {
		ShowNaviBox();
	} else {
		HideNaviBox();
	}
}
