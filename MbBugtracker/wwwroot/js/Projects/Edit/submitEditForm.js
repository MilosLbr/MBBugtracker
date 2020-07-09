
function submitEditForm(projectEditViewModel, url) {
    projectEditViewModel["projectName"] = $("#projectName").val();
    projectEditViewModel["description"] = $("#description").val();
    projectEditViewModel["selectedUserIds"] = selectedUserIds;
    projectEditViewModel["projectStatusId"] = $("#ProjectStatusId").val();

    $.ajax({
        url: url,
        method: "POST",
        data: JSON.stringify(projectEditViewModel),
        contentType: 'application/json',
    }).done(data => {
        toastr.success("Updated!");
        top.location.href = "/projects/"
    }).fail(er => {
        toastr.error("Edit failed!")
    })
}