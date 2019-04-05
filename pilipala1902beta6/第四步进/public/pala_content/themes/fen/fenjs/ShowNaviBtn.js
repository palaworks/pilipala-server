//用于控制NaviCardBox显示状态的js文件

//显示NaviCardBox
function ShowNaviBox() {
	$(".ShowNaviBtn").css("transform","rotate(180deg)"); //按钮方向改变
	$(".NaviCardBox").slideDown();
}

//隐藏NaviCardBox
function HideNaviBox() {
	$(".ShowNaviBtn").css("transform","rotate(0deg)"); //按钮方向改变
	$(".NaviCardBox").slideUp();
}

//监听窗口变化以调整NaviCardBox显示状态
window.onresize = function () {
	if (document.body.clientWidth >= 1025) {
		ShowNaviBox();
	} else {
		HideNaviBox();
	}
}
