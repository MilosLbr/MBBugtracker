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
            toastr.error(er.responseText);
        });
}