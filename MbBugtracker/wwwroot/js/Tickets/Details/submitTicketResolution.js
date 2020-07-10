function submitTicketResolution(ticketIdFromView) {
    let ticketId = ticketIdFromView;
    let ticketStatus = $("#selectTicketStatus").val();
    let comment = $("#resolutionComment").val();

    let postData = {
        ticketStatusId: ticketStatus,
        resolutionComment: comment
    }

    $.ajax({
        url: "/api/ticketresolution/" + ticketId,
        data: JSON.stringify(postData),
        method: "POST",
        contentType: 'application/json',
    })
        .done(() => {
            window.location.reload();
        })
        .fail((er) => {
            if (er.status == 403) {
                toastr.error("You are forbidden to perform this action!");
            } else {
                toastr.error("An error has ocured!");
            }
        });
}