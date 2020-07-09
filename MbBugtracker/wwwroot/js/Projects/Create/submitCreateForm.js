function submitCreateForm() {
    let projectToCreate = {}; // dto

    projectToCreate["projectName"] = $("#projectName").val();
    projectToCreate["description"] = $("#description").val();
    projectToCreate["selectedUserIds"] = selectedUserIds;
    projectToCreate["projectStatusId"] = $("#projectStatuses").val();

    $.ajax({
        url: "/api/projects/create",
        method: "POST",
        data: JSON.stringify(projectToCreate),
        contentType: 'application/json',
    }).done(data => {
        console.log(data, ' returned ')
        // reset input fields
        $("#projectName").val("");
        $("#description").val("");
        $("#selectedDevelopers").html("");

        toastr.success('Inserted new project!', 'Success!')
    }).fail(er => {
        toastr.error(er.responseJSON);
    })
}