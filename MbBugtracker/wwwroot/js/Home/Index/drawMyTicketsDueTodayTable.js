function drawMyTicketsDueTodayTable(myTickets, ticketStatuses) {

    let tableData = myTickets.filter(t => {
        let now = new Date().setHours(0, 0, 0, 0);
        let ticketDate = new Date(t.dueDate).setHours(0, 0, 0, 0);

        return now == ticketDate && t.ticketStatusId != 2;
    });

    let myTicketsTodayTable = $("#myTicketsTodayTable");
    myTicketsTodayTable.DataTable({
        data: tableData,
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