function populateListOfComments(comments) {

    let commentContainerDiv = $("#commentContainer");
    commentContainerDiv.html("");

    comments.forEach((cmt) => {
        let commentElem = $("<blockquote></blockquote>");
        commentElem.addClass("blockquote p-2 ml-4");
        let commentHeader = $("<h5></h5>");
        let commentParagraph = $("<p></p>");
        let commentFooter = $("<footer></footer>");
        commentFooter.addClass("blockquote-footer");

        commentHeader.text(cmt.createdBy + ":");
        commentParagraph.text(cmt.content);
        commentFooter.text(moment(cmt.dateAdded).format("DD.MM.YYYY. HH:mm:ss"));
        commentFooter.text(moment(cmt.dateAdded).format("DD.MM.YYYY. HH:mm:ss"));

        commentElem.append(commentHeader, commentParagraph, commentFooter);
        commentContainerDiv.append(commentElem);
    });
}