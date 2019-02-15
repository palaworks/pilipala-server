function showDeepin(id) {

    $.ajax({
        type: "post",
        contentType: "application/json",
        url: "../services/indexServ.asmx/loadDeepin",
        data: "{text_id:" + text_id + "}", 
        dataType: "json",
        success: function (result) {
            $.each(result.d, function (index, data) {
                alert(data.id);
            });
        }
    });

};