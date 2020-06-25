function postTicketComment(ticketIdFromView) {
    let content = $("#commentContent").val();

    let postData = {
        content
    }

    $.ajax({
        url: "/api/ticketcomments/" + ticketIdFromView,
        data: JSON.stringify(postData),
        method: "POST",
        contentType: 'application/json',
    })
        .done((comments) => {
            populateListOfComments(comments);
            toastr.success("Posted!");
            $("#commentContent").val("");
        })
        .fail((er) => {
            toastr.error(er.responseText);
        });
}