function GoUp() {
    $('body,html').animate({ scrollTop: 0 }, 200);
}
function CardUpClose() {
    GoUp();instance.close();
}
