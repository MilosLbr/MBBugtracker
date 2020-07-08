function drawAllMyTicketsTable(myTickets, ticketStatuses) {

    let myTicketsTable = $("#myTicketsTable");
    myTicketsTable.DataTable({
        data: myTickets,
        aaSorting: [],
        pageLength: 4,
        bLengthChange: false,
        columns: [
            {
                data: "title",
                render: (data, type, ticket) => {
                    let detailsHref = '/Tickets/Details/' + ticket.id;
                    return "<a target='_blank' href='" + detailsHref + "'>" + ticket.title + "<a>";
                }
            },
            {
                data: "ticketStatusId",
                render: data => {
                    return ticketStatuses[data];
                }
            },
            {
                data: "dueDate",
                render: data => {
                    let date = new Date(data);
                    let formated = moment(date).format("DD.MM.YYYY");
                    let dateForSort = moment(date).format("YYYYMMDD");
                    return "<span class='hiddenDate'>" + dateForSort + "</span>" + formated;
                }
            }
        ]
    })
}