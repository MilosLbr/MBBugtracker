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
            if (er.status == 403) {
                toastr.error("You are forbidden to perform this action!");
            } else {
                toastr.error("An error has ocured!");
            }
        });
}